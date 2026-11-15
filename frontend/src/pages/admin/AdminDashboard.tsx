import { useCallback, useEffect, useState } from 'react';
import { api, buildQuery, getErrorMessage } from '../../api/client';
import type { Statistics } from '../../api/types';
import { useAuth } from '../../auth/AuthContext';
import { Button } from '../../components/common/Button';
import { EmptyState } from '../../components/common/EmptyState';
import { useToast } from '../../components/common/Toast';
import { daysAgoISO, formatDate, formatNumber, formatVnd, todayISO } from '../../utils/format';

function getThisMonthStartISO(): string {
  const d = new Date();
  return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-01`;
}

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

  const applyPreset = (days: number | 'thisMonth') => {
    const today = todayISO();
    if (days === 'thisMonth') {
      const start = getThisMonthStartISO();
      setFromDate(start);
      setToDate(today);
    } else {
      setFromDate(daysAgoISO(days));
      setToDate(today);
    }
  };

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
    'Quản trị viên';

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>Tổng quan & Thống kê</h1>
          <p className="page-subtitle">
            Xin chào {displayName} · Báo cáo tổng hợp số liệu vận hành Bến xe Miền Đông
          </p>
        </div>

        <div className="page-actions" style={{ display: 'flex', flexDirection: 'column', alignItems: 'flex-end', gap: '8px' }}>
          {/* Quick presets */}
          <div className="preset-chips">
            <button
              type="button"
              className={`preset-chip ${fromDate === todayISO() && toDate === todayISO() ? 'active' : ''}`}
              onClick={() => applyPreset(0)}
            >
              Hôm nay
            </button>
            <button
              type="button"
              className={`preset-chip ${fromDate === daysAgoISO(6) && toDate === todayISO() ? 'active' : ''}`}
              onClick={() => applyPreset(6)}
            >
              7 ngày qua
            </button>
            <button
              type="button"
              className={`preset-chip ${fromDate === daysAgoISO(29) && toDate === todayISO() ? 'active' : ''}`}
              onClick={() => applyPreset(29)}
            >
              30 ngày qua
            </button>
            <button
              type="button"
              className={`preset-chip ${fromDate === getThisMonthStartISO() && toDate === todayISO() ? 'active' : ''}`}
              onClick={() => applyPreset('thisMonth')}
            >
              Tháng này
            </button>
          </div>

          <div className="filter-inline">
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
            <Button
              variant="primary"
              onClick={() => void load(fromDate, toDate)}
              loading={loading}
              disabled={loading}
            >
              Xem thống kê
            </Button>
          </div>
        </div>
      </div>

      {/* KPI STAT CARDS */}
      <div className="stat-grid">
        <div className="stat-card stat-teal">
          <span className="stat-label">Tổng doanh thu</span>
          <strong className="stat-value" style={{ color: 'var(--accent)', letterSpacing: '-0.02em' }}>
            {formatVnd(data?.totalRevenue ?? data?.tongDoanhThu ?? 0)}
          </strong>
          <span className="stat-hint">Doanh thu vé đã thanh toán</span>
        </div>

        <div className="stat-card stat-indigo">
          <span className="stat-label">Số vé đã xuất</span>
          <strong className="stat-value">
            {formatNumber(data?.ticketCount ?? data?.soVe ?? 0)}
          </strong>
          <span className="stat-hint">Số vé đặt thành công</span>
        </div>

        <div className="stat-card stat-amber">
          <span className="stat-label">Chuyến xe phục vụ</span>
          <strong className="stat-value">
            {formatNumber(data?.tripCount ?? data?.soChuyenXe ?? 0)}
          </strong>
          <span className="stat-hint">Chuyến xe trong lịch trình</span>
        </div>

        <div className="stat-card stat-rose">
          <span className="stat-label">Lượng khách hàng</span>
          <strong className="stat-value">
            {formatNumber(data?.customerCount ?? data?.soKhachHang ?? 0)}
          </strong>
          <span className="stat-hint">Khách phát sinh giao dịch</span>
        </div>
      </div>

      <div className="dash-grid">
        {/* DAILY REVENUE BAR CHART */}
        <section className="card">
          <div className="card-header">
            <div>
              <h2>Doanh thu theo ngày</h2>
              <span className="text-muted">
                Khoảng thời gian: {formatDate(fromDate)} – {formatDate(toDate)}
              </span>
            </div>
          </div>
          {chart.length === 0 ? (
            <EmptyState
              title="Chưa có dữ liệu doanh thu"
              description="Không phát sinh giao dịch nào trong khoảng thời gian đã chọn."
            />
          ) : (
            <div className="bar-chart" style={{ padding: '1rem 0.5rem' }}>
              {chart.map((item) => {
                const dateVal = item.date ?? item.ngay ?? '';
                const revVal = item.revenue ?? item.doanhThu ?? 0;
                const ticketVal = item.ticketCount ?? item.soVe ?? 0;
                const height = Math.max(3, Math.round((revVal / maxRevenue) * 100));
                return (
                  <div
                    className="bar-col"
                    key={dateVal}
                    title={`${formatDate(dateVal)}: ${formatVnd(revVal)} (${ticketVal} vé)`}
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

        {/* POPULAR ROUTES RANKING */}
        <section className="card">
          <div className="card-header">
            <div>
              <h2>Tuyến xe bán chạy nhất</h2>
              <span className="text-muted">Xếp hạng theo tổng lượng vé đặt</span>
            </div>
          </div>
          {popularRoutes.length === 0 ? (
            <EmptyState
              title="Chưa có dữ liệu tuyến xe"
              description="Chưa có tuyến xe nào phát sinh đơn vé trong khoảng ngày này."
            />
          ) : (
            <ul className="rank-list" style={{ padding: '0.5rem' }}>
              {popularRoutes.map((item, index) => {
                const name = item.routeName ?? item.tenTuyen ?? '';
                const count = item.ticketCount ?? item.soVe ?? 0;
                const percent = Math.round((count / maxRoute) * 100);
                const rankClass = index === 0 ? 'rank-pos gold' : index === 1 ? 'rank-pos silver' : index === 2 ? 'rank-pos bronze' : 'rank-pos';
                return (
                  <li key={name} className="rank-item">
                    <span className={rankClass} style={{ fontWeight: 800 }}>
                      #{index + 1}
                    </span>
                    <div className="rank-main">
                      <div className="rank-header">
                        <strong>{name}</strong>
                        <span className="text-muted" style={{ fontWeight: 700 }}>
                          {count} vé
                        </span>
                      </div>
                      <div className="rank-bar-bg">
                        <div
                          className="rank-bar-fg"
                          style={{
                            width: `${percent}%`,
                            background: index === 0 ? 'var(--accent)' : 'var(--primary)',
                          }}
                        />
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
