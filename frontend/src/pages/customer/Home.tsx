import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { api, getErrorMessage } from '../../api/client';
import type { BusCompany, Route, Trip } from '../../api/types';
import { formatVnd, todayISO } from '../../utils/format';
import { CustomerNavbar } from '../../components/layout/CustomerNavbar';
import './Home.css';

const DEFAULT_LOCATIONS = [
  'TP HCM',
  'Đà Lạt',
  'Vũng Tàu',
  'Nha Trang',
  'Phan Thiết',
  'Quy Nhơn',
  'Đà Nẵng',
  'Buôn Ma Thuột',
];

const LOCATION_METAS: Record<string, { desc: string; region: string }> = {
  'TP HCM': { desc: 'Bến xe Miền Đông Mới · TP. Thủ Đức', region: 'Đông Nam Bộ' },
  'Đà Lạt': { desc: 'Bến xe liên tỉnh Đà Lạt', region: 'Tây Nguyên' },
  'Vũng Tàu': { desc: 'Bến xe khách Vũng Tàu', region: 'Bà Rịa - Vũng Tàu' },
  'Nha Trang': { desc: 'Bến xe phía Nam Nha Trang', region: 'Khánh Hòa' },
  'Phan Thiết': { desc: 'Bến xe Bắc Phan Thiết', region: 'Bình Thuận' },
  'Quy Nhơn': { desc: 'Bến xe trung tâm Quy Nhơn', region: 'Bình Định' },
  'Đà Nẵng': { desc: 'Bến xe trung tâm Đà Nẵng', region: 'Đà Nẵng' },
  'Buôn Ma Thuột': { desc: 'Bến xe liên tỉnh Đắk Lắk', region: 'Đắk Lắk' },
};

function formatVietnameseDate(isoDate: string): { formatted: string; weekday: string } {
  if (!isoDate) return { formatted: '', weekday: '' };
  try {
    const [y, m, d] = isoDate.split('-').map(Number);
    const formatted = `${String(d).padStart(2, '0')}/${String(m).padStart(2, '0')}/${y}`;

    const date = new Date(y, m - 1, d);
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    date.setHours(0, 0, 0, 0);

    const diffDays = Math.round((date.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));
    const weekdays = ['Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'];
    const dayName = weekdays[date.getDay()] ?? '';

    let weekday = dayName;
    if (diffDays === 0) weekday = `Hôm nay · ${dayName}`;
    else if (diffDays === 1) weekday = `Ngày mai · ${dayName}`;

    return { formatted, weekday };
  } catch {
    return { formatted: isoDate, weekday: '' };
  }
}

export function Home() {
  const navigate = useNavigate();

  // Dữ liệu thực từ database
  const [routes, setRoutes] = useState<Route[]>([]);
  const [busCompanies, setBusCompanies] = useState<BusCompany[]>([]);
  const [loadingInitial, setLoadingInitial] = useState(true);

  // Form tìm kiếm - khởi tạo mặc định TP HCM -> Đà Lạt
  const [departure, setDeparture] = useState('TP HCM');
  const [destination, setDestination] = useState('Đà Lạt');
  const [travelDate, setTravelDate] = useState(todayISO());

  // Custom Dropdowns cho Điểm đi & Điểm đến
  const [openDeparture, setOpenDeparture] = useState(false);
  const [openDestination, setOpenDestination] = useState(false);
  const departureRef = useRef<HTMLDivElement>(null);
  const destinationRef = useRef<HTMLDivElement>(null);
  const dateInputRef = useRef<HTMLInputElement>(null);

  // Kết quả tìm kiếm thực tế
  const [trips, setTrips] = useState<Trip[]>([]);
  const [searched, setSearched] = useState(false);
  const [loadingTrips, setLoadingTrips] = useState(false);
  const [searchError, setSearchError] = useState('');

  // Đóng dropdowns khi click bên ngoài
  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (departureRef.current && !departureRef.current.contains(e.target as Node)) {
        setOpenDeparture(false);
      }
      if (destinationRef.current && !destinationRef.current.contains(e.target as Node)) {
        setOpenDestination(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  // Tải dữ liệu thật từ Backend (Tuyến đường và Nhà xe)
  useEffect(() => {
    let isMounted = true;
    Promise.all([
      api.get<Route[]>('/routes', { redirectOn401: false }).catch(() => [] as Route[]),
      api.get<BusCompany[]>('/bus-companies', { redirectOn401: false }).catch(() => [] as BusCompany[]),
    ]).then(([routesData, companiesData]) => {
      if (!isMounted) return;
      const activeRoutes = (routesData || []).filter((r) => (r.isActive ?? r.trangThai) !== false);
      const activeCompanies = (companiesData || []).filter((c) => (c.isActive ?? c.trangThai) !== false);

      setRoutes(activeRoutes);
      setBusCompanies(activeCompanies);
      setLoadingInitial(false);

      if (activeRoutes.length > 0) {
        const preferred = activeRoutes.find((r) => {
          const dep = (r.departure ?? r.diemDi ?? '').toLowerCase();
          return dep.includes('tp hcm') || dep.includes('miền đông') || dep.includes('hồ chí minh');
        });
        const selected = preferred || activeRoutes[0];
        setDeparture(selected.departure ?? selected.diemDi ?? 'TP HCM');
        setDestination(selected.destination ?? selected.diemDen ?? 'Đà Lạt');
      }
    });

    return () => {
      isMounted = false;
    };
  }, []);

  // Danh sách các điểm đi duy nhất có trong database
  const departureOptions = useMemo(() => {
    if (routes.length === 0) return DEFAULT_LOCATIONS;
    const set = new Set<string>();
    routes.forEach((r) => {
      const dep = r.departure ?? r.diemDi;
      if (dep) set.add(dep);
    });
    return Array.from(set);
  }, [routes]);

  // Danh sách các điểm đến tương ứng với điểm đi đã chọn
  const destinationOptions = useMemo(() => {
    if (routes.length === 0) {
      return DEFAULT_LOCATIONS.filter((loc) => loc !== departure);
    }
    const filtered = routes
      .filter((r) => (r.departure ?? r.diemDi) === departure)
      .map((r) => r.destination ?? r.diemDen)
      .filter(Boolean) as string[];

    if (filtered.length === 0) {
      const allTo = Array.from(
        new Set(routes.map((r) => r.destination ?? r.diemDen).filter(Boolean)),
      ).filter((d) => d !== departure) as string[];
      return allTo.length > 0 ? allTo : DEFAULT_LOCATIONS.filter((loc) => loc !== departure);
    }
    return Array.from(new Set(filtered));
  }, [routes, departure]);

  const handleDepartureChange = (newDeparture: string) => {
    setDeparture(newDeparture);
    if (routes.length > 0) {
      const validDestinations = routes
        .filter((r) => (r.departure ?? r.diemDi) === newDeparture)
        .map((r) => r.destination ?? r.diemDen)
        .filter(Boolean) as string[];
      if (validDestinations.length > 0 && !validDestinations.includes(destination)) {
        setDestination(validDestinations[0]);
      }
    } else if (destination === newDeparture) {
      const fallback = DEFAULT_LOCATIONS.find((loc) => loc !== newDeparture) || 'Đà Lạt';
      setDestination(fallback);
    }
  };

  const popularRoutesFromDb = useMemo(() => {
    return routes.slice(0, 6);
  }, [routes]);

  const handleSwap = () => {
    const temp = departure;
    setDeparture(destination);
    setDestination(temp);
  };

  const handleSearch = useCallback(
    async (overrideDeparture?: string, overrideDestination?: string) => {
      const from = overrideDeparture ?? departure;
      const to = overrideDestination ?? destination;

      if (!from || !to) {
        setSearchError('Vui lòng chọn Điểm đi và Điểm đến để tra cứu chuyến xe.');
        return;
      }

      setLoadingTrips(true);
      setSearchError('');
      setSearched(true);

      try {
        const queryParams = new URLSearchParams();
        if (from) {
          queryParams.set('departure', from);
          queryParams.set('diemDi', from);
        }
        if (to) {
          queryParams.set('destination', to);
          queryParams.set('diemDen', to);
        }
        if (travelDate) {
          queryParams.set('date', travelDate);
          queryParams.set('ngay', travelDate);
        }

        const data = await api.get<Trip[]>(`/trips/search?${queryParams.toString()}`, {
          redirectOn401: false,
        });
        setTrips(data || []);
      } catch (err) {
        setSearchError(getErrorMessage(err));
        setTrips([]);
      } finally {
        setLoadingTrips(false);
      }
    },
    [departure, destination, travelDate],
  );

  const handleSelectQuickRoute = (route: Route) => {
    const dep = route.departure ?? route.diemDi ?? '';
    const dest = route.destination ?? route.diemDen ?? '';
    setDeparture(dep);
    setDestination(dest);
    handleSearch(dep, dest);
    window.scrollTo({ top: 400, behavior: 'smooth' });
  };

  const handleSelectTrip = (trip: Trip) => {
    const id = trip.tripId ?? trip.maChuyen;
    navigate(`/dat-ve/chon-ghe/${id}`, {
      state: {
        trip,
        travelDate,
        departure: trip.departure ?? trip.diemDi ?? departure,
        destination: trip.destination ?? trip.diemDen ?? destination,
      },
    });
  };

  return (
    <div className="home-container">
      {/* 1. TOP NAVIGATION BAR */}
      <CustomerNavbar />

      {/* 2. HERO SECTION */}
      <section className="hero-section">
        <div className="hero-overlay-gradient" />
        <div className="hero-content">
          <div className="hero-badge-tag">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="currentColor">
              <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z" />
            </svg>
            HỆ THỐNG VÉ CHÍNH THỨC
          </div>

          <h1 className="hero-title">
            Kết Nối Mọi Hành Trình <br />
            <span className="highlight-text">Từ Bến Xe Miền Đông</span>
          </h1>

          <p className="hero-subtitle">
            Hơn {routes.length > 0 ? routes.length : '14'} tuyến liên tỉnh phục vụ bởi {busCompanies.length > 0 ? busCompanies.length : '5'} đơn vị vận tải uy tín.
            Đặt vé và giữ chỗ trực tuyến an toàn theo giá niêm yết chính thức.
          </p>

          <div className="hero-trust-badges">
            <div className="trust-badge">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
                <path d="M20 6L9 17l-5-5" />
              </svg>
              <span>Giữ đúng 100% chỗ ngồi</span>
            </div>
            <div className="trust-badge">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
                <path d="M12 1v22M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6" />
              </svg>
              <span>Giá vé niêm yết không phụ thu</span>
            </div>
            <div className="trust-badge">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
                <path d="M21 12a9 9 0 0 0-9-9 9.75 9.75 0 0 0-6.74 2.74L3 8" />
                <path d="M3 3v5h5M3 12a9 9 0 0 0 9 9 9.75 9.75 0 0 0 6.74-2.74L21 16" />
                <path d="M16 21h5v-5" />
              </svg>
              <span>Hủy vé dễ dàng trước giờ chạy</span>
            </div>
            <div className="trust-badge">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
                <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2" />
                <circle cx="9" cy="7" r="4" />
                <path d="M23 21v-2a4 4 0 0 0-3-3.87" />
                <path d="M16 3.13a4 4 0 0 1 0 7.75" />
              </svg>
              <span>Xác nhận vé điện tử tức thì</span>
            </div>
          </div>

          {/* FLOATING BOOKING WIDGET */}
          <div className="hero-booking-widget">
            <form
              className="widget-search-form"
              onSubmit={(e) => {
                e.preventDefault();
                handleSearch();
              }}
            >
              {/* ĐIỂM ĐI */}
              <div className={`form-field-group field-from ${openDeparture ? 'field-open' : ''}`} ref={departureRef}>
                <label>ĐIỂM ĐI</label>
                <button
                  type="button"
                  className="custom-select-trigger"
                  onClick={() => {
                    setOpenDeparture((prev) => !prev);
                    setOpenDestination(false);
                  }}
                  aria-expanded={openDeparture}
                >
                  <div className="trigger-left">
                    <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#1e40af" strokeWidth="2">
                      <circle cx="12" cy="12" r="10" />
                      <circle cx="12" cy="12" r="3" fill="#1e40af" />
                    </svg>
                    <span className="trigger-value">{departure}</span>
                  </div>
                  <svg
                    className={`trigger-chevron ${openDeparture ? 'open' : ''}`}
                    width="16"
                    height="16"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="#757684"
                    strokeWidth="2.5"
                  >
                    <polyline points="6 9 12 15 18 9" />
                  </svg>
                </button>
                <span className="field-subtext">
                  {LOCATION_METAS[departure]?.desc || 'Điểm khởi hành tại bến'}
                </span>

                <button
                  type="button"
                  className="btn-swap-stations"
                  onClick={() => {
                    handleSwap();
                    setOpenDeparture(false);
                    setOpenDestination(false);
                  }}
                  title="Đảo chiều điểm đi và đến"
                >
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
                    <path d="M7 16V4M7 4L3 8M7 4L11 8M17 8V20M17 20L21 16M17 20L13 16" />
                  </svg>
                </button>

                {openDeparture && (
                  <div className="custom-dropdown-panel">
                    <div className="dropdown-panel-header">
                      <span>CHỌN ĐIỂM KHỞI HÀNH</span>
                      <span className="dropdown-panel-badge">{departureOptions.length} điểm</span>
                    </div>
                    <div className="dropdown-panel-list">
                      {departureOptions.map((opt) => {
                        const isSelected = departure === opt;
                        const meta = LOCATION_METAS[opt];
                        return (
                          <button
                            key={opt}
                            type="button"
                            className={`dropdown-option-item ${isSelected ? 'active' : ''}`}
                            onClick={() => {
                              handleDepartureChange(opt);
                              setOpenDeparture(false);
                            }}
                          >
                            <div className="opt-icon-circle">
                              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                                <path d="M12 2a8 8 0 0 0-8 8c0 5.25 8 12 8 12s8-6.75 8-12a8 8 0 0 0-8-8z" />
                                <circle cx="12" cy="10" r="3" />
                              </svg>
                            </div>
                            <div className="opt-text-wrap">
                              <div className="opt-title-row">
                                <span className="opt-name">{opt}</span>
                                {meta?.region && <span className="opt-region-pill">{meta.region}</span>}
                              </div>
                              <span className="opt-desc">{meta?.desc || 'Điểm đón bến xe liên tỉnh'}</span>
                            </div>
                            {isSelected && (
                              <div className="opt-check-icon">
                                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#1e40af" strokeWidth="3">
                                  <polyline points="20 6 9 17 4 12" />
                                </svg>
                              </div>
                            )}
                          </button>
                        );
                      })}
                    </div>
                  </div>
                )}
              </div>

              {/* ĐIỂM ĐẾN */}
              <div className={`form-field-group field-to ${openDestination ? 'field-open' : ''}`} ref={destinationRef}>
                <label>ĐIỂM ĐẾN</label>
                <button
                  type="button"
                  className="custom-select-trigger"
                  onClick={() => {
                    setOpenDestination((prev) => !prev);
                    setOpenDeparture(false);
                  }}
                  aria-expanded={openDestination}
                >
                  <div className="trigger-left">
                    <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#ea580c" strokeWidth="2">
                      <path d="M12 2a8 8 0 0 0-8 8c0 5.25 8 12 8 12s8-6.75 8-12a8 8 0 0 0-8-8z" />
                      <circle cx="12" cy="10" r="3" />
                    </svg>
                    <span className="trigger-value">{destination}</span>
                  </div>
                  <svg
                    className={`trigger-chevron ${openDestination ? 'open' : ''}`}
                    width="16"
                    height="16"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="#757684"
                    strokeWidth="2.5"
                  >
                    <polyline points="6 9 12 15 18 9" />
                  </svg>
                </button>
                <span className="field-subtext">
                  {LOCATION_METAS[destination]?.desc || 'Bến xe đến / Điểm trả'}
                </span>

                {openDestination && (
                  <div className="custom-dropdown-panel">
                    <div className="dropdown-panel-header">
                      <span>CHỌN ĐIỂM ĐẾN</span>
                      <span className="dropdown-panel-badge">{destinationOptions.length} tuyến</span>
                    </div>
                    <div className="dropdown-panel-list">
                      {destinationOptions.map((opt) => {
                        const isSelected = destination === opt;
                        const meta = LOCATION_METAS[opt];
                        return (
                          <button
                            key={opt}
                            type="button"
                            className={`dropdown-option-item ${isSelected ? 'active' : ''}`}
                            onClick={() => {
                              setDestination(opt);
                              setOpenDestination(false);
                            }}
                          >
                            <div className="opt-icon-circle to-icon">
                              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                                <path d="M12 2a8 8 0 0 0-8 8c0 5.25 8 12 8 12s8-6.75 8-12a8 8 0 0 0-8-8z" />
                                <circle cx="12" cy="10" r="3" />
                              </svg>
                            </div>
                            <div className="opt-text-wrap">
                              <div className="opt-title-row">
                                <span className="opt-name">{opt}</span>
                                {meta?.region && <span className="opt-region-pill">{meta.region}</span>}
                              </div>
                              <span className="opt-desc">{meta?.desc || 'Điểm trả khách tại bến'}</span>
                            </div>
                            {isSelected && (
                              <div className="opt-check-icon to-check">
                                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#ea580c" strokeWidth="3">
                                  <polyline points="20 6 9 17 4 12" />
                                </svg>
                              </div>
                            )}
                          </button>
                        );
                      })}
                    </div>
                  </div>
                )}
              </div>

              {/* NGÀY ĐI */}
              <div className="form-field-group field-date">
                <div className="field-label-row">
                  <label>NGÀY ĐI</label>
                  <div className="quick-date-links">
                    <button type="button" onClick={() => setTravelDate(todayISO())}>
                      Hôm nay
                    </button>
                    <span>•</span>
                    <button
                      type="button"
                      onClick={() => {
                        const d = new Date();
                        d.setDate(d.getDate() + 1);
                        setTravelDate(d.toISOString().slice(0, 10));
                      }}
                    >
                      Ngày mai
                    </button>
                  </div>
                </div>

                <div
                  className="date-picker-wrap"
                  onClick={() => dateInputRef.current?.showPicker?.()}
                  style={{ cursor: 'pointer' }}
                >
                  <div className="trigger-left">
                    <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#1e40af" strokeWidth="2">
                      <rect x="3" y="4" width="18" height="18" rx="2" ry="2" />
                      <line x1="16" y1="2" x2="16" y2="6" />
                      <line x1="8" y1="2" x2="8" y2="6" />
                      <line x1="3" y1="10" x2="21" y2="10" />
                    </svg>
                    <span className="trigger-value" style={{ fontWeight: 600 }}>
                      {formatVietnameseDate(travelDate).formatted}
                    </span>
                  </div>
                  <input
                    ref={dateInputRef}
                    type="date"
                    className="hidden-date-input"
                    value={travelDate}
                    min={todayISO()}
                    onChange={(e) => setTravelDate(e.target.value)}
                    required
                    title="Chọn ngày khởi hành (định dạng Ngày/Tháng/Năm)"
                  />
                </div>
                <span className="field-subtext">{formatVietnameseDate(travelDate).weekday}</span>
              </div>

              <div className="form-action-group">
                <button type="submit" className="btn-search-hero" disabled={loadingTrips || loadingInitial}>
                  {loadingTrips ? (
                    <span>Đang tìm...</span>
                  ) : (
                    <>
                      <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
                        <circle cx="11" cy="11" r="8" />
                        <line x1="21" y1="21" x2="16.65" y2="16.65" />
                      </svg>
                      <span>Tìm Chuyến Xe</span>
                    </>
                  )}
                </button>
              </div>
            </form>

            {/* TUYẾN ĐƯỜNG PHỔ BIẾN THỰC TẾ TRONG DATABASE */}
            {popularRoutesFromDb.length > 0 && (
              <div className="quick-suggestions-row" id="tuyen-duong">
                <span className="suggestion-label">
                  <svg width="16" height="16" viewBox="0 0 24 24" fill="#ea580c">
                    <path d="M17.5 12a5.5 5.5 0 1 1-11 0c0-1.87 1.02-3.62 2.6-4.57.3-.18.7.02.73.37.1 1.07.72 2.02 1.67 2.45.2.09.4-.04.4-.26 0-.36-.07-.71-.2-1.04C10.5 6.47 12.35 4 14.5 4c.26 0 .44.25.37.5-.47 1.62-.2 3.39.73 4.75.14.21.43.25.62.1.4-.3.73-.67 1-1.08.15-.24.48-.22.61.03 1.05 2.05.67 4.7-.33 6.7z" />
                  </svg>
                  Tuyến xe kết nối:
                </span>
                <div className="suggestion-pills">
                  {popularRoutesFromDb.map((route) => {
                    const id = route.routeId ?? route.maTuyen;
                    const dep = route.departure ?? route.diemDi;
                    const dest = route.destination ?? route.diemDen;
                    const dist = route.distance ?? route.quangDuong;
                    return (
                      <button
                        key={id}
                        type="button"
                        className="suggestion-pill"
                        onClick={() => handleSelectQuickRoute(route)}
                      >
                        <span>{dep} ➔ {dest}</span>
                        {dist && <span className="pill-distance">({dist} km)</span>}
                      </button>
                    );
                  })}
                </div>
              </div>
            )}
          </div>
        </div>
      </section>

      {/* 3. KẾT QUẢ TÌM KIẾM CHUYẾN XE */}
      {searched && (
        <section className="search-results-section" id="ket-qua">
          <div className="results-container">
            <div className="results-header">
              <div>
                <h2>
                  Kết quả chuyến xe: <span className="text-primary">{departure} ➔ {destination}</span>
                </h2>
                <p className="results-sub">
                  Ngày khởi hành: <strong>{formatVietnameseDate(travelDate).formatted}</strong> · Tìm thấy <strong>{trips.length}</strong> chuyến phù hợp trong hệ thống
                </p>
              </div>
              <button
                type="button"
                className="btn-clear-search"
                onClick={() => {
                  setSearched(false);
                  setTrips([]);
                }}
              >
                Đóng kết quả
              </button>
            </div>

            {searchError && <div className="search-error-alert">{searchError}</div>}

            {loadingTrips ? (
              <div className="results-loading">
                <span className="spinner-large" />
                <p>Đang tra cứu dữ liệu chuyến xe từ hệ thống bến...</p>
              </div>
            ) : trips.length === 0 ? (
              <div className="no-trips-card">
                <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="#94a3b8" strokeWidth="1.5">
                  <circle cx="12" cy="12" r="10" />
                  <line x1="8" y1="12" x2="16" y2="12" />
                </svg>
                <h3>Chưa có chuyến xe nào xuất bến vào ngày {formatVietnameseDate(travelDate).formatted}</h3>
                <p>Hiện chưa có chuyến xe nào chạy tuyến này vào ngày đã chọn. Vui lòng chọn ngày khác hoặc thử tuyến đường khác.</p>
              </div>
            ) : (
              <div className="trip-cards-grid">
                {trips.map((trip) => {
                  const tripId = trip.tripId ?? trip.maChuyen;
                  const companyName = trip.companyName ?? trip.tenNhaXe ?? '';
                  const typeName = trip.typeName ?? trip.tenLoaiXe ?? '';
                  const seatCap = trip.seatCapacity ?? trip.soCho ?? 0;
                  const licensePlate = trip.licensePlate ?? trip.bienSoXe ?? '';
                  const driverName = trip.driverName ?? trip.tenTx ?? '';
                  const depTime = trip.departureTime ?? trip.ngayGioKhoiHanh;
                  const arrTime = trip.estimatedArrivalTime ?? trip.ngayGioDenDuKien;
                  const dep = trip.departure ?? trip.diemDi ?? '';
                  const dest = trip.destination ?? trip.diemDen ?? '';
                  const price = trip.actualPrice ?? trip.giaVeThucTe ?? 0;
                  const booked = trip.bookedSeats ?? trip.ticketsSold ?? trip.soGheDaDat ?? trip.soVeDaBan ?? 0;
                  const conTrong = seatCap - booked;

                  return (
                    <div key={tripId} className="trip-card">
                      <div className="trip-card-main">
                        <div className="operator-info">
                          <div className="operator-badge">
                            {companyName ? companyName.charAt(0).toUpperCase() : 'BX'}
                          </div>
                          <div>
                            <strong className="operator-name">{companyName || 'Nhà xe bến Miền Đông'}</strong>
                            <span className="vehicle-type-label">
                              {typeName} ({seatCap} chỗ) {licensePlate ? `· BKS: ${licensePlate}` : ''} {driverName ? `· Tài xế: ${driverName}` : ''}
                            </span>
                          </div>
                        </div>

                        <div className="trip-timeline">
                          <div className="timeline-node">
                            <span className="node-time">
                              {depTime
                                ? new Date(depTime).toLocaleTimeString('vi-VN', {
                                    hour: '2-digit',
                                    minute: '2-digit',
                                  })
                                : '--:--'}
                            </span>
                            <span className="node-place">{dep}</span>
                          </div>
                          <div className="timeline-divider">
                            <span className="divider-line" />
                            <span className="bus-icon-node">🚌</span>
                          </div>
                          <div className="timeline-node">
                            <span className="node-time">
                              {arrTime
                                ? new Date(arrTime).toLocaleTimeString('vi-VN', {
                                    hour: '2-digit',
                                    minute: '2-digit',
                                  })
                                : '--:--'}
                            </span>
                            <span className="node-place">{dest}</span>
                          </div>
                        </div>

                        <div className="trip-amenities">
                          <span className="amenity-chip">Bảo hiểm hành khách</span>
                          <span className="amenity-chip">Nước suối & khăn</span>
                          <span className="amenity-chip">Wifi bến xe</span>
                          <span className="amenity-chip">Đúng giờ xuất bến</span>
                        </div>
                      </div>

                      <div className="trip-card-action">
                        <div className="price-tag">
                          <span className="price-label">Giá vé niêm yết</span>
                          <strong className="price-val">{formatVnd(price)}</strong>
                        </div>
                        <span className={`seats-left-badge ${conTrong <= 5 ? 'seats-urgent' : ''}`}>
                          Còn {conTrong > 0 ? conTrong : 0} / {seatCap} chỗ trống
                        </span>
                        <button
                          type="button"
                          className="btn-select-trip"
                          onClick={() => handleSelectTrip(trip)}
                          disabled={conTrong <= 0}
                        >
                          {conTrong > 0 ? 'Chọn Chuyến' : 'Hết Chỗ'}
                        </button>
                      </div>
                    </div>
                  );
                })}
              </div>
            )}
          </div>
        </section>
      )}

      {/* 4. CÁC ĐƠN VỊ VẬN TẢI ĐỐI TÁC */}
      <section className="section-operators" id="doi-tac">
        <div className="section-container">
          <div className="text-center-wrapper">
            <span className="section-badge-chip">ĐỐI TÁC VẬN TẢI CHÍNH THỨC</span>
            <h2 className="section-title">Các nhà xe hoạt động tại Bến xe Miền Đông</h2>
            <p className="section-subtitle">
              Danh sách các nhà xe đã đăng ký luồng tuyến và niêm yết giá vé chính thức tại bến
            </p>
          </div>

          <div className="operators-grid">
            {busCompanies.map((op) => {
              const id = op.companyId ?? op.maNx;
              const name = op.companyName ?? op.tenNx ?? '';
              const phone = op.phone ?? op.sdt;
              return (
                <div key={id} className="operator-card">
                  <div className="operator-card-top">
                    <div className="operator-avatar" style={{ backgroundColor: '#1e40af20', color: '#1e40af' }}>
                      {name.charAt(0) || 'BX'}
                    </div>
                    <div className="operator-badge-active">
                      <span>Đang hoạt động</span>
                    </div>
                  </div>
                  <h4 className="op-name">{name}</h4>
                  <p className="op-type">
                    {phone ? `Hotline: ${phone}` : 'Liên hệ bến xe'}
                  </p>
                  <div className="operator-card-bottom">
                    <span>{op.email ? op.email : 'contact@benxemiendong.vn'}</span>
                  </div>
                </div>
              );
            })}
          </div>
        </div>
      </section>

      {/* 5. TẠI SAO CHỌN COACHBOOKING */}
      <section className="section-features">
        <div className="section-container">
          <div className="text-center-wrapper">
            <span className="section-badge-chip">TIỆN ÍCH HỆ THỐNG</span>
            <h2 className="section-title">Tại sao chọn đặt vé trên CoachBooking?</h2>
            <p className="section-subtitle">
              Hệ thống bán vé trực tuyến chính thống của Bến xe Miền Đông Mới
            </p>
          </div>

          <div className="features-grid">
            <div className="feature-card">
              <div className="feature-icon icon-blue">
                <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                  <path d="M4 15s1-1 4-1 5 2 8 2 4-1 4-1V3s-1 1-4 1-5-2-8-2-4 1-4 1zM4 22v-7" />
                </svg>
              </div>
              <h3>Lịch trình xuất bến minh bạch</h3>
              <p>
                Dữ liệu chuyến xe, tài xế và biển số xe được đồng bộ trực tiếp từ trung tâm điều hành bến xe,
                đảm bảo thông tin giờ chạy luôn chuẩn xác.
              </p>
            </div>

            <div className="feature-card">
              <div className="feature-icon icon-orange">
                <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                  <path d="M7 13h10M7 17h10M5 5h14a2 2 0 0 1 2 2v10a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V7a2 2 0 0 1 2-2z" />
                </svg>
              </div>
              <h3>Sơ đồ ghế thời gian thực</h3>
              <p>
                Kiểm tra ghế trống và chọn chỗ ngồi chính xác trên sơ đồ ghế trước khi thanh toán.
                Ngăn chặn tình trạng trùng ghế nhờ kiểm soát giao dịch ở tầng cơ sở dữ liệu.
              </p>
            </div>

            <div className="feature-card">
              <div className="feature-icon icon-green">
                <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                  <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z" />
                </svg>
              </div>
              <h3>An toàn & Đúng giá niêm yết</h3>
              <p>
                Mọi chuyến xe đều áp dụng giá vé niêm yết theo quy định, không phụ thu trái phép.
                Có hóa đơn điện tử và bảo hiểm hành khách đi kèm.
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* 6. FOOTER */}
      <footer className="home-footer" id="lien-he">
        <div className="footer-container">
          <div className="footer-top-grid">
            <div className="footer-col-brand">
              <div className="footer-brand-header">
                <div className="footer-brand-logo">BX</div>
                <div>
                  <h3 className="footer-brand-title">Bến Xe Miền Đông Mới</h3>
                  <span className="footer-brand-sub">Tổng Công ty Cơ khí Giao thông Vận tải Sài Gòn (SAMCO)</span>
                </div>
              </div>
              <p className="footer-desc">
                Hệ thống bán vé trực tuyến chính thức, phục vụ hành khách kết nối TP. Hồ Chí Minh với các tỉnh Miền Trung, Miền Bắc và Tây Nguyên.
              </p>
            </div>

            <div className="footer-col">
              <h4>Liên kết nhanh</h4>
              <ul className="footer-links">
                <li><a href="#tuyen-duong">Lịch trình các tuyến</a></li>
                <li><a href="#doi-tac">Danh sách nhà xe</a></li>
                <li><Link to="/login">Đăng nhập tài khoản</Link></li>
                <li><Link to="/register">Đăng ký thành viên</Link></li>
              </ul>
            </div>

            <div className="footer-col">
              <h4>Hỗ trợ hành khách</h4>
              <ul className="footer-links">
                <li><a href="#">Quy định hủy đổi vé</a></li>
                <li><a href="#">Chính sách hành lý</a></li>
                <li><a href="#">Hướng dẫn gửi xe tại bến</a></li>
                <li><a href="#">Bảo hiểm chuyến đi</a></li>
              </ul>
            </div>

            <div className="footer-col">
              <h4>Thông tin liên hệ</h4>
              <ul className="footer-contact-list">
                <li>
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z" />
                    <circle cx="12" cy="10" r="3" />
                  </svg>
                  <span>501 Hoàng Hữu Nam, P. Long Bình, TP. Thủ Đức, TP. HCM</span>
                </li>
                <li>
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6 19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 22 16.92z" />
                  </svg>
                  <span>Tổng đài đặt vé: 1900 571 285</span>
                </li>
                <li>
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z" />
                    <polyline points="22,6 12,13 2,6" />
                  </svg>
                  <span>hotro@benxemiendong.vn</span>
                </li>
              </ul>
            </div>
          </div>

          <div className="footer-bottom">
            <p>© 2026 Bến xe Miền Đông Mới. Bản quyền thuộc Tổng Công ty SAMCO.</p>
            <p className="footer-powered">CoachBooking Platform v1.0 · Spring Boot & React</p>
          </div>
        </div>
      </footer>
    </div>
  );
}
