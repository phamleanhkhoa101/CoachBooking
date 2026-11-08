import { useCallback, useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { api, getErrorMessage } from '../../api/client';
import type { HoaDonKhachHang, InvoiceDetail, TicketItem } from '../../api/types';
import { PaidBadge } from '../../components/common/Badge';
import { FormField } from '../../components/common/FormField';
import { Modal } from '../../components/common/Modal';
import { useToast } from '../../components/common/Toast';
import { formatDateTime, formatVnd } from '../../utils/format';

const PAYMENT_METHODS = ['Tiền mặt', 'Chuyển khoản', 'Thẻ ATM'];

export function CustomerHistory() {
  const { showToast } = useToast();
  const [rows, setRows] = useState<HoaDonKhachHang[]>([]);
  const [loading, setLoading] = useState(true);
  const [openId, setOpenId] = useState<number | null>(null);
  const [details, setDetails] = useState<Record<number, InvoiceDetail>>({});
  const [detailLoading, setDetailLoading] = useState(false);

  const [payTarget, setPayTarget] = useState<HoaDonKhachHang | null>(null);
  const [payMethod, setPayMethod] = useState('Tiền mặt');
  const [cancelTarget, setCancelTarget] = useState<{ invoiceId: number; ticket: TicketItem } | null>(
    null,
  );
  const [busy, setBusy] = useState(false);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const data = await api.get<HoaDonKhachHang[]>('/invoices/my');
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
    const id = item.invoiceId ?? item.maHd;
    if (openId === id) {
      setOpenId(null);
      return;
    }
    setOpenId(id);
    if (details[id]) return;
    setDetailLoading(true);
    try {
      const data = await api.get<InvoiceDetail>(`/invoices/${id}`);
      setDetails((prev) => ({ ...prev, [id]: data }));
    } catch {
      // Dùng dữ liệu danh sách nếu có
    } finally {
      setDetailLoading(false);
    }
  };

  const getTicketsForInvoice = (item: HoaDonKhachHang): TicketItem[] => {
    const id = item.invoiceId ?? item.maHd;
    const detail = details[id];
    if (detail?.tickets) return detail.tickets;
    if (detail?.veXes) return detail.veXes;
    return item.tickets ?? item.veXes ?? [];
  };

  const handlePay = async () => {
    if (!payTarget) return;
    const id = payTarget.invoiceId ?? payTarget.maHd;
    setBusy(true);
    try {
      await api.post(`/invoices/${id}/pay`, { paymentMethod: payMethod });
      showToast(`Đã thanh toán hóa đơn #${id}.`);
      setPayTarget(null);
      setDetails((prev) => {
        const next = { ...prev };
        delete next[id];
        return next;
      });
      await load();
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setBusy(false);
    }
  };

  const handleCancelTicket = async () => {
    if (!cancelTarget) return;
    const ticketId = cancelTarget.ticket.ticketId ?? cancelTarget.ticket.maVe;
    const seatNumber = cancelTarget.ticket.seatNumber ?? cancelTarget.ticket.choNgoi;
    setBusy(true);
    try {
      await api.post(`/tickets/${ticketId}/cancel`);
      showToast(`Đã hủy vé ghế ${seatNumber}.`);
      setDetails((prev) => {
        const next = { ...prev };
        delete next[cancelTarget.invoiceId];
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
          <p className="page-subtitle">Xem lại các hóa đơn và vé đã đặt, thanh toán hoặc hủy vé khi đủ điều kiện.</p>
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
            <p>Bạn chưa có đơn đặt vé nào.</p>
            <Link to="/app/dat-ve" className="btn btn-primary">
              Đặt vé ngay
            </Link>
          </div>
        </div>
      ) : (
        <div className="history-list">
          {rows.map((row) => {
            const invoiceId = row.invoiceId ?? row.maHd;
            const createdAt = row.createdAt ?? row.ngayLap;
            const isPaid = Boolean(row.isPaid ?? row.trangThai);
            const totalAmount = row.totalAmount ?? row.tongTien;
            const ticketCount = row.ticketCount ?? row.soVe;
            const paymentMethod = row.paymentMethod ?? row.phuongThuc;
            const isOpen = openId === invoiceId;
            const tickets = getTicketsForInvoice(row);

            return (
              <article className="card history-card" key={invoiceId}>
                <header className="history-head">
                  <div className="history-head-main">
                    <span className="history-id">Hóa đơn #{invoiceId}</span>
                    <span className="text-muted">· {formatDateTime(createdAt)}</span>
                    <PaidBadge paid={isPaid} />
                  </div>
                  <div className="history-head-meta">
                    <strong className="text-primary">{formatVnd(totalAmount)}</strong>
                    <span className="text-muted">· {ticketCount} vé</span>
                    <span className="text-muted">· {paymentMethod ?? 'Chưa thanh toán'}</span>
                  </div>
                  <div className="history-head-actions">
                    <button
                      type="button"
                      className="btn btn-secondary btn-sm"
                      onClick={() => void toggleDetail(row)}
                    >
                      {isOpen ? 'Thu gọn' : 'Chi tiết vé'}
                    </button>
                    {!isPaid ? (
                      <button
                        type="button"
                        className="btn btn-primary btn-sm"
                        onClick={() => {
                          setPayMethod('Tiền mặt');
                          setPayTarget(row);
                        }}
                      >
                        Thanh toán
                      </button>
                    ) : null}
                  </div>
                </header>

                {isOpen ? (
                  <div className="history-body">
                    {detailLoading && !details[invoiceId] ? (
                      <div className="empty-state">
                        <span className="spinner" /> Đang tải danh sách vé…
                      </div>
                    ) : tickets.length === 0 ? (
                      <div className="empty-state">Không có thông tin vé cho hóa đơn này.</div>
                    ) : (
                      <div className="ticket-list">
                        {tickets.map((ticket) => {
                          const ticketId = ticket.ticketId ?? ticket.maVe;
                          const seatNumber = ticket.seatNumber ?? ticket.choNgoi;
                          const departure = ticket.departure ?? ticket.diemDi;
                          const destination = ticket.destination ?? ticket.diemDen;
                          const departureTime = ticket.departureTime ?? ticket.ngayGioKhoiHanh;
                          const price = ticket.price ?? ticket.giaVe;

                          return (
                            <div className="ticket-card" key={ticketId}>
                              <div className="ticket-card-main">
                                <div className="ticket-code">Vé #{ticketId}</div>
                                <div className="ticket-route">
                                  <strong>
                                    {departure} → {destination}
                                  </strong>
                                  <span className="text-muted">
                                    Khởi hành {formatDateTime(departureTime)}
                                  </span>
                                </div>
                                <div className="ticket-seat">
                                  Ghế: <strong>{seatNumber}</strong>
                                </div>
                                <div className="ticket-price">
                                  <strong>{formatVnd(price)}</strong>
                                </div>
                              </div>
                              <div className="ticket-card-actions">
                                <button
                                  type="button"
                                  className="btn btn-danger-ghost btn-sm"
                                  onClick={() =>
                                    setCancelTarget({
                                      invoiceId,
                                      ticket,
                                    })
                                  }
                                >
                                  Hủy vé
                                </button>
                              </div>
                            </div>
                          );
                        })}
                      </div>
                    )}
                  </div>
                ) : null}
              </article>
            );
          })}
        </div>
      )}

      <Modal
        open={Boolean(payTarget)}
        onClose={() => setPayTarget(null)}
        title="Thanh toán hóa đơn"
        subtitle={
          payTarget
            ? `Hóa đơn #${payTarget.invoiceId ?? payTarget.maHd} · ${formatVnd(
                payTarget.totalAmount ?? payTarget.tongTien,
              )}`
            : undefined
        }
        footer={
          <>
            <button type="button" className="btn btn-secondary" onClick={() => setPayTarget(null)}>
              Hủy
            </button>
            <button
              type="button"
              className="btn btn-primary"
              onClick={() => void handlePay()}
              disabled={busy}
            >
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
            <button
              type="button"
              className="btn btn-secondary"
              onClick={() => setCancelTarget(null)}
            >
              Giữ vé
            </button>
            <button
              type="button"
              className="btn btn-danger"
              onClick={() => void handleCancelTicket()}
              disabled={busy}
            >
              {busy ? 'Đang hủy…' : 'Xác nhận hủy'}
            </button>
          </>
        }
      >
        <p>
          Bạn có chắc muốn hủy vé ghế{' '}
          <strong>
            {cancelTarget?.ticket.seatNumber ?? cancelTarget?.ticket.choNgoi}
          </strong>{' '}
          (chuyến{' '}
          {cancelTarget?.ticket.departure ?? cancelTarget?.ticket.diemDi} →{' '}
          {cancelTarget?.ticket.destination ?? cancelTarget?.ticket.diemDen} lúc{' '}
          {formatDateTime(
            cancelTarget?.ticket.departureTime ?? cancelTarget?.ticket.ngayGioKhoiHanh,
          )}
          )?
        </p>
        <p className="text-muted">Chỉ hủy được khi vé đã thanh toán và trước giờ xe khởi hành.</p>
      </Modal>
    </div>
  );
}
