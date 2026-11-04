import { useCallback, useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { api, getErrorMessage } from '../api/client';
import type { HoaDonDetail, HoaDonKhachHang, VeItem } from '../api/types';
import { PaidBadge } from '../components/Badge';
import { FormField } from '../components/FormField';
import { Modal } from '../components/Modal';
import { useToast } from '../components/Toast';
import { formatDateTime, formatVnd } from '../utils/format';

const PAYMENT_METHODS = ['Tiền mặt', 'Chuyển khoản', 'Thẻ ATM'];

export function CustomerHistory() {
  const { showToast } = useToast();
  const [rows, setRows] = useState<HoaDonKhachHang[]>([]);
  const [loading, setLoading] = useState(true);
  const [openId, setOpenId] = useState<number | null>(null);
  const [details, setDetails] = useState<Record<number, HoaDonDetail>>({});
  const [detailLoading, setDetailLoading] = useState(false);

  const [payTarget, setPayTarget] = useState<HoaDonKhachHang | null>(null);
  const [payMethod, setPayMethod] = useState('Tiền mặt');
  const [cancelTarget, setCancelTarget] = useState<{ maHd: number; ve: VeItem } | null>(null);
  const [busy, setBusy] = useState(false);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const data = await api.get<HoaDonKhachHang[]>('/hoa-don/cua-toi');
      setRows(Array.isArray(data) ? data : []);
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setLoading(false);
    }
  }, [showToast]);

  useEffect(() => {
    void load();
  }, [load]);

  const toggleDetail = async (item: HoaDonKhachHang) => {
    if (openId === item.maHd) {
      setOpenId(null);
      return;
    }
    setOpenId(item.maHd);
    if (details[item.maHd]) return;
    setDetailLoading(true);
    try {
      const data = await api.get<HoaDonDetail>(`/hoa-don/${item.maHd}`);
      setDetails((prev) => ({ ...prev, [item.maHd]: data }));
    } catch {
      // Khách hàng có thể không có quyền xem chi tiết: dùng dữ liệu danh sách nếu có.
    } finally {
      setDetailLoading(false);
    }
  };

  const veCuaHoaDon = (item: HoaDonKhachHang): VeItem[] => {
    const detail = details[item.maHd];
    if (detail?.veXes) return detail.veXes;
    return item.veXes ?? [];
  };

  const handlePay = async () => {
    if (!payTarget) return;
    setBusy(true);
    try {
      await api.post(`/hoa-don/${payTarget.maHd}/thanh-toan`, { phuongThuc: payMethod });
      showToast(`Đã thanh toán hóa đơn #${payTarget.maHd}.`);
      setPayTarget(null);
      setDetails((prev) => {
        const next = { ...prev };
        delete next[payTarget.maHd];
        return next;
      });
      await load();
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setBusy(false);
    }
  };

  const handleCancelVe = async () => {
    if (!cancelTarget) return;
    setBusy(true);
    try {
      await api.post(`/ve/${cancelTarget.ve.maVe}/huy`);
      showToast(`Đã hủy vé ghế ${cancelTarget.ve.choNgoi}.`);
      setDetails((prev) => {
        const next = { ...prev };
        delete next[cancelTarget.maHd];
        return next;
      });
      setCancelTarget(null);
      await load();
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setBusy(false);
    }
  };

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>Lịch sử đặt vé</h1>
          <p className="page-subtitle">Theo dõi các hóa đơn của bạn, thanh toán hoặc hủy vé chưa thanh toán.</p>
        </div>
        <div className="page-actions">
          <Link to="/app/dat-ve" className="btn btn-primary">
            + Đặt vé mới
          </Link>
        </div>
      </div>

      {loading ? (
        <div className="card">
          <div className="empty-state">
            <span className="spinner" /> Đang tải lịch sử…
          </div>
        </div>
      ) : rows.length === 0 ? (
        <div className="card">
          <div className="empty-state">
            Bạn chưa có hóa đơn nào. <Link to="/app/dat-ve">Đặt vé ngay</Link>.
          </div>
        </div>
      ) : (
        <div className="history-list">
          {rows.map((item) => {
            const isOpen = openId === item.maHd;
            const ves = veCuaHoaDon(item);
            return (
              <section className={`card history-card ${isOpen ? 'history-card-open' : ''}`} key={item.maHd}>
                <div className="history-head">
                  <div className="history-code">
                    <strong>Hóa đơn #{item.maHd}</strong>
                    <span className="text-muted">{formatDateTime(item.ngayLap)}</span>
                  </div>
                  <div className="history-meta">
                    <span className="text-muted">{item.soVe ?? 0} vé</span>
                    <strong>{formatVnd(item.tongTien)}</strong>
                    <PaidBadge paid={item.trangThai} />
                  </div>
                  <div className="row-actions">
                    <button type="button" className="btn btn-ghost btn-sm" onClick={() => void toggleDetail(item)}>
                      {isOpen ? 'Thu gọn' : 'Xem vé'}
                    </button>
                    {!item.trangThai ? (
                      <button
                        type="button"
                        className="btn btn-secondary btn-sm"
                        onClick={() => {
                          setPayMethod('Tiền mặt');
                          setPayTarget(item);
                        }}
                      >
                        Thanh toán
                      </button>
                    ) : null}
                  </div>
                </div>

                {isOpen ? (
                  <div className="history-body">
                    {detailLoading && !details[item.maHd] ? (
                      <div className="empty-state">
                        <span className="spinner" /> Đang tải chi tiết…
                      </div>
                    ) : ves.length === 0 ? (
                      <div className="empty-state">Không có thông tin vé chi tiết.</div>
                    ) : (
                      <div className="ticket-list">
                        {ves.map((ve) => (
                          <div className="ticket-row" key={ve.maVe}>
                            <div className="ticket-code">
                              <span className="seat-chip">Ghế {ve.choNgoi}</span>
                              <span className="text-muted">Mã vé #{ve.maVe}</span>
                            </div>
                            <div className="ticket-route">
                              <strong>
                                {ve.diemDi} → {ve.diemDen}
                              </strong>
                              <span className="text-muted">Khởi hành {formatDateTime(ve.ngayGioKhoiHanh)}</span>
                            </div>
                            <strong className="text-primary">{formatVnd(ve.giaVe)}</strong>
                            {!item.trangThai ? (
                              <button
                                type="button"
                                className="btn btn-danger-ghost btn-sm"
                                onClick={() => setCancelTarget({ maHd: item.maHd, ve })}
                              >
                                Hủy vé
                              </button>
                            ) : null}
                          </div>
                        ))}
                      </div>
                    )}
                    <div className="history-foot">
                      <span className="text-muted">
                        Phương thức: {details[item.maHd]?.thanhToan?.phuongThuc ?? item.phuongThuc ?? '—'}
                      </span>
                    </div>
                  </div>
                ) : null}
              </section>
            );
          })}
        </div>
      )}

      <Modal
        open={Boolean(payTarget)}
        onClose={() => setPayTarget(null)}
        title="Thanh toán hóa đơn"
        subtitle={payTarget ? `Hóa đơn #${payTarget.maHd} · ${formatVnd(payTarget.tongTien)}` : undefined}
        footer={
          <>
            <button type="button" className="btn btn-secondary" onClick={() => setPayTarget(null)}>
              Hủy
            </button>
            <button type="button" className="btn btn-primary" onClick={() => void handlePay()} disabled={busy}>
              {busy ? 'Đang xử lý…' : 'Xác nhận thanh toán'}
            </button>
          </>
        }
      >
        <FormField
          label="Phương thức thanh toán"
          type="select"
          value={payMethod}
          onChange={setPayMethod}
          options={PAYMENT_METHODS.map((value) => ({ value, label: value }))}
        />
      </Modal>

      <Modal
        open={Boolean(cancelTarget)}
        onClose={() => setCancelTarget(null)}
        title="Hủy vé"
        footer={
          <>
            <button type="button" className="btn btn-secondary" onClick={() => setCancelTarget(null)}>
              Giữ vé
            </button>
            <button type="button" className="btn btn-danger" onClick={() => void handleCancelVe()} disabled={busy}>
              {busy ? 'Đang hủy…' : 'Hủy vé'}
            </button>
          </>
        }
      >
        <p>
          Bạn có chắc muốn hủy vé ghế <strong>{cancelTarget?.ve.choNgoi}</strong> của hóa đơn{' '}
          <strong>#{cancelTarget?.maHd}</strong>? Chỉ hủy được khi vé chưa thanh toán và chuyến chưa khởi hành.
        </p>
      </Modal>
    </div>
  );
}
