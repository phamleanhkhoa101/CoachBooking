import { useCallback, useEffect, useState } from 'react';
import { api, buildQuery, getErrorMessage } from '../api/client';
import type { ThongKe } from '../api/types';
import { useAuth } from '../auth/AuthContext';
import { useToast } from '../components/Toast';
import { daysAgoISO, formatDate, formatNumber, formatVnd, todayISO } from '../utils/format';

export function AdminDashboard() {
  const { nguoiDung } = useAuth();
  const { showToast } = useToast();
  const [tu, setTu] = useState(daysAgoISO(29));
  const [den, setDen] = useState(todayISO());
  const [data, setData] = useState<ThongKe | null>(null);
  const [loading, setLoading] = useState(true);

  const load = useCallback(
    async (from: string, to: string) => {
      setLoading(true);
      try {
        const res = await api.get<ThongKe>(`/thong-ke${buildQuery({ tu: from, den: to })}`);
        setData(res);
      } catch (error) {
        showToast(getErrorMessage(error), 'error');
      } finally {
        setLoading(false);
      }
    },
    [showToast],
  );

  useEffect(() => {
    void load(tu, den);
  }, [load, tu, den]);

  const chart = data?.doanhThuTheoNgay ?? [];
  const maxRevenue = Math.max(1, ...chart.map((item) => item.doanhThu));
  const maxTuyen = Math.max(1, ...(data?.tuyenPhoBien ?? []).map((item) => item.soVe));

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>Tổng quan</h1>
          <p className="page-subtitle">
            Xin chào {nguoiDung?.hoTen ?? nguoiDung?.tenDangNhap} · Thống kê hoạt động kinh doanh
          </p>
        </div>
        <div className="page-actions filter-inline">
          <div className="field field-compact">
            <label htmlFor="tk-tu">Từ ngày</label>
            <input id="tk-tu" type="date" value={tu} onChange={(event) => setTu(event.target.value)} />
          </div>
          <div className="field field-compact">
            <label htmlFor="tk-den">Đến ngày</label>
            <input id="tk-den" type="date" value={den} onChange={(event) => setDen(event.target.value)} />
          </div>
          <button type="button" className="btn btn-primary" onClick={() => void load(tu, den)} disabled={loading}>
            {loading ? 'Đang tải…' : 'Xem thống kê'}
          </button>
        </div>
      </div>

      <div className="stat-grid">
        <div className="stat-card stat-teal">
          <span className="stat-label">Doanh thu</span>
          <strong className="stat-value">{formatVnd(data?.tongDoanhThu ?? 0)}</strong>
          <span className="stat-hint">Tổng tiền đã thanh toán</span>
        </div>
        <div className="stat-card stat-indigo">
          <span className="stat-label">Số vé bán</span>
          <strong className="stat-value">{formatNumber(data?.soVe ?? 0)}</strong>
          <span className="stat-hint">Vé trong khoảng thời gian</span>
        </div>
        <div className="stat-card stat-amber">
          <span className="stat-label">Chuyến xe</span>
          <strong className="stat-value">{formatNumber(data?.soChuyenXe ?? 0)}</strong>
          <span className="stat-hint">Chuyến đã vận hành</span>
        </div>
        <div className="stat-card stat-rose">
          <span className="stat-label">Khách hàng</span>
          <strong className="stat-value">{formatNumber(data?.soKhachHang ?? 0)}</strong>
          <span className="stat-hint">Khách phát sinh giao dịch</span>
        </div>
      </div>

      <div className="dash-grid">
        <section className="card">
          <div className="card-header">
            <h2>Doanh thu theo ngày</h2>
            <span className="text-muted">{formatDate(tu)} – {formatDate(den)}</span>
          </div>
          {chart.length === 0 ? (
            <div className="empty-state">Chưa có dữ liệu trong khoảng thời gian này.</div>
          ) : (
            <div className="bar-chart">
              {chart.map((item) => {
                const height = Math.max(3, Math.round((item.doanhThu / maxRevenue) * 100));
                return (
                  <div className="bar-col" key={item.ngay} title={`${formatDate(item.ngay)}: ${formatVnd(item.doanhThu)} · ${item.soVe} vé`}>
                    <div className="bar-track">
                      <div className="bar" style={{ height: `${height}%` }} />
                    </div>
                    <span className="bar-label">{formatDate(item.ngay).slice(0, 5)}</span>
                  </div>
                );
              })}
            </div>
          )}
        </section>

        <section className="card">
          <div className="card-header">
            <h2>Tuyến phổ biến</h2>
            <span className="text-muted">Theo số vé</span>
          </div>
          {(data?.tuyenPhoBien ?? []).length === 0 ? (
            <div className="empty-state">Chưa có dữ liệu.</div>
          ) : (
            <div className="rank-list">
              {(data?.tuyenPhoBien ?? []).map((item, index) => (
                <div className="rank-item" key={item.tenTuyen}>
                  <span className="rank-index">{index + 1}</span>
                  <div className="rank-body">
                    <div className="rank-head">
                      <strong>{item.tenTuyen}</strong>
                      <span>{formatNumber(item.soVe)} vé</span>
                    </div>
                    <div className="rank-track">
                      <div className="rank-fill" style={{ width: `${Math.round((item.soVe / maxTuyen) * 100)}%` }} />
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
        </section>
      </div>
    </div>
  );
}
