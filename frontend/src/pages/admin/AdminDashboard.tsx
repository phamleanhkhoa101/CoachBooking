import { useCallback, useEffect, useState } from 'react';
import { api, buildQuery, getErrorMessage } from '../../api/client';
import type { Statistics } from '../../api/types';
import { useAuth } from '../../auth/AuthContext';
import { useToast } from '../../components/common/Toast';
import { daysAgoISO, formatDate, formatNumber, formatVnd, todayISO } from '../../utils/format';

export function AdminDashboard() {
  const { user, nguoiDung } = useAuth();
  const currentUser = user ?? nguoiDung;
  const { showToast } = useToast();
  const [fromDate, setFromDate] = useState(daysAgoISO(29));
  const [toDate, setToDate] = useState(todayISO());
  const [data, setData] = useState<Statistics | null>(null);
  const [loading, setLoading] = useState(true);

  const load = useCallback(
    async (from: string, to: string) => {
      setLoading(true);
      try {
        const res = await api.get<Statistics>(
          `/statistics${buildQuery({ fromDate: from, toDate: to, tu: from, den: to })}`,
        );
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
    void load(fromDate, toDate);
  }, [load, fromDate, toDate]);

  const chart = data?.dailyRevenue ?? data?.doanhThuTheoNgay ?? [];
  const maxRevenue = Math.max(
    1,
    ...chart.map((item) => item.revenue ?? item.doanhThu ?? 0),
  );
  const popularRoutes = data?.popularRoutes ?? data?.tuyenPhoBien ?? [];
  const maxRoute = Math.max(
    1,
    ...popularRoutes.map((item) => item.ticketCount ?? item.soVe ?? 0),
  );

  const displayName =
    currentUser?.fullName ??
    currentUser?.username ??
    currentUser?.hoTen ??
    currentUser?.tenDangNhap ??
    'Quản trị viên';

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>Tổng quan</h1>
          <p className="page-subtitle">
            Xin chào {displayName} · Thống kê hoạt động kinh doanh
          </p>
        </div>
        <div className="page-actions filter-inline">
          <div className="field field-compact">
            <label htmlFor="tk-tu">Từ ngày</label>
            <input
              id="tk-tu"
              type="date"
              value={fromDate}
              onChange={(event) => setFromDate(event.target.value)}
            />
          </div>
          <div className="field field-compact">
            <label htmlFor="tk-den">Đến ngày</label>
            <input
              id="tk-den"
              type="date"
              value={toDate}
              onChange={(event) => setToDate(event.target.value)}
            />
          </div>
          <button
            type="button"
            className="btn btn-primary"
            onClick={() => void load(fromDate, toDate)}
            disabled={loading}
          >
            {loading ? 'Đang tải…' : 'Xem thống kê'}
          </button>
        </div>
      </div>

      <div className="stat-grid">
        <div className="stat-card stat-teal">
          <span className="stat-label">Doanh thu</span>
          <strong className="stat-value">
            {formatVnd(data?.totalRevenue ?? data?.tongDoanhThu ?? 0)}
          </strong>
          <span className="stat-hint">Tổng tiền đã thanh toán</span>
        </div>
        <div className="stat-card stat-indigo">
          <span className="stat-label">Số vé bán</span>
          <strong className="stat-value">
            {formatNumber(data?.ticketCount ?? data?.soVe ?? 0)}
          </strong>
          <span className="stat-hint">Vé trong khoảng thời gian</span>
        </div>
        <div className="stat-card stat-amber">
          <span className="stat-label">Chuyến xe</span>
          <strong className="stat-value">
            {formatNumber(data?.tripCount ?? data?.soChuyenXe ?? 0)}
          </strong>
          <span className="stat-hint">Chuyến đã vận hành</span>
        </div>
        <div className="stat-card stat-rose">
          <span className="stat-label">Khách hàng</span>
          <strong className="stat-value">
            {formatNumber(data?.customerCount ?? data?.soKhachHang ?? 0)}
          </strong>
          <span className="stat-hint">Khách phát sinh giao dịch</span>
        </div>
      </div>

      <div className="dash-grid">
        <section className="card">
          <div className="card-header">
            <h2>Doanh thu theo ngày</h2>
            <span className="text-muted">
              {formatDate(fromDate)} – {formatDate(toDate)}
            </span>
          </div>
          {chart.length === 0 ? (
            <div className="empty-state">Chưa có dữ liệu trong khoảng thời gian này.</div>
          ) : (
            <div className="bar-chart">
              {chart.map((item) => {
                const dateVal = item.date ?? item.ngay ?? '';
                const revVal = item.revenue ?? item.doanhThu ?? 0;
                const ticketVal = item.ticketCount ?? item.soVe ?? 0;
                const height = Math.max(3, Math.round((revVal / maxRevenue) * 100));
                return (
                  <div
                    className="bar-col"
                    key={dateVal}
                    title={`${formatDate(dateVal)}: ${formatVnd(revVal)} · ${ticketVal} vé`}
                  >
                    <div className="bar-track">
                      <div className="bar-fill" style={{ height: `${height}%` }} />
                    </div>
                    <span className="bar-label">{dateVal.slice(8, 10)}</span>
                  </div>
                );
              })}
            </div>
          )}
        </section>

        <section className="card">
          <div className="card-header">
            <h2>Tuyến xe bán chạy</h2>
            <span className="text-muted">Xếp hạng theo số lượng vé</span>
          </div>
          {popularRoutes.length === 0 ? (
            <div className="empty-state">Chưa có dữ liệu tuyến xe.</div>
          ) : (
            <ul className="rank-list">
              {popularRoutes.map((item, index) => {
                const name = item.routeName ?? item.tenTuyen ?? '';
                const count = item.ticketCount ?? item.soVe ?? 0;
                const percent = Math.round((count / maxRoute) * 100);
                return (
                  <li key={name} className="rank-item">
                    <span className="rank-pos">#{index + 1}</span>
                    <div className="rank-main">
                      <div className="rank-header">
                        <strong>{name}</strong>
                        <span className="text-muted">{count} vé</span>
                      </div>
                      <div className="rank-bar-bg">
                        <div className="rank-bar-fg" style={{ width: `${percent}%` }} />
                      </div>
                    </div>
                  </li>
                );
              })}
            </ul>
          )}
        </section>
      </div>
    </div>
  );
}
