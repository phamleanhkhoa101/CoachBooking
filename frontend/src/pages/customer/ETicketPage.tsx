import { useEffect, useState } from 'react';
import { Link, useLocation, useNavigate, useParams } from 'react-router-dom';
import { api } from '../../api/client';
import type { InvoiceDetail } from '../../api/types';
import { BookingStepper } from '../../components/customer/BookingStepper';
import { useToast } from '../../components/common/Toast';
import { formatCurrency, formatDateTime } from '../../utils/format';
import './ETicketPage.css';

export function ETicketPage() {
  const { invoiceId } = useParams<{ invoiceId: string }>();
  const navigate = useNavigate();
  const location = useLocation();
  const { showToast } = useToast();

  const [invoice, setInvoice] = useState<InvoiceDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [errorMsg, setErrorMsg] = useState('');

  // Read possible router state from CheckoutPage
  const state = location.state as Record<string, any> | undefined;

  useEffect(() => {
    if (!invoiceId) {
      navigate('/tra-cuu-ve', { replace: true });
      return;
    }

    void (async () => {
      setLoading(true);
      setErrorMsg('');
      try {
        // Try fetching through authenticated or direct endpoint
        const data = await api.get<InvoiceDetail>(`/invoices/${invoiceId}`);
        setInvoice(data);
      } catch {
        // If not authenticated or forbidden, try lookup if phone is in state
        if (state?.passengerPhone) {
          try {
            const data = await api.post<InvoiceDetail>('/invoices/lookup', {
              invoiceId: Number(invoiceId),
              phone: state.passengerPhone,
            });
            setInvoice(data);
          } catch (e: unknown) {
            setErrorMsg(
              'Không thể tải chi tiết vé điện tử. Quý khách có thể tra cứu vé bằng mã hóa đơn và số điện thoại.',
            );
          }
        } else {
          setErrorMsg(
            'Vui lòng đăng nhập hoặc sử dụng tính năng Tra cứu vé để xem chi tiết vé.',
          );
        }
      } finally {
        setLoading(false);
      }
    })();
  }, [invoiceId, state, navigate]);

  const handleCopyCode = () => {
    if (invoiceId) {
      void navigator.clipboard.writeText(`#CBK${invoiceId}`);
      showToast('Đã sao chép mã vé: #CBK' + invoiceId);
    }
  };

  const handlePrint = () => {
    window.print();
  };

  // Derive information from invoice or state
  const idNum = invoice?.invoiceId ?? Number(invoiceId);
  const customerName =
    invoice?.customerName ?? state?.passengerName ?? 'Quý khách';
  const customerPhone = invoice?.phone ?? state?.passengerPhone ?? '—';
  const totalAmount =
    invoice?.totalAmount ?? invoice?.tongTien ?? state?.totalPrice ?? 0;
  const isPaid = invoice?.isPaid ?? invoice?.trangThai ?? (state?.paymentMethod !== 'Thanh toán sau');
  const tickets = invoice?.tickets ?? invoice?.chiTietVe ?? [];

  const firstTicket = tickets.length > 0 ? tickets[0] : null;
  const departure =
    firstTicket?.departure ??
    firstTicket?.diemDi ??
    state?.dep ??
    'TP. Hồ Chí Minh';
  const destination =
    firstTicket?.destination ??
    firstTicket?.diemDen ??
    state?.dest ??
    'Điểm đến';
  const depTime =
    firstTicket?.departureTime ??
    firstTicket?.ngayGioKhoiHanh ??
    state?.depTime;
  const vehicleType =
    firstTicket?.vehicleTypeName ??
    firstTicket?.tenLoaiXe ??
    state?.typeName ??
    'Xe khách Limousine';
  const licensePlate =
    firstTicket?.licensePlate ??
    firstTicket?.bienSoXe ??
    state?.trip?.licensePlate ??
    '51B-342.89';

  const seatLabels =
    tickets.length > 0
      ? tickets.map((t: any) => t.seatNumber ?? t.soGhe).join(', ')
      : state?.selectedSeats
      ? state.selectedSeats.join(', ')
      : 'Theo sơ đồ';

  const qrValue = `COACHBOOKING|INV:${idNum}|PHONE:${customerPhone}|SEATS:${seatLabels}`;
  const qrUrl = `https://api.qrserver.com/v1/create-qr-code/?size=180x180&data=${encodeURIComponent(
    qrValue,
  )}`;

  return (
    <div className="eticket-page">
      {/* 1. STEPPER */}
      <div className="no-print">
        <BookingStepper currentStep={4} />
      </div>

      <div className="eticket-container">
        {loading && (
          <div className="search-loading-state no-print" style={{ marginBottom: '1.5rem' }}>
            <span className="spinner-large" />
            <p>Đang tải dữ liệu vé điện tử...</p>
          </div>
        )}

        {errorMsg && (
          <div className="checkout-error-alert no-print" style={{ marginBottom: '1.5rem' }}>
            <p>{errorMsg}</p>
          </div>
        )}

        {/* SUCCESS HERO ALERT */}
        <div className="success-hero-alert no-print">
          <div className="hero-alert-icon">
            <svg
              width="32"
              height="32"
              viewBox="0 0 24 24"
              fill="none"
              stroke="#ffffff"
              strokeWidth="3"
            >
              <polyline points="20 6 9 17 4 12" />
            </svg>
          </div>
          <div className="hero-alert-content">
            <h2>Đặt vé thành công! Chúc quý khách thượng lộ bình an!</h2>
            <p>
              Mã đơn vé:{' '}
              <strong className="booking-code-chip">#CBK{idNum}</strong>
              <button
                type="button"
                className="btn-copy-code"
                onClick={handleCopyCode}
              >
                Sao chép
              </button>
            </p>
            <span className="hero-alert-sub">
              Thông tin vé điện tử và mã QR đã được gửi về số điện thoại{' '}
              <strong>{customerPhone}</strong>.
            </span>
          </div>
        </div>

        {/* PRINT / ACTION TOOLBAR */}
        <div className="ticket-action-toolbar no-print">
          <button type="button" className="btn-print-ticket" onClick={handlePrint}>
            <svg
              width="18"
              height="18"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="2"
            >
              <polyline points="6 9 6 2 18 2 18 9" />
              <path d="M6 18H4a2 2 0 0 1-2-2v-5a2 2 0 0 1 2-2h16a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2h-2" />
              <rect x="6" y="14" width="12" height="8" />
            </svg>
            <span>In vé / Lưu PDF</span>
          </button>

          <Link to="/ve-cua-toi" className="btn-view-history">
            Xem danh sách vé của tôi
          </Link>
          <Link to="/" className="btn-home-back">
            Về trang chủ
          </Link>
        </div>

        {/* THE BOARDING PASS VOUCHER (PRINTABLE) */}
        <div className="boarding-pass-card">
          {/* LEFT: MAIN TICKET BODY */}
          <div className="pass-main-body">
            <div className="pass-header">
              <div className="pass-brand">
                <div className="pass-logo-badge">BX</div>
                <div>
                  <span className="pass-brand-title">BẾN XE MIỀN ĐÔNG</span>
                  <span className="pass-brand-sub">COACHBOOKING · VÉ XE ĐIỆN TỬ</span>
                </div>
              </div>

              <div className="pass-status-badge">
                {isPaid ? (
                  <span className="status-tag paid">ĐÃ THANH TOÁN</span>
                ) : (
                  <span className="status-tag unpaid">CHỜ THANH TOÁN</span>
                )}
                <span className="pass-serial">ET-CBK{idNum}-VN</span>
              </div>
            </div>

            {/* ROUTE DISPLAY */}
            <div className="pass-route-row">
              <div className="route-point">
                <span className="point-city">{departure}</span>
                <span className="point-sub">Bến xe Miền Đông Mới</span>
              </div>

              <div className="route-arrow-center">
                <span className="transit-icon">🚌</span>
                <span className="arrow-line" />
                <span className="arrow-head">➔</span>
              </div>

              <div className="route-point right">
                <span className="point-city">{destination}</span>
                <span className="point-sub">Bến xe trung tâm liên tỉnh</span>
              </div>
            </div>

            {/* DETAILS GRID */}
            <div className="pass-details-grid">
              <div className="detail-item">
                <span className="d-label">Hành khách</span>
                <strong className="d-val">{customerName}</strong>
              </div>
              <div className="detail-item">
                <span className="d-label">Số điện thoại</span>
                <strong className="d-val">{customerPhone}</strong>
              </div>
              <div className="detail-item">
                <span className="d-label">Giờ xuất bến</span>
                <strong className="d-val text-blue">
                  {depTime ? formatDateTime(depTime) : 'Theo giờ vé'}
                </strong>
              </div>
              <div className="detail-item">
                <span className="d-label">Loại xe & BKS</span>
                <strong className="d-val">
                  {vehicleType} · {licensePlate}
                </strong>
              </div>
              <div className="detail-item">
                <span className="d-label">Vị trí ghế ngồi</span>
                <strong className="d-val text-orange seat-text">
                  Ghế: {seatLabels}
                </strong>
              </div>
              <div className="detail-item">
                <span className="d-label">Cửa lên xe</span>
                <strong className="d-val">Cửa số 03 (Sảnh tầng 1)</strong>
              </div>
            </div>

            <div className="pass-footer-price">
              <span>Tổng thanh toán:</span>
              <strong className="pass-price-val">{formatCurrency(totalAmount)}</strong>
              <span className="vat-hint">(Đã bao gồm VAT & bảo hiểm hành khách)</span>
            </div>
          </div>

          {/* PERFORATED DIVIDER */}
          <div className="pass-divider">
            <div className="notch top" />
            <div className="dashed-line" />
            <div className="notch bottom" />
          </div>

          {/* RIGHT: CHECK-IN QR STUB */}
          <div className="pass-qr-stub">
            <span className="stub-title">MÃ CHECK-IN LÊN XE</span>
            <div className="stub-qr-box">
              <img src={qrUrl} alt="QR Code Check-in" className="qr-image" />
            </div>
            <div className="stub-pin">
              <span>MÃ TRA CỨU:</span>
              <strong>PASS-{idNum}</strong>
            </div>
            <p className="stub-hint">
              Quý khách xuất trình mã QR này tại cổng kiểm soát hoặc cho nhân viên soát vé lên xe.
            </p>
          </div>
        </div>

        {/* IMPORTANT PASSENGER GUIDELINES */}
        <div className="passenger-guidelines no-print">
          <h3>Lưu ý quan trọng cho hành khách</h3>
          <div className="guidelines-grid">
            <div className="guide-card">
              <div className="guide-icon">⏱️</div>
              <strong>Thời gian có mặt</strong>
              <p>Có mặt tại bến xe trước giờ xuất bến từ 30 – 45 phút để làm thủ tục nhận phòng/chỗ và gửi hành lý.</p>
            </div>
            <div className="guide-card">
              <div className="guide-icon">🧳</div>
              <strong>Quy định hành lý</strong>
              <p>Mỗi hành khách được miễn cước 20kg hành lý tiêu chuẩn. Nghiêm cấm mang theo chất cháy nổ hoặc hàng cấm.</p>
            </div>
            <div className="guide-card">
              <div className="guide-icon">🚐</div>
              <strong>Trung chuyển nội thành</strong>
              <p>Xe có dịch vụ trung chuyển miễn phí trong bán kính 5km tại điểm đến. Vui lòng liên hệ phụ xe khi xe gần tới nơi.</p>
            </div>
            <div className="guide-card">
              <div className="guide-icon">📞</div>
              <strong>Hỗ trợ 24/7</strong>
              <p>Tổng đài CSKH Bến xe Miền Đông: 1900 6067 - 028 3511 6858 hỗ trợ quý khách mọi lúc mọi nơi.</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
