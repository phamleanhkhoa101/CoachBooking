import { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { api } from '../../api/client';
import type { BookingRequest, BookingResponse, Profile, TripSearchResponse } from '../../api/types';
import { useAuth } from '../../auth/AuthContext';
import { BookingStepper } from '../../components/customer/BookingStepper';
import { useToast } from '../../components/common/Toast';
import { formatCurrency, formatTime } from '../../utils/format';
import './CheckoutPage.css';

interface CheckoutLocationState {
  tripId: number;
  trip?: TripSearchResponse;
  selectedSeats: number[];
  boardingPoint: string;
  dropoffPoint: string;
  totalPrice: number;
  price: number;
  dep: string;
  dest: string;
  depTime?: string;
  typeName: string;
}

export function CheckoutPage() {
  const location = useLocation();
  const navigate = useNavigate();
  const { nguoiDung } = useAuth();
  const { showToast } = useToast();

  const state = location.state as CheckoutLocationState | undefined;

  // Form states
  const [fullName, setFullName] = useState('');
  const [phone, setPhone] = useState('');
  const [email, setEmail] = useState('');
  const [dateOfBirth, setDateOfBirth] = useState('1995-01-01');
  const [isMale, setIsMale] = useState(true);
  const [notes, setNotes] = useState('');
  const [paymentMethod, setPaymentMethod] = useState<'Chuyển khoản' | 'Tiền mặt' | 'Thanh toán sau'>('Chuyển khoản');
  const [agreeTerms, setAgreeTerms] = useState(true);
  const [transferConfirmed, setTransferConfirmed] = useState(false);

  const [submitting, setSubmitting] = useState(false);
  const [errorMsg, setErrorMsg] = useState('');

  // 15-Minute Countdown Timer (900 seconds)
  const [secondsLeft, setSecondsLeft] = useState(15 * 60);

  useEffect(() => {
    if (!state?.tripId || !state?.selectedSeats?.length) {
      navigate('/tim-chuyen', { replace: true });
      return;
    }

    const timer = setInterval(() => {
      setSecondsLeft((prev) => {
        if (prev <= 1) {
          clearInterval(timer);
          alert('Hết thời gian giữ chỗ (15 phút)! Vui lòng chọn lại chuyến xe.');
          navigate('/tim-chuyen');
          return 0;
        }
        return prev - 1;
      });
    }, 1000);

    return () => clearInterval(timer);
  }, [state, navigate]);

  // Autofill if logged in
  useEffect(() => {
    if (nguoiDung) {
      if (nguoiDung.hoTen) setFullName(nguoiDung.hoTen);
      void (async () => {
        try {
          const profile = await api.get<Profile>('/profile');
          if (profile) {
            if (profile.fullName || profile.hoTen) {
              setFullName((profile.fullName || profile.hoTen)!);
            }
            if (profile.phone || profile.sdt) {
              setPhone((profile.phone || profile.sdt)!);
            }
            if (profile.dateOfBirth || profile.ngaySinh) {
              setDateOfBirth((profile.dateOfBirth || profile.ngaySinh)!);
            }
          }
        } catch {
          // ignore
        }
      })();
    }
  }, [nguoiDung]);

  if (!state?.tripId) return null;

  const minutes = Math.floor(secondsLeft / 60);
  const seconds = secondsLeft % 60;
  const timerDisplay = `${minutes < 10 ? '0' + minutes : minutes}:${
    seconds < 10 ? '0' + seconds : seconds
  }`;

  const transferMemo = `CBK${state.tripId} ${phone ? phone.slice(-4) : 'VE'}`;

  const handleCopy = (text: string, label: string) => {
    void navigator.clipboard.writeText(text);
    showToast(`Đã sao chép ${label}: ${text}`);
  };

  const handleSubmitBooking = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrorMsg('');

    if (!fullName.trim()) {
      setErrorMsg('Vui lòng nhập họ và tên hành khách.');
      return;
    }
    if (!phone.trim() || !/^\d{9,11}$/.test(phone.trim())) {
      setErrorMsg('Số điện thoại không hợp lệ (cần từ 9 đến 11 chữ số).');
      return;
    }
    if (!dateOfBirth) {
      setErrorMsg('Vui lòng chọn ngày sinh của hành khách.');
      return;
    }
    if (!agreeTerms) {
      setErrorMsg('Vui lòng đồng ý với điều khoản đặt vé để tiếp tục.');
      return;
    }

    setSubmitting(true);
    try {
      const payload: BookingRequest = {
        tripId: state.tripId,
        seatNumbers: state.selectedSeats,
        paymentMethod: paymentMethod,
        customerName: fullName.trim(),
        phone: phone.trim(),
        dateOfBirth: dateOfBirth,
        gender: isMale,
      };

      const res = await api.post<BookingResponse>('/bookings', payload);
      const invoiceId = res.invoiceId ?? res.maHd;

      showToast('Đặt vé thành công!');
      navigate(`/ve-dien-tu/${invoiceId}`, {
        state: {
          invoiceId,
          bookingResponse: res,
          passengerName: fullName,
          passengerPhone: phone,
          boardingPoint: state.boardingPoint,
          dropoffPoint: state.dropoffPoint,
          selectedSeats: state.selectedSeats,
          trip: state.trip,
          dep: state.dep,
          dest: state.dest,
          depTime: state.depTime,
          typeName: state.typeName,
          totalPrice: state.totalPrice,
          paymentMethod,
        },
      });
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : 'Có lỗi khi tiến hành đặt vé';
      setErrorMsg(msg);
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="checkout-page">
      {/* 1. STEPPER */}
      <BookingStepper currentStep={3} />

      {/* 2. RESERVATION COUNTDOWN ALERT BANNER */}
      <section className="reservation-alert-strip">
        <div className="alert-content-inner">
          <div className="timer-badge">
            <svg
              width="20"
              height="20"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="2.5"
            >
              <circle cx="12" cy="12" r="10" />
              <polyline points="12 6 12 12 16 14" />
            </svg>
            <span>Thời gian giữ vé còn lại: <strong>{timerDisplay}</strong></span>
          </div>
          <p className="timer-desc">
            Vui lòng hoàn tất thanh toán trước khi thời gian đếm ngược kết thúc để hệ thống không tự động hủy chỗ!
          </p>
        </div>
      </section>

      {/* 3. MAIN CHECKOUT 2-COLUMN GRID */}
      <div className="checkout-container">
        <form className="checkout-layout-grid" onSubmit={handleSubmitBooking}>
          {/* LEFT COLUMN: PASSENGER INFO & PAYMENT METHODS */}
          <div className="checkout-left-col">
            {/* PASSENGER INFO */}
            <div className="checkout-card">
              <div className="card-head">
                <div className="card-icon-bubble">1</div>
                <h3>Thông tin hành khách đi xe</h3>
              </div>

              <div className="form-grid-2">
                <div className="form-group-field full-width">
                  <label>
                    Họ và tên hành khách <span className="req">*</span>
                  </label>
                  <input
                    type="text"
                    required
                    placeholder="Ví dụ: Nguyễn Văn An"
                    value={fullName}
                    onChange={(e) => setFullName(e.target.value)}
                  />
                </div>

                <div className="form-group-field">
                  <label>
                    Số điện thoại nhận vé <span className="req">*</span>
                    <span className="info-badge">SMS / Zalo ZNS</span>
                  </label>
                  <input
                    type="tel"
                    required
                    placeholder="0912 345 678"
                    value={phone}
                    onChange={(e) => setPhone(e.target.value)}
                  />
                </div>

                <div className="form-group-field">
                  <label>Email nhận vé điện tử PDF</label>
                  <input
                    type="email"
                    placeholder="email@vidu.com"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                  />
                </div>

                <div className="form-group-field">
                  <label>
                    Ngày sinh <span className="req">*</span>
                  </label>
                  <input
                    type="date"
                    required
                    value={dateOfBirth}
                    onChange={(e) => setDateOfBirth(e.target.value)}
                  />
                </div>

                <div className="form-group-field">
                  <label>Giới tính</label>
                  <div className="radio-group-gender">
                    <label className="radio-label">
                      <input
                        type="radio"
                        name="gender"
                        checked={isMale}
                        onChange={() => setIsMale(true)}
                      />
                      <span>Nam</span>
                    </label>
                    <label className="radio-label">
                      <input
                        type="radio"
                        name="gender"
                        checked={!isMale}
                        onChange={() => setIsMale(false)}
                      />
                      <span>Nữ</span>
                    </label>
                  </div>
                </div>

                <div className="form-group-field full-width">
                  <label>Ghi chú đón / trả hoặc hành lý kèm theo (nếu có)</label>
                  <textarea
                    rows={2}
                    placeholder="Ví dụ: Có 1 thùng trái cây 15kg, đón gần ngã tư Hàng Xanh..."
                    value={notes}
                    onChange={(e) => setNotes(e.target.value)}
                  />
                </div>
              </div>
            </div>

            {/* PAYMENT METHODS */}
            <div className="checkout-card">
              <div className="card-head">
                <div className="card-icon-bubble">2</div>
                <h3>Phương thức thanh toán</h3>
              </div>

              <div className="payment-options-list">
                {/* OPTION 1: VIETQR */}
                <div
                  className={`payment-option-box ${
                    paymentMethod === 'Chuyển khoản' ? 'selected' : ''
                  }`}
                  onClick={() => setPaymentMethod('Chuyển khoản')}
                >
                  <div className="option-radio-row">
                    <input
                      type="radio"
                      name="paymentMethod"
                      value="Chuyển khoản"
                      checked={paymentMethod === 'Chuyển khoản'}
                      onChange={() => setPaymentMethod('Chuyển khoản')}
                    />
                    <div className="option-info">
                      <div className="option-title-row">
                        <strong>Quét mã VietQR 24/7 (Khuyên dùng)</strong>
                        <span className="recom-badge">Miễn phí giao dịch · Nhanh nhất</span>
                      </div>
                      <p className="option-desc">
                        Chuyển khoản ngân hàng tức thì qua mã QR tự động Napas247, hệ thống xác nhận thanh toán vé trong 30 giây.
                      </p>
                    </div>
                  </div>

                  {/* VIETQR EMBEDDED BOX */}
                  {paymentMethod === 'Chuyển khoản' && (
                    <div className="vietqr-details-frame" onClick={(e) => e.stopPropagation()}>
                      <div className="vietqr-layout">
                        <div className="vietqr-image-card">
                          <img
                            src={`https://api.vietqr.io/image/970436-1028889977-compact2.jpg?amount=${state.totalPrice}&addInfo=${encodeURIComponent(
                              transferMemo,
                            )}&accountName=BEN%20XE%20MIEN%20DONG`}
                            alt="Mã VietQR thanh toán vé"
                            className="vietqr-code-img"
                          />
                          <span className="vietqr-hint">Mở app Ngân hàng quét QR</span>
                        </div>

                        <div className="vietqr-meta-col">
                          <div className="meta-row">
                            <span className="m-label">Ngân hàng thụ hưởng:</span>
                            <strong className="m-val">Vietcombank (VCB)</strong>
                          </div>
                          <div className="meta-row">
                            <span className="m-label">Tên tài khoản:</span>
                            <strong className="m-val">CONG TY CP BEN XE MIEN DONG</strong>
                          </div>
                          <div className="meta-row highlight">
                            <span className="m-label">Số tài khoản:</span>
                            <div className="val-copy-group">
                              <strong className="m-val copyable">1028 8899 77</strong>
                              <button
                                type="button"
                                className="btn-copy"
                                onClick={() => handleCopy('1028889977', 'Số tài khoản')}
                              >
                                Sao chép
                              </button>
                            </div>
                          </div>
                          <div className="meta-row highlight">
                            <span className="m-label">Số tiền:</span>
                            <strong className="m-val text-orange">
                              {formatCurrency(state.totalPrice)}
                            </strong>
                          </div>
                          <div className="meta-row highlight">
                            <span className="m-label">Nội dung chuyển khoản:</span>
                            <div className="val-copy-group">
                              <strong className="m-val text-blue copyable">
                                {transferMemo}
                              </strong>
                              <button
                                type="button"
                                className="btn-copy"
                                onClick={() => handleCopy(transferMemo, 'Nội dung CK')}
                              >
                                Sao chép
                              </button>
                            </div>
                          </div>

                          <div className="transfer-confirm-check">
                            <label className="checkbox-row">
                              <input
                                type="checkbox"
                                checked={transferConfirmed}
                                onChange={(e) => setTransferConfirmed(e.target.checked)}
                              />
                              <span>Tôi đã mở ứng dụng ngân hàng và chuyển khoản thành công</span>
                            </label>
                          </div>
                        </div>
                      </div>
                    </div>
                  )}
                </div>

                {/* OPTION 2: CASH AT COUNTER */}
                <div
                  className={`payment-option-box ${
                    paymentMethod === 'Tiền mặt' ? 'selected' : ''
                  }`}
                  onClick={() => setPaymentMethod('Tiền mặt')}
                >
                  <div className="option-radio-row">
                    <input
                      type="radio"
                      name="paymentMethod"
                      value="Tiền mặt"
                      checked={paymentMethod === 'Tiền mặt'}
                      onChange={() => setPaymentMethod('Tiền mặt')}
                    />
                    <div className="option-info">
                      <strong>Thanh toán tiền mặt tại quầy vé Bến xe Miền Đông</strong>
                      <p className="option-desc">
                        Nhận mã đặt vé qua SMS. Quý khách vui lòng đến Quầy vé số 04 - Sảnh trung tâm Bến xe Miền Đông Mới trước giờ xuất bến 60 phút để nộp tiền và nhận vé.
                      </p>
                    </div>
                  </div>
                </div>

                {/* OPTION 3: PAY LATER */}
                <div
                  className={`payment-option-box ${
                    paymentMethod === 'Thanh toán sau' ? 'selected' : ''
                  }`}
                  onClick={() => setPaymentMethod('Thanh toán sau')}
                >
                  <div className="option-radio-row">
                    <input
                      type="radio"
                      name="paymentMethod"
                      value="Thanh toán sau"
                      checked={paymentMethod === 'Thanh toán sau'}
                      onChange={() => setPaymentMethod('Thanh toán sau')}
                    />
                    <div className="option-info">
                      <div className="option-title-row">
                        <strong>Thanh toán sau (Giữ vé 24 giờ)</strong>
                        <span className="member-badge">Thành viên SAMCO</span>
                      </div>
                      <p className="option-desc">
                        Giữ chỗ trên chuyến xe và thanh toán trực tuyến qua tài khoản hoặc tại quầy trước giờ khởi hành tối thiểu 2 giờ.
                      </p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          {/* RIGHT COLUMN: STICKY ORDER SUMMARY */}
          <aside className="checkout-right-col">
            <div className="order-summary-card">
              <h3>Tóm tắt đơn đặt vé</h3>

              <div className="trip-summary-box">
                <span className="summary-route">
                  {state.dep} ➔ {state.dest}
                </span>
                <strong className="summary-bus-title">
                  {state.trip?.companyName ?? 'Nhà xe Phương Trang'} · {state.typeName}
                </strong>
                <span className="summary-time">
                  Giờ xuất bến: <strong>{state.depTime ? formatTime(state.depTime) : '--:--'}</strong>
                </span>
              </div>

              <div className="summary-points">
                <div className="point-item">
                  <span className="dot blue" />
                  <div>
                    <span className="p-label">Điểm đón:</span>
                    <span className="p-name">{state.boardingPoint}</span>
                  </div>
                </div>
                <div className="point-item">
                  <span className="dot orange" />
                  <div>
                    <span className="p-label">Điểm trả:</span>
                    <span className="p-name">{state.dropoffPoint}</span>
                  </div>
                </div>
              </div>

              <div className="order-divider" />

              <div className="order-seats-row">
                <span>Ghế đã chọn ({state.selectedSeats.length} vé):</span>
                <div className="order-seat-pills">
                  {state.selectedSeats.map((s) => (
                    <span key={s} className="pill">
                      Ghế {s}
                    </span>
                  ))}
                </div>
              </div>

              <div className="order-divider" />

              <div className="order-price-breakdown">
                <div className="price-row">
                  <span>Giá vé ({state.selectedSeats.length} x {formatCurrency(state.price)}):</span>
                  <strong>{formatCurrency(state.totalPrice)}</strong>
                </div>
                <div className="price-row">
                  <span>Bảo hiểm hành khách:</span>
                  <span className="text-teal">Bao gồm (0đ)</span>
                </div>
                <div className="price-row">
                  <span>Phí dịch vụ bến bãi:</span>
                  <span className="text-teal">Miễn phí</span>
                </div>
                <div className="order-total-highlight">
                  <span>TỔNG TIỀN THANH TOÁN:</span>
                  <span className="total-highlight-val">
                    {formatCurrency(state.totalPrice)}
                  </span>
                </div>
              </div>

              {errorMsg && <div className="checkout-error-alert">{errorMsg}</div>}

              <div className="terms-checkbox-wrap">
                <label className="checkbox-row">
                  <input
                    type="checkbox"
                    checked={agreeTerms}
                    onChange={(e) => setAgreeTerms(e.target.checked)}
                  />
                  <span>
                    Tôi đồng ý với <a href="#terms">Điều khoản sử dụng</a> và{' '}
                    <a href="#cancel-policy">Chính sách hoàn hủy vé</a> của Bến xe Miền Đông.
                  </span>
                </label>
              </div>

              <button
                type="submit"
                className="btn-submit-order"
                disabled={submitting}
              >
                {submitting ? (
                  <>
                    <span className="spinner-small" /> Đang xử lý đặt vé...
                  </>
                ) : (
                  `Xác nhận đặt vé (${formatCurrency(state.totalPrice)})`
                )}
              </button>

              <div className="security-commitments">
                <div className="commit-item">
                  <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#059669" strokeWidth="2">
                    <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z" />
                  </svg>
                  <span>100% Giữ đúng chỗ đã chọn</span>
                </div>
                <div className="commit-item">
                  <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#059669" strokeWidth="2">
                    <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                    <path d="M7 11V7a5 5 0 0 1 10 0v4" />
                  </svg>
                  <span>Bảo mật chuẩn giao dịch ngân hàng</span>
                </div>
              </div>
            </div>
          </aside>
        </form>
      </div>
    </div>
  );
}
