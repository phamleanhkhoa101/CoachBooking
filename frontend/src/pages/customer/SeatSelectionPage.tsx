import { useEffect, useState } from 'react';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { api } from '../../api/client';
import type { SeatMapResponse, TripSearchResponse } from '../../api/types';
import { BookingStepper } from '../../components/customer/BookingStepper';
import { formatCurrency, formatTime } from '../../utils/format';
import './SeatSelectionPage.css';

const DEFAULT_BOARDING_POINTS = [
  'Bến xe Miền Đông Mới (501 Hoàng Hữu Nam, TP. Thủ Đức) - Cửa 03',
  'Cây xăng Comeco Hàng Xanh (178 Điện Biên Phủ, Bình Thạnh)',
  'Khu Công nghệ cao / Cầu vượt Trạm 2 (TP. Thủ Đức)',
  'Ngã 3 Vũng Tàu / Cổng chào Biên Hòa',
];

const DEFAULT_DROPOFF_POINTS = [
  'Bến xe trung tâm liên tỉnh tại điểm đến',
  'Văn phòng trung tâm nhà xe (Có trung chuyển nội thành 5km)',
  'Dọc Quốc lộ / Điểm trả khách thuận tiện theo lộ trình xe',
];

export function SeatSelectionPage() {
  const { tripId } = useParams<{ tripId: string }>();
  const navigate = useNavigate();
  const location = useLocation();

  // Try to read trip from router state or fetch
  const passedTrip = (location.state as { trip?: TripSearchResponse })?.trip;

  const [loading, setLoading] = useState(true);
  const [errorMsg, setErrorMsg] = useState('');
  const [seatData, setSeatData] = useState<SeatMapResponse | null>(null);

  const [selectedSeats, setSelectedSeats] = useState<number[]>([]);
  const [boardingPoint, setBoardingPoint] = useState(DEFAULT_BOARDING_POINTS[0]);
  const [dropoffPoint, setDropoffPoint] = useState(DEFAULT_DROPOFF_POINTS[0]);

  useEffect(() => {
    if (!tripId) return;
    void (async () => {
      setLoading(true);
      setErrorMsg('');
      try {
        const res = await api.get<SeatMapResponse>(`/trips/${tripId}/seats`);
        setSeatData(res);
      } catch (err: unknown) {
        const msg = err instanceof Error ? err.message : 'Không thể tải sơ đồ ghế chuyến xe';
        setErrorMsg(msg);
      } finally {
        setLoading(false);
      }
    })();
  }, [tripId]);

  const capacity = seatData?.seatCapacity ?? passedTrip?.seatCapacity ?? 40;
  const bookedSet = new Set(seatData?.bookedSeats ?? []);
  const price = seatData?.price ?? passedTrip?.actualPrice ?? 0;
  const typeName = seatData?.typeName ?? passedTrip?.typeName ?? 'Xe khách';
  const dep = seatData?.departure ?? passedTrip?.departure ?? 'TP HCM';
  const dest = seatData?.destination ?? passedTrip?.destination ?? 'Điểm đến';
  const depTime = seatData?.departureTime ?? passedTrip?.departureTime;

  // Is sleeper coach?
  const isSleeper = typeName.toLowerCase().includes('giuong') || typeName.toLowerCase().includes('limousine');

  // Build deck 1 and deck 2
  const allSeatNumbers = Array.from({ length: capacity }, (_, i) => i + 1);
  const half = Math.ceil(capacity / 2);
  const deck1 = isSleeper ? allSeatNumbers.slice(0, half) : allSeatNumbers;
  const deck2 = isSleeper ? allSeatNumbers.slice(half) : [];

  const handleSeatClick = (seatNum: number) => {
    if (bookedSet.has(seatNum)) return;
    setSelectedSeats((prev) =>
      prev.includes(seatNum)
        ? prev.filter((s) => s !== seatNum)
        : [...prev, seatNum].sort((a, b) => a - b),
    );
  };

  const totalPrice = selectedSeats.length * price;

  const handleProceedToCheckout = () => {
    if (selectedSeats.length === 0) {
      alert('Vui lòng chọn ít nhất 1 chỗ ngồi trên xe để tiếp tục.');
      return;
    }

    navigate('/dat-ve/thanh-toan', {
      state: {
        tripId: Number(tripId),
        trip: passedTrip,
        seatData,
        selectedSeats,
        boardingPoint,
        dropoffPoint,
        totalPrice,
        price,
        dep,
        dest,
        depTime,
        typeName,
      },
    });
  };

  return (
    <div className="seat-selection-page">
      {/* 1. STEPPER */}
      <BookingStepper currentStep={2} />

      {/* 2. TRIP HEADLINE STRIP */}
      <section className="trip-headline-strip">
        <div className="headline-inner">
          <div className="headline-info">
            <span className="route-tag">
              {dep} ➔ {dest}
            </span>
            <h1>
              Chuyến xe lúc {depTime ? formatTime(depTime) : '--:--'} · {typeName}
            </h1>
            <p>
              Giá niêm yết:{' '}
              <strong className="text-orange">{formatCurrency(price)}/vé</strong>{' '}
              · Bến xe Miền Đông Mới (SAMCO)
            </p>
          </div>

          <button
            type="button"
            className="btn-back-to-search"
            onClick={() => navigate(-1)}
          >
            ← Chọn chuyến khác
          </button>
        </div>
      </section>

      {/* 3. SEAT SELECTION & SUMMARY */}
      <div className="seat-layout-container">
        {loading ? (
          <div className="seat-loading-card">
            <span className="spinner-large" />
            <p>Đang tải sơ đồ ghế thời gian thực...</p>
          </div>
        ) : errorMsg ? (
          <div className="seat-error-card">
            <p className="error-text">{errorMsg}</p>
            <button
              type="button"
              className="btn-retry"
              onClick={() => window.location.reload()}
            >
              Tải lại
            </button>
          </div>
        ) : (
          <div className="seat-main-grid">
            {/* LEFT / CENTER: SEAT MAP & BOARDING POINTS */}
            <div className="seat-deck-column">
              {/* LEGEND */}
              <div className="seat-legend-bar">
                <div className="legend-item">
                  <span className="seat-box available" />
                  <span>Ghế trống</span>
                </div>
                <div className="legend-item">
                  <span className="seat-box selecting" />
                  <span>Ghế đang chọn</span>
                </div>
                <div className="legend-item">
                  <span className="seat-box booked" />
                  <span>Ghế đã bán</span>
                </div>
              </div>

              {/* COACH BUS GRAPHIC & SEATS */}
              <div className="bus-visual-deck">
                {/* Driver Row */}
                <div className="bus-steering-row">
                  <span className="steering-icon">🚗 Bác tài</span>
                  <span className="bus-door">🚪 Cửa lên</span>
                </div>

                <div className="decks-wrapper">
                  {/* Deck 1 (Tầng dưới / Sàn đơn) */}
                  <div className="deck-card">
                    <h3 className="deck-title">
                      {isSleeper ? 'Tầng 1 (Tầng dưới)' : 'Sơ đồ ghế ngồi'}
                    </h3>
                    <div className="deck-grid">
                      {deck1.map((seatNum) => {
                        const isBooked = bookedSet.has(seatNum);
                        const isSelected = selectedSeats.includes(seatNum);
                        const seatLabel = isSleeper
                          ? `A${seatNum < 10 ? '0' + seatNum : seatNum}`
                          : `Ghế ${seatNum}`;

                        return (
                          <button
                            key={seatNum}
                            type="button"
                            className={`seat-unit ${
                              isBooked
                                ? 'booked'
                                : isSelected
                                ? 'selected'
                                : 'available'
                            }`}
                            disabled={isBooked}
                            onClick={() => handleSeatClick(seatNum)}
                          >
                            <span className="seat-label">{seatLabel}</span>
                          </button>
                        );
                      })}
                    </div>
                  </div>

                  {/* Deck 2 (Tầng trên nếu là giường nằm) */}
                  {isSleeper && deck2.length > 0 && (
                    <div className="deck-card">
                      <h3 className="deck-title">Tầng 2 (Tầng trên)</h3>
                      <div className="deck-grid">
                        {deck2.map((seatNum) => {
                          const isBooked = bookedSet.has(seatNum);
                          const isSelected = selectedSeats.includes(seatNum);
                          const seatLabel = `B${
                            seatNum - half < 10
                              ? '0' + (seatNum - half)
                              : seatNum - half
                          }`;

                          return (
                            <button
                              key={seatNum}
                              type="button"
                              className={`seat-unit ${
                                isBooked
                                  ? 'booked'
                                  : isSelected
                                  ? 'selected'
                                  : 'available'
                              }`}
                              disabled={isBooked}
                              onClick={() => handleSeatClick(seatNum)}
                            >
                              <span className="seat-label">{seatLabel}</span>
                            </button>
                          );
                        })}
                      </div>
                    </div>
                  )}
                </div>
              </div>

              {/* BOARDING & DROPOFF POINTS SELECTOR */}
              <div className="pickup-dropoff-card">
                <h3>Chọn điểm đón & điểm trả khách</h3>

                <div className="form-group-pickup">
                  <label>
                    <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#1e40af" strokeWidth="2">
                      <circle cx="12" cy="12" r="10" />
                      <circle cx="12" cy="12" r="3" />
                    </svg>
                    Điểm đón tại nơi xuất phát ({dep}):
                  </label>
                  <select
                    value={boardingPoint}
                    onChange={(e) => setBoardingPoint(e.target.value)}
                  >
                    {DEFAULT_BOARDING_POINTS.map((pt) => (
                      <option key={pt} value={pt}>
                        {pt}
                      </option>
                    ))}
                  </select>
                </div>

                <div className="form-group-pickup">
                  <label>
                    <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#f97316" strokeWidth="2">
                      <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z" />
                      <circle cx="12" cy="10" r="3" />
                    </svg>
                    Điểm trả tại nơi đến ({dest}):
                  </label>
                  <select
                    value={dropoffPoint}
                    onChange={(e) => setDropoffPoint(e.target.value)}
                  >
                    {DEFAULT_DROPOFF_POINTS.map((pt) => (
                      <option key={pt} value={pt}>
                        {pt}
                      </option>
                    ))}
                  </select>
                </div>
              </div>
            </div>

            {/* RIGHT: STICKY ORDER SUMMARY */}
            <aside className="seat-summary-sidebar">
              <div className="summary-sticky-card">
                <h3>Tóm tắt đặt chỗ</h3>

                <div className="summary-section">
                  <div className="summary-row">
                    <span className="label">Tuyến:</span>
                    <strong className="val">
                      {dep} ➔ {dest}
                    </strong>
                  </div>
                  <div className="summary-row">
                    <span className="label">Giờ xuất bến:</span>
                    <strong className="val">
                      {depTime ? formatTime(depTime) : '--:--'}
                    </strong>
                  </div>
                  <div className="summary-row">
                    <span className="label">Loại xe:</span>
                    <span className="val">{typeName}</span>
                  </div>
                </div>

                <div className="summary-divider" />

                {/* SELECTED SEATS LIST */}
                <div className="summary-section">
                  <div className="summary-row">
                    <span className="label">Vị trí ghế đã chọn:</span>
                    <strong className="val text-orange">
                      {selectedSeats.length > 0
                        ? selectedSeats.join(', ')
                        : 'Chưa chọn ghế'}
                    </strong>
                  </div>

                  {selectedSeats.length > 0 && (
                    <div className="selected-seats-pills">
                      {selectedSeats.map((s) => (
                        <span key={s} className="seat-pill">
                          Số {s}
                        </span>
                      ))}
                    </div>
                  )}
                </div>

                <div className="summary-divider" />

                {/* PRICE CALCULATION */}
                <div className="summary-section">
                  <div className="summary-row">
                    <span className="label">Số lượng vé:</span>
                    <span className="val">{selectedSeats.length} vé</span>
                  </div>
                  <div className="summary-row">
                    <span className="label">Đơn giá:</span>
                    <span className="val">{formatCurrency(price)}</span>
                  </div>
                  <div className="summary-row">
                    <span className="label">Bảo hiểm hành khách:</span>
                    <span className="val text-teal">Đã bao gồm (0đ)</span>
                  </div>

                  <div className="summary-total-row">
                    <span>Tổng tiền tạm tính:</span>
                    <strong className="total-price-val">
                      {formatCurrency(totalPrice)}
                    </strong>
                  </div>
                </div>

                {/* SUBMIT BUTTON */}
                <button
                  type="button"
                  className="btn-proceed-checkout"
                  disabled={selectedSeats.length === 0}
                  onClick={handleProceedToCheckout}
                >
                  Tiếp tục thanh toán ({formatCurrency(totalPrice)})
                </button>

                <p className="summary-disclaimer">
                  * Vé được giữ trong vòng 15 phút sau khi sang bước thanh toán.
                </p>
              </div>
            </aside>
          </div>
        )}
      </div>
    </div>
  );
}
