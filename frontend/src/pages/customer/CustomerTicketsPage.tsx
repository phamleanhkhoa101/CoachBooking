import { useEffect, useMemo, useState } from 'react';
import { Link } from 'react-router-dom';
import { api, getErrorMessage } from '../../api/client';
import type { HoaDonKhachHang } from '../../api/types';
import { useAuth } from '../../auth/AuthContext';
import { useToast } from '../../components/common/Toast';
import { formatCurrency, formatDateTime } from '../../utils/format';
import './CustomerTicketsPage.css';

export function CustomerTicketsPage() {
  const { nguoiDung } = useAuth();
  const { showToast } = useToast();

  const [invoices, setInvoices] = useState<HoaDonKhachHang[]>([]);
  const [loading, setLoading] = useState(true);
  const [activeTab, setActiveTab] = useState<'all' | 'paid' | 'unpaid'>('all');

  // Cancel invoice modal state
  const [cancelModalInvoice, setCancelModalInvoice] = useState<HoaDonKhachHang | null>(null);
  const [cancelBusy, setCancelBusy] = useState(false);

  // Pay invoice modal state
  const [payModalInvoice, setPayModalInvoice] = useState<HoaDonKhachHang | null>(null);
  const [payMethod, setPayMethod] = useState<'Chuyển khoản' | 'Tiền mặt'>('Chuyển khoản');
  const [payBusy, setPayBusy] = useState(false);

  const fetchInvoices = async () => {
    setLoading(true);
    try {
      const data = await api.get<HoaDonKhachHang[]>('/invoices/my');
      setInvoices(Array.isArray(data) ? data : []);
    } catch (err) {
      showToast(getErrorMessage(err), 'error');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (nguoiDung) {
      void fetchInvoices();
    } else {
      setLoading(false);
    }
  }, [nguoiDung]);

  const filteredInvoices = useMemo(() => {
    return invoices.filter((item) => {
      const isPaid = item.isPaid ?? item.trangThai;
      if (activeTab === 'paid') return Boolean(isPaid);
      if (activeTab === 'unpaid') return !isPaid;
      return true;
    });
  }, [invoices, activeTab]);

  const handleCancelInvoice = async () => {
    if (!cancelModalInvoice) return;
    const id = cancelModalInvoice.invoiceId ?? cancelModalInvoice.maHd;
    setCancelBusy(true);
    try {
      await api.post(`/invoices/${id}/cancel`);
      showToast(`Đã hủy thành công đơn vé #${id}`);
      setCancelModalInvoice(null);
      await fetchInvoices();
    } catch (err) {
      showToast(getErrorMessage(err), 'error');
    } finally {
      setCancelBusy(false);
    }
  };

  const handlePayInvoice = async () => {
    if (!payModalInvoice) return;
    const id = payModalInvoice.invoiceId ?? payModalInvoice.maHd;
    setPayBusy(true);
    try {
      await api.post(`/invoices/${id}/pay`, { paymentMethod: payMethod });
      showToast(`Thanh toán thành công đơn vé #${id}`);
      setPayModalInvoice(null);
      await fetchInvoices();
    } catch (err) {
      showToast(getErrorMessage(err), 'error');
    } finally {
      setPayBusy(false);
    }
  };

  if (!nguoiDung) {
    return (
      <div className="customer-tickets-page guest-gate">
        <div className="gate-card">
          <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="#1e40af" strokeWidth="1.5">
            <rect x="2" y="3" width="20" height="14" rx="2" />
            <line x1="8" y1="21" x2="16" y2="21" />
            <line x1="12" y1="17" x2="12" y2="21" />
          </svg>
          <h2>Quý khách chưa đăng nhập tài khoản</h2>
          <p>
            Vui lòng đăng nhập để xem danh sách vé đã đặt và lịch sử chuyến đi của bạn.
          </p>
          <div className="gate-actions">
            <Link to="/login" className="btn-gate-login">
              Đăng nhập ngay
            </Link>
            <Link to="/tra-cuu-ve" className="btn-gate-lookup">
              Tra cứu vé bằng mã đơn & SĐT
            </Link>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="customer-tickets-page">
      <div className="tickets-page-header">
        <div className="header-container">
          <h1>Vé của tôi & Lịch sử đặt chỗ</h1>
          <p>Quản lý toàn bộ vé xe khách, xem vé điện tử mã QR hoặc hủy vé trước giờ khởi hành.</p>
        </div>
      </div>

      <div className="tickets-main-container">
        {/* TABS */}
        <div className="tickets-tabs-bar">
          <button
            type="button"
            className={`tab-item ${activeTab === 'all' ? 'active' : ''}`}
            onClick={() => setActiveTab('all')}
          >
            Tất cả ({invoices.length})
          </button>
          <button
            type="button"
            className={`tab-item ${activeTab === 'paid' ? 'active' : ''}`}
            onClick={() => setActiveTab('paid')}
          >
            Đã thanh toán (
            {invoices.filter((i) => i.isPaid ?? i.trangThai).length}
            )
          </button>
          <button
            type="button"
            className={`tab-item ${activeTab === 'unpaid' ? 'active' : ''}`}
            onClick={() => setActiveTab('unpaid')}
          >
            Chờ thanh toán (
            {invoices.filter((i) => !(i.isPaid ?? i.trangThai)).length}
            )
          </button>
        </div>

        {/* LIST */}
        {loading ? (
          <div className="tickets-loading-card">
            <span className="spinner-large" />
            <p>Đang tải danh sách vé của bạn...</p>
          </div>
        ) : filteredInvoices.length === 0 ? (
          <div className="tickets-empty-card">
            <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="#94a3b8" strokeWidth="1.5">
              <rect x="2" y="3" width="20" height="14" rx="2" />
              <line x1="8" y1="21" x2="16" y2="21" />
              <line x1="12" y1="17" x2="12" y2="21" />
            </svg>
            <h3>Chưa có vé xe nào trong mục này</h3>
            <p>Quý khách chưa có đơn vé nào hoặc chưa đặt chuyến xe phù hợp với bộ lọc.</p>
            <Link to="/tim-chuyen" className="btn-book-now">
              Tìm chuyến xe ngay
            </Link>
          </div>
        ) : (
          <div className="tickets-cards-grid">
            {filteredInvoices.map((inv) => {
              const id = inv.invoiceId ?? inv.maHd;
              const isPaid = inv.isPaid ?? inv.trangThai;
              const amount = inv.totalAmount ?? inv.tongTien ?? 0;
              const tickets = inv.tickets ?? inv.veXes ?? [];
              const seatCount = inv.ticketCount ?? inv.soVe ?? tickets.length;
              const createdAt = inv.createdAt ?? inv.ngayTao;

              return (
                <div key={id} className="ticket-record-card">
                  <div className="card-top-row">
                    <div className="code-col">
                      <span className="ticket-code-badge">#CBK{id}</span>
                      <span className="ticket-time-label">
                        Ngày đặt: {createdAt ? formatDateTime(createdAt) : '—'}
                      </span>
                    </div>

                    <div className="status-col">
                      {isPaid ? (
                        <span className="tag-status paid">ĐÃ THANH TOÁN</span>
                      ) : (
                        <span className="tag-status unpaid">CHỜ THANH TOÁN</span>
                      )}
                    </div>
                  </div>

                  <div className="card-mid-body">
                    <div className="card-seats-info">
                      <span className="info-label">Số lượng vé:</span>
                      <strong>{seatCount} vé</strong>
                      {tickets.length > 0 && (
                        <span className="seat-list-txt">
                          (Ghế: {tickets.map((t) => t.seatNumber ?? t.choNgoi).join(', ')})
                        </span>
                      )}
                    </div>

                    <div className="card-price-info">
                      <span className="info-label">Tổng thanh toán:</span>
                      <strong className="total-amount">{formatCurrency(amount)}</strong>
                    </div>
                  </div>

                  <div className="card-bottom-actions">
                    <Link
                      to={`/ve-dien-tu/${id}`}
                      className="btn-action-eticket"
                    >
                      <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                        <rect x="2" y="3" width="20" height="14" rx="2" />
                        <line x1="8" y1="21" x2="16" y2="21" />
                        <line x1="12" y1="17" x2="12" y2="21" />
                      </svg>
                      Xem vé & QR
                    </Link>

                    {!isPaid && (
                      <button
                        type="button"
                        className="btn-action-pay"
                        onClick={() => setPayModalInvoice(inv)}
                      >
                        Thanh toán ngay
                      </button>
                    )}

                    {!isPaid && (
                      <button
                        type="button"
                        className="btn-action-cancel"
                        onClick={() => setCancelModalInvoice(inv)}
                      >
                        Hủy đơn vé
                      </button>
                    )}
                  </div>
                </div>
              );
            })}
          </div>
        )}
      </div>

      {/* CANCEL MODAL */}
      {cancelModalInvoice && (
        <div className="custom-modal-backdrop" onClick={() => setCancelModalInvoice(null)}>
          <div className="custom-modal-dialog" onClick={(e) => e.stopPropagation()}>
            <h3>Xác nhận hủy đơn vé #{cancelModalInvoice.invoiceId ?? cancelModalInvoice.maHd}</h3>
            <p>
              Quý khách có chắc chắn muốn hủy đơn vé này không? Chỗ ngồi đã giữ sẽ được giải phóng lại cho hành khách khác trên hệ thống.
            </p>
            <div className="modal-actions-bar">
              <button
                type="button"
                className="btn-modal-back"
                onClick={() => setCancelModalInvoice(null)}
                disabled={cancelBusy}
              >
                Giữ lại vé
              </button>
              <button
                type="button"
                className="btn-modal-confirm-cancel"
                onClick={handleCancelInvoice}
                disabled={cancelBusy}
              >
                {cancelBusy ? 'Đang hủy...' : 'Đồng ý hủy vé'}
              </button>
            </div>
          </div>
        </div>
      )}

      {/* PAY MODAL */}
      {payModalInvoice && (
        <div className="custom-modal-backdrop" onClick={() => setPayModalInvoice(null)}>
          <div className="custom-modal-dialog" onClick={(e) => e.stopPropagation()}>
            <h3>Thanh toán đơn vé #{payModalInvoice.invoiceId ?? payModalInvoice.maHd}</h3>
            <p>
              Số tiền thanh toán:{' '}
              <strong className="text-orange">
                {formatCurrency(payModalInvoice.totalAmount ?? payModalInvoice.tongTien ?? 0)}
              </strong>
            </p>

            <div className="pay-method-choice">
              <label className="radio-label">
                <input
                  type="radio"
                  name="modalPayMethod"
                  checked={payMethod === 'Chuyển khoản'}
                  onChange={() => setPayMethod('Chuyển khoản')}
                />
                <span>Chuyển khoản ngân hàng (VietQR / Internet Banking)</span>
              </label>
              <label className="radio-label">
                <input
                  type="radio"
                  name="modalPayMethod"
                  checked={payMethod === 'Tiền mặt'}
                  onChange={() => setPayMethod('Tiền mặt')}
                />
                <span>Thanh toán tiền mặt tại quầy vé bến xe</span>
              </label>
            </div>

            <div className="modal-actions-bar">
              <button
                type="button"
                className="btn-modal-back"
                onClick={() => setPayModalInvoice(null)}
                disabled={payBusy}
              >
                Đóng
              </button>
              <button
                type="button"
                className="btn-modal-confirm-pay"
                onClick={handlePayInvoice}
                disabled={payBusy}
              >
                {payBusy ? 'Đang xử lý...' : 'Xác nhận thanh toán'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
