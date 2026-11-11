import { useState } from 'react';
import { Link } from 'react-router-dom';
import { api } from '../../api/client';
import type { InvoiceDetail } from '../../api/types';
import { formatCurrency, formatDateTime } from '../../utils/format';
import './TicketLookupPage.css';

export function TicketLookupPage() {
  const [invoiceId, setInvoiceId] = useState('');
  const [phone, setPhone] = useState('');
  const [loading, setLoading] = useState(false);
  const [errorMsg, setErrorMsg] = useState('');
  const [result, setResult] = useState<InvoiceDetail | null>(null);

  const handleLookup = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrorMsg('');
    setResult(null);

    const idNum = parseInt(invoiceId.replace(/\D/g, ''), 10);
    if (!idNum) {
      setErrorMsg('Vui lòng nhập mã hóa đơn hợp lệ (chỉ gồm số).');
      return;
    }
    if (!phone.trim()) {
      setErrorMsg('Vui lòng nhập số điện thoại đã dùng khi đặt vé.');
      return;
    }

    setLoading(true);
    try {
      const data = await api.post<InvoiceDetail>('/invoices/lookup', {
        invoiceId: idNum,
        phone: phone.trim(),
      });
      setResult(data);
    } catch (err: unknown) {
      const msg =
        err instanceof Error
          ? err.message
          : 'Không tìm thấy thông tin vé phù hợp với mã hóa đơn và số điện thoại đã cung cấp.';
      setErrorMsg(msg);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="ticket-lookup-page">
      <div className="lookup-hero">
        <h1>Tra cứu vé xe & Hóa đơn điện tử</h1>
        <p>
          Quý khách không cần đăng nhập. Chỉ cần nhập mã đơn hàng / hóa đơn và số điện thoại đặt vé để kiểm tra thông tin vé, in vé hoặc lấy mã QR lên xe.
        </p>
      </div>

      <div className="lookup-container">
        {/* LOOKUP FORM CARD */}
        <div className="lookup-form-card">
          <form onSubmit={handleLookup}>
            <div className="lookup-inputs-grid">
              <div className="lookup-field">
                <label>Mã hóa đơn / Mã đơn vé</label>
                <div className="input-with-icon">
                  <span className="input-prefix">#CBK</span>
                  <input
                    type="text"
                    required
                    placeholder="Ví dụ: 89421 hoặc 15"
                    value={invoiceId}
                    onChange={(e) => setInvoiceId(e.target.value)}
                  />
                </div>
              </div>

              <div className="lookup-field">
                <label>Số điện thoại đặt vé</label>
                <input
                  type="tel"
                  required
                  placeholder="Ví dụ: 0912 345 678"
                  value={phone}
                  onChange={(e) => setPhone(e.target.value)}
                />
              </div>

              <button
                type="submit"
                className="btn-lookup-submit"
                disabled={loading}
              >
                {loading ? 'Đang tra cứu...' : 'Tra cứu vé'}
              </button>
            </div>
          </form>

          {errorMsg && <div className="lookup-error-box">{errorMsg}</div>}
        </div>

        {/* LOOKUP RESULT CARD */}
        {result && (
          <div className="lookup-result-card">
            <div className="result-card-header">
              <div className="result-title-col">
                <span className="invoice-tag">Hóa đơn #{result.invoiceId}</span>
                <h2>Thông tin vé xe của quý khách</h2>
                <span className="created-date">
                  Ngày đặt:{' '}
                  {result.createdAt
                    ? formatDateTime(result.createdAt)
                    : 'Hôm nay'}
                </span>
              </div>

              <div className="result-status-col">
                {result.isPaid ? (
                  <span className="badge-paid">ĐÃ THANH TOÁN</span>
                ) : (
                  <span className="badge-unpaid">CHỜ THANH TOÁN</span>
                )}
                <span className="total-val">
                  {formatCurrency(result.totalAmount)}
                </span>
              </div>
            </div>

            <div className="result-passenger-info">
              <div className="info-cell">
                <span className="label">Hành khách:</span>
                <strong>{result.customerName || 'Quý khách'}</strong>
              </div>
              <div className="info-cell">
                <span className="label">Số điện thoại:</span>
                <strong>{result.phone}</strong>
              </div>
              <div className="info-cell">
                <span className="label">Phương thức:</span>
                <strong>
                  {result.payment?.paymentMethod || 'Tại quầy / Trả sau'}
                </strong>
              </div>
            </div>

            <div className="result-tickets-table">
              <h4>Danh sách vé trên hóa đơn:</h4>
              <div className="tickets-grid">
                {(result.tickets || []).map((t, idx) => (
                  <div key={idx} className="ticket-item-row">
                    <div className="ticket-route-info">
                      <strong>
                        {t.departure ?? t.diemDi} ➔ {t.destination ?? t.diemDen}
                      </strong>
                      <span>
                        Giờ chạy:{' '}
                        {t.departureTime
                          ? formatDateTime(t.departureTime)
                          : 'Theo lịch'}
                      </span>
                    </div>

                    <div className="ticket-meta-info">
                      <span className="seat-badge">
                        Ghế: {t.seatNumber}
                      </span>
                      <span className="type-badge">
                        {t.typeName ?? t.vehicleTypeName ?? 'Xe khách'}
                      </span>
                      <strong className="ticket-price">
                        {formatCurrency(t.price ?? 0)}
                      </strong>
                    </div>
                  </div>
                ))}
              </div>
            </div>

            <div className="result-actions-footer">
              <Link
                to={`/ve-dien-tu/${result.invoiceId}`}
                state={{
                  passengerName: result.customerName,
                  passengerPhone: result.phone,
                  totalPrice: result.totalAmount,
                }}
                className="btn-open-eticket"
              >
                <svg
                  width="18"
                  height="18"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  strokeWidth="2"
                >
                  <rect x="2" y="3" width="20" height="14" rx="2" />
                  <line x1="8" y1="21" x2="16" y2="21" />
                  <line x1="12" y1="17" x2="12" y2="21" />
                </svg>
                <span>Xem & In vé điện tử (Mã QR Check-in)</span>
              </Link>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
