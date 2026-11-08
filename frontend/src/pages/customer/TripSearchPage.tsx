import { useEffect, useMemo, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { api, buildQuery } from '../../api/client';
import type { Route, TripSearchResponse } from '../../api/types';
import { BookingStepper } from '../../components/customer/BookingStepper';
import { formatCurrency, formatTime } from '../../utils/format';
import './TripSearchPage.css';

export function TripSearchPage() {
  const location = useLocation();
  const navigate = useNavigate();

  // Parse query params
  const searchParams = new URLSearchParams(location.search);
  const initDeparture = searchParams.get('departure') || 'TP HCM';
  const initDestination = searchParams.get('destination') || 'Đà Lạt';
  const initDate =
    searchParams.get('date') || new Date().toISOString().slice(0, 10);

  const [routes, setRoutes] = useState<Route[]>([]);
  const [departure, setDeparture] = useState(initDeparture);
  const [destination, setDestination] = useState(initDestination);
  const [travelDate, setTravelDate] = useState(initDate);

  const [loading, setLoading] = useState(false);
  const [trips, setTrips] = useState<TripSearchResponse[]>([]);
  const [errorMsg, setErrorMsg] = useState('');

  // Filters
  const [selectedCompanies, setSelectedCompanies] = useState<string[]>([]);
  const [selectedVehicleTypes, setSelectedVehicleTypes] = useState<string[]>([]);
  const [timeFilter, setTimeFilter] = useState<string>('all'); // 'all', 'morning', 'afternoon', 'evening', 'night'
  const [sortOrder, setSortOrder] = useState<string>('time-asc'); // 'time-asc', 'price-asc', 'price-desc'

  // Load routes on mount
  useEffect(() => {
    void (async () => {
      try {
        const data = await api.get<Route[]>('/routes');
        if (Array.isArray(data)) {
          setRoutes(data);
        }
      } catch (err) {
        console.error('Failed to load routes', err);
      }
    })();
  }, []);

  // Fetch trips when search params change or form submitted
  const fetchTrips = async (dep: string, dest: string, d: string) => {
    setLoading(true);
    setErrorMsg('');
    try {
      const qs = buildQuery({
        departureLocation: dep,
        arrivalLocation: dest,
        departureDate: d,
      });
      const res = await api.get<TripSearchResponse[]>(`/trips/search${qs}`);
      setTrips(Array.isArray(res) ? res : []);
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : 'Lỗi khi tra cứu chuyến xe';
      setErrorMsg(msg);
      setTrips([]);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    void fetchTrips(departure, destination, travelDate);
  }, []);

  const handleSearchSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    void fetchTrips(departure, destination, travelDate);
  };

  // Departure and Destination options
  const departureOptions = useMemo(() => {
    const list = Array.from(
      new Set(routes.map((r) => r.departure ?? r.diemDi).filter(Boolean)),
    ) as string[];
    return list.length > 0 ? list : ['TP HCM', 'Hà Nội', 'Đà Nẵng', 'Cần Thơ'];
  }, [routes]);

  const destinationOptions = useMemo(() => {
    const filtered = routes
      .filter((r) => !departure || (r.departure ?? r.diemDi) === departure)
      .map((r) => r.destination ?? r.diemDen)
      .filter(Boolean) as string[];
    const list = Array.from(new Set(filtered));
    return list.length > 0 ? list : ['Đà Lạt', 'Vũng Tàu', 'Nha Trang', 'Phan Thiết'];
  }, [routes, departure]);

  // Available Companies and Vehicle Types for Filter
  const availableCompanies = useMemo(() => {
    const names = trips
      .map((t) => t.companyName ?? t.tenNhaXe)
      .filter(Boolean) as string[];
    return Array.from(new Set(names));
  }, [trips]);

  const availableVehicleTypes = useMemo(() => {
    const types = trips
      .map((t) => t.typeName ?? t.tenLoaiXe)
      .filter(Boolean) as string[];
    return Array.from(new Set(types));
  }, [trips]);

  // Filter & Sort trips
  const filteredTrips = useMemo(() => {
    return trips
      .filter((t) => {
        const comp = t.companyName ?? t.tenNhaXe ?? '';
        if (selectedCompanies.length > 0 && !selectedCompanies.includes(comp)) {
          return false;
        }

        const type = t.typeName ?? t.tenLoaiXe ?? '';
        if (
          selectedVehicleTypes.length > 0 &&
          !selectedVehicleTypes.includes(type)
        ) {
          return false;
        }

        const depTime = t.departureTime ?? t.ngayGioKhoiHanh;
        if (depTime && timeFilter !== 'all') {
          const hour = new Date(depTime).getHours();
          if (timeFilter === 'early' && (hour < 0 || hour >= 6)) return false;
          if (timeFilter === 'morning' && (hour < 6 || hour >= 12)) return false;
          if (timeFilter === 'afternoon' && (hour < 12 || hour >= 18))
            return false;
          if (timeFilter === 'evening' && (hour < 18 || hour >= 24)) return false;
        }

        return true;
      })
      .sort((a, b) => {
        const priceA = a.actualPrice ?? a.giaVeThucTe ?? 0;
        const priceB = b.actualPrice ?? b.giaVeThucTe ?? 0;
        const timeA = new Date(
          a.departureTime ?? a.ngayGioKhoiHanh ?? 0,
        ).getTime();
        const timeB = new Date(
          b.departureTime ?? b.ngayGioKhoiHanh ?? 0,
        ).getTime();

        if (sortOrder === 'price-asc') return priceA - priceB;
        if (sortOrder === 'price-desc') return priceB - priceA;
        return timeA - timeB;
      });
  }, [trips, selectedCompanies, selectedVehicleTypes, timeFilter, sortOrder]);

  const toggleCompany = (company: string) => {
    setSelectedCompanies((prev) =>
      prev.includes(company)
        ? prev.filter((c) => c !== company)
        : [...prev, company],
    );
  };

  const toggleVehicleType = (type: string) => {
    setSelectedVehicleTypes((prev) =>
      prev.includes(type) ? prev.filter((t) => t !== type) : [...prev, type],
    );
  };

  return (
    <div className="trip-search-page">
      {/* 1. STEPPER */}
      <BookingStepper currentStep={1} />

      {/* 2. SEARCH BAR HEADER */}
      <section className="search-bar-strip">
        <div className="search-bar-inner">
          <form className="search-inline-form" onSubmit={handleSearchSubmit}>
            <div className="search-input-group">
              <label>Điểm xuất phát</label>
              <select
                value={departure}
                onChange={(e) => setDeparture(e.target.value)}
              >
                {departureOptions.map((opt) => (
                  <option key={opt} value={opt}>
                    {opt}
                  </option>
                ))}
              </select>
            </div>

            <div className="search-input-group">
              <label>Điểm đến</label>
              <select
                value={destination}
                onChange={(e) => setDestination(e.target.value)}
              >
                {destinationOptions.map((opt) => (
                  <option key={opt} value={opt}>
                    {opt}
                  </option>
                ))}
              </select>
            </div>

            <div className="search-input-group">
              <label>Ngày khởi hành</label>
              <input
                type="date"
                value={travelDate}
                min={new Date().toISOString().slice(0, 10)}
                onChange={(e) => setTravelDate(e.target.value)}
              />
            </div>

            <button type="submit" className="btn-search-submit">
              <svg
                width="18"
                height="18"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="2.5"
              >
                <circle cx="11" cy="11" r="8" />
                <line x1="21" y1="21" x2="16.65" y2="16.65" />
              </svg>
              <span>Tìm chuyến</span>
            </button>
          </form>
        </div>
      </section>

      {/* 3. MAIN RESULTS CONTAINER */}
      <div className="search-results-wrapper">
        <div className="search-results-layout">
          {/* SIDEBAR FILTER */}
          <aside className="search-filters-sidebar">
            <div className="filter-card">
              <div className="filter-card-head">
                <h3>Bộ lọc tìm kiếm</h3>
                {(selectedCompanies.length > 0 ||
                  selectedVehicleTypes.length > 0 ||
                  timeFilter !== 'all') && (
                  <button
                    type="button"
                    className="btn-reset-filters"
                    onClick={() => {
                      setSelectedCompanies([]);
                      setSelectedVehicleTypes([]);
                      setTimeFilter('all');
                    }}
                  >
                    Xóa lọc
                  </button>
                )}
              </div>

              {/* Time of day filter */}
              <div className="filter-section">
                <h4>Giờ xuất bến</h4>
                <div className="filter-chips">
                  {[
                    { id: 'all', label: 'Tất cả' },
                    { id: 'early', label: 'Sáng sớm (00:00 - 06:00)' },
                    { id: 'morning', label: 'Buổi sáng (06:00 - 12:00)' },
                    { id: 'afternoon', label: 'Buổi chiều (12:00 - 18:00)' },
                    { id: 'evening', label: 'Buổi tối (18:00 - 24:00)' },
                  ].map((t) => (
                    <button
                      key={t.id}
                      type="button"
                      className={`filter-chip ${
                        timeFilter === t.id ? 'active' : ''
                      }`}
                      onClick={() => setTimeFilter(t.id)}
                    >
                      {t.label}
                    </button>
                  ))}
                </div>
              </div>

              {/* Bus Companies Filter */}
              {availableCompanies.length > 0 && (
                <div className="filter-section">
                  <h4>Nhà xe ({availableCompanies.length})</h4>
                  <div className="filter-checkbox-list">
                    {availableCompanies.map((comp) => (
                      <label key={comp} className="checkbox-row">
                        <input
                          type="checkbox"
                          checked={selectedCompanies.includes(comp)}
                          onChange={() => toggleCompany(comp)}
                        />
                        <span>{comp}</span>
                      </label>
                    ))}
                  </div>
                </div>
              )}

              {/* Vehicle Types Filter */}
              {availableVehicleTypes.length > 0 && (
                <div className="filter-section">
                  <h4>Loại xe</h4>
                  <div className="filter-checkbox-list">
                    {availableVehicleTypes.map((type) => (
                      <label key={type} className="checkbox-row">
                        <input
                          type="checkbox"
                          checked={selectedVehicleTypes.includes(type)}
                          onChange={() => toggleVehicleType(type)}
                        />
                        <span>{type}</span>
                      </label>
                    ))}
                  </div>
                </div>
              )}
            </div>
          </aside>

          {/* RESULTS LIST */}
          <main className="search-trips-main">
            <div className="results-header-bar">
              <div className="results-title-info">
                <h2>
                  Tuyến {departure} ➔ {destination}
                </h2>
                <p>
                  Ngày đi: <strong>{travelDate}</strong> · Tìm thấy{' '}
                  <strong>{filteredTrips.length}</strong> chuyến xe phù hợp
                </p>
              </div>

              <div className="results-sort-bar">
                <span>Sắp xếp:</span>
                <select
                  value={sortOrder}
                  onChange={(e) => setSortOrder(e.target.value)}
                >
                  <option value="time-asc">Giờ sớm nhất</option>
                  <option value="price-asc">Giá thấp nhất</option>
                  <option value="price-desc">Giá cao nhất</option>
                </select>
              </div>
            </div>

            {errorMsg && <div className="search-alert error">{errorMsg}</div>}

            {loading ? (
              <div className="search-loading-state">
                <span className="spinner-large" />
                <p>Đang tải dữ liệu chuyến xe từ Bến xe Miền Đông...</p>
              </div>
            ) : filteredTrips.length === 0 ? (
              <div className="no-trips-state">
                <svg
                  width="48"
                  height="48"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="#94a3b8"
                  strokeWidth="1.5"
                >
                  <circle cx="12" cy="12" r="10" />
                  <line x1="8" y1="12" x2="16" y2="12" />
                </svg>
                <h3>Không tìm thấy chuyến xe nào</h3>
                <p>
                  Hiện không có chuyến xe nào chạy tuyến {departure} đi{' '}
                  {destination} vào ngày {travelDate}. Quý khách vui lòng chọn
                  ngày khác hoặc điều chỉnh bộ lọc.
                </p>
              </div>
            ) : (
              <div className="trip-results-list">
                {filteredTrips.map((trip) => {
                  const tripId = trip.tripId ?? trip.maChuyen;
                  const companyName = trip.companyName ?? trip.tenNhaXe ?? 'Bến xe Miền Đông';
                  const typeName = trip.typeName ?? trip.tenLoaiXe ?? 'Xe khách';
                  const seatCap = trip.seatCapacity ?? trip.soCho ?? 0;
                  const booked = trip.bookedSeats ?? trip.soGheDaDat ?? 0;
                  const seatsLeft = Math.max(0, seatCap - booked);
                  const price = trip.actualPrice ?? trip.giaVeThucTe ?? 0;
                  const depTime = trip.departureTime ?? trip.ngayGioKhoiHanh;
                  const arrTime = trip.estimatedArrivalTime ?? trip.ngayGioDenDuKien;
                  const licensePlate = trip.licensePlate ?? trip.bienSoXe ?? '';
                  const driverName = trip.driverName ?? trip.tenTx ?? '';

                  return (
                    <div key={tripId} className="trip-card-item">
                      <div className="trip-card-operator">
                        <div className="operator-badge-initial">
                          {companyName.charAt(0).toUpperCase()}
                        </div>
                        <div className="operator-meta">
                          <strong className="operator-name">{companyName}</strong>
                          <span className="operator-type">
                            {typeName} ({seatCap} chỗ) {licensePlate ? `· BKS: ${licensePlate}` : ''} {driverName ? `· Tài xế: ${driverName}` : ''}
                          </span>
                        </div>
                      </div>

                      <div className="trip-card-body">
                        {/* Timeline */}
                        <div className="trip-route-timeline">
                          <div className="time-node">
                            <span className="time-text">
                              {depTime ? formatTime(depTime) : '--:--'}
                            </span>
                            <span className="place-text">{trip.departure ?? trip.diemDi}</span>
                          </div>

                          <div className="timeline-connector">
                            <span className="dot-line" />
                            <span className="bus-symbol">🚌</span>
                          </div>

                          <div className="time-node">
                            <span className="time-text">
                              {arrTime ? formatTime(arrTime) : '--:--'}
                            </span>
                            <span className="place-text">{trip.destination ?? trip.diemDen}</span>
                          </div>
                        </div>

                        {/* Amenities */}
                        <div className="trip-features">
                          <span className="feature-pill">Bảo hiểm hành khách</span>
                          <span className="feature-pill">Nước suối & Khăn</span>
                          <span className="feature-pill">Wifi bến xe</span>
                          <span className="feature-pill">Đúng giờ xuất bến</span>
                        </div>

                        {/* Price & Action */}
                        <div className="trip-pricing-box">
                          <div className="price-col">
                            <span className="price-label">Giá niêm yết</span>
                            <span className="price-value">{formatCurrency(price)}</span>
                            <span
                              className={`seats-left-pill ${
                                seatsLeft <= 5 ? 'urgent' : ''
                              }`}
                            >
                              Còn {seatsLeft} chỗ trống
                            </span>
                          </div>

                          <button
                            type="button"
                            className="btn-select-trip"
                            disabled={seatsLeft === 0}
                            onClick={() =>
                              navigate(`/dat-ve/chon-ghe/${tripId}`, {
                                state: {
                                  trip,
                                  travelDate,
                                  departure,
                                  destination,
                                },
                              })
                            }
                          >
                            {seatsLeft === 0 ? 'Hết chỗ' : 'Chọn chuyến'}
                          </button>
                        </div>
                      </div>
                    </div>
                  );
                })}
              </div>
            )}
          </main>
        </div>
      </div>
    </div>
  );
}
