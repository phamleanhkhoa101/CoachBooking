import { useEffect, useMemo, useRef, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { api } from '../../api/client';
import type { BusCompany, Route } from '../../api/types';
import { todayISO } from '../../utils/format';
import { BrandLogo } from '../../components/common/BrandLogo';
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

const DESTINATION_BACKDROPS: Array<{ keywords: string[]; image: string }> = [
  { keywords: ['đà lạt', 'da lat', 'lâm đồng', 'buôn ma thuột', 'pleiku'], image: '/images/dest-dalat.jpg' },
  { keywords: ['nha trang', 'vũng tàu', 'phan thiết', 'mũi né', 'cam ranh'], image: '/images/dest-nhatrang.jpg' },
  { keywords: ['đà nẵng', 'da nang', 'huế', 'hội an', 'quảng ngãi'], image: '/images/dest-danang.jpg' },
  { keywords: ['quy nhơn', 'quy nhon', 'phú yên', 'tuy hòa', 'bình định'], image: '/images/dest-quynhon.jpg' },
];

const FALLBACK_BACKDROPS = [
  '/images/dest-dalat.jpg',
  '/images/dest-nhatrang.jpg',
  '/images/dest-danang.jpg',
  '/images/dest-quynhon.jpg',
];

function getDestinationImage(destination: string, index: number): string {
  const normalized = destination.toLowerCase();
  const match = DESTINATION_BACKDROPS.find((entry) =>
    entry.keywords.some((kw) => normalized.includes(kw)),
  );
  return match?.image ?? FALLBACK_BACKDROPS[index % FALLBACK_BACKDROPS.length];
}

function formatVietnameseDateShort(isoDate: string): string {
  if (!isoDate) return 'Chọn ngày';
  try {
    const [y, m, d] = isoDate.split('-').map(Number);
    const date = new Date(y, m - 1, d);
    const weekdays = ['Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'];
    const dayName = weekdays[date.getDay()] ?? '';
    return `${dayName}, ${String(d).padStart(2, '0')}/${String(m).padStart(2, '0')}/${y}`;
  } catch {
    return isoDate;
  }
}

function formatDurationLabel(duration?: string | null): string {
  if (!duration) return '';
  const parts = duration.split(':').map(Number);
  if (parts.length < 2 || Number.isNaN(parts[0])) return '';
  const hours = parts[0];
  const mins = parts[1] || 0;
  return mins > 0 ? `≈ ${hours} giờ ${mins}p` : `≈ ${hours} giờ`;
}

export function Home() {
  const navigate = useNavigate();

  const [routes, setRoutes] = useState<Route[]>([]);
  const [busCompanies, setBusCompanies] = useState<BusCompany[]>([]);
  const [loadingInitial, setLoadingInitial] = useState(true);

  const [departure, setDeparture] = useState('TP HCM');
  const [destination, setDestination] = useState('Đà Lạt');
  const [travelDate, setTravelDate] = useState(todayISO());

  const [openDropdown, setOpenDropdown] = useState<'departure' | 'destination' | null>(null);
  const [searchError, setSearchError] = useState('');

  const searchBarRef = useRef<HTMLFormElement>(null);
  const dateInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (searchBarRef.current && !searchBarRef.current.contains(event.target as Node)) {
        setOpenDropdown(null);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  useEffect(() => {
    let mounted = true;
    Promise.all([
      api.get<Route[]>('/routes', { redirectOn401: false }).catch(() => [] as Route[]),
      api.get<BusCompany[]>('/bus-companies', { redirectOn401: false }).catch(() => [] as BusCompany[]),
    ]).then(([routesData, companiesData]) => {
      if (!mounted) return;
      const activeRoutes = (routesData || []).filter((r) => (r.isActive ?? r.trangThai) !== false);
      const activeCompanies = (companiesData || []).filter((c) => (c.isActive ?? c.trangThai) !== false);

      setRoutes(activeRoutes);
      setBusCompanies(activeCompanies);
      setLoadingInitial(false);

      if (activeRoutes.length > 0) {
        const preferred = activeRoutes.find((r) => {
          const dep = (r.departure ?? r.diemDi ?? '').toLowerCase();
          return dep.includes('tp hcm') || dep.includes('hồ chí minh') || dep.includes('miền đông');
        });
        const chosen = preferred ?? activeRoutes[0];
        setDeparture(chosen.departure ?? chosen.diemDi ?? 'TP HCM');
        setDestination(chosen.destination ?? chosen.diemDen ?? 'Đà Lạt');
      }
    });

    return () => {
      mounted = false;
    };
  }, []);

  const departureOptions = useMemo(() => {
    if (routes.length === 0) return DEFAULT_LOCATIONS;
    const unique = new Set<string>();
    routes.forEach((r) => {
      const dep = r.departure ?? r.diemDi;
      if (dep) unique.add(dep);
    });
    return Array.from(unique);
  }, [routes]);

  const destinationOptions = useMemo(() => {
    if (routes.length === 0) {
      return DEFAULT_LOCATIONS.filter((loc) => loc !== departure);
    }
    const matching = routes
      .filter((r) => (r.departure ?? r.diemDi) === departure)
      .map((r) => r.destination ?? r.diemDen)
      .filter(Boolean) as string[];
    if (matching.length > 0) return Array.from(new Set(matching));

    const fallback = Array.from(
      new Set(routes.map((r) => r.destination ?? r.diemDen).filter(Boolean)),
    ).filter((loc) => loc !== departure) as string[];
    return fallback.length > 0 ? fallback : DEFAULT_LOCATIONS.filter((loc) => loc !== departure);
  }, [routes, departure]);

  const handleDepartureSelect = (newDep: string) => {
    setDeparture(newDep);
    setOpenDropdown(null);
    if (routes.length > 0) {
      const validDestinations = routes
        .filter((r) => (r.departure ?? r.diemDi) === newDep)
        .map((r) => r.destination ?? r.diemDen)
        .filter(Boolean) as string[];
      if (validDestinations.length > 0 && !validDestinations.includes(destination)) {
        setDestination(validDestinations[0]);
      }
    } else if (destination === newDep) {
      setDestination(DEFAULT_LOCATIONS.find((loc) => loc !== newDep) ?? 'Đà Lạt');
    }
  };

  const handleSwap = () => {
    setDeparture(destination);
    setDestination(departure);
    setOpenDropdown(null);
  };

  const goToSearch = (from: string, to: string) => {
    if (!from || !to) {
      setSearchError('Vui lòng chọn điểm đi và điểm đến để tìm chuyến.');
      return;
    }
    setSearchError('');
    const qs = new URLSearchParams({
      departure: from,
      destination: to,
      date: travelDate,
    });
    navigate(`/tim-chuyen?${qs.toString()}`);
  };

  const popularRoutes = useMemo(() => routes.slice(0, 4), [routes]);

  return (
    <div className="nw-home">
      <CustomerNavbar variant="transparent" />

      {/* ==================== 1. CINEMATIC HERO — THE NIGHT WINDOW ==================== */}
      <section className="nw-hero" aria-label="Tìm kiếm chuyến xe">
        <div className="nw-hero-bg" aria-hidden="true" />
        <div className="nw-hero-scrim" aria-hidden="true" />

        <div className="nw-hero-inner">
          <div className="nw-hero-copy">
            <h1 className="nw-hero-title">
              <span className="nw-hero-serif">Đi xa một chút.</span>
              <span className="nw-hero-sans">
                GẦN NHAU HƠN.
                <svg
                  className="nw-amber-brush"
                  viewBox="0 0 220 14"
                  fill="none"
                  aria-hidden="true"
                >
                  <path
                    d="M3 10.5C58 3.2 146 2.2 217 8.4"
                    stroke="#F2A34B"
                    strokeWidth="4.2"
                    strokeLinecap="round"
                  />
                </svg>
              </span>
            </h1>
            <p className="nw-hero-subtitle">
              Đặt vé xe khách dễ dàng, an toàn và nhanh chóng đến mọi miền đất nước từ Bến xe Miền Đông.
            </p>
          </div>

          {/* FLOATING DARK-GLASS SEARCH BAR */}
          <form
            ref={searchBarRef}
            className="nw-search-bar"
            onSubmit={(e) => {
              e.preventDefault();
              goToSearch(departure, destination);
            }}
          >
            {/* Nơi đi */}
            <div className="nw-search-field">
              <button
                type="button"
                className="nw-field-trigger"
                onClick={() => setOpenDropdown(openDropdown === 'departure' ? null : 'departure')}
                aria-expanded={openDropdown === 'departure'}
              >
                <span className="nw-field-icon" aria-hidden="true">
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.9">
                    <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z" />
                    <circle cx="12" cy="10" r="3" />
                  </svg>
                </span>
                <span className="nw-field-text">
                  <span className="nw-field-label">Nơi đi</span>
                  <span className="nw-field-value">{departure || 'Chọn điểm đi'}</span>
                </span>
              </button>

              {openDropdown === 'departure' && (
                <div className="nw-dropdown" role="listbox" aria-label="Chọn điểm đi">
                  {departureOptions.map((loc) => (
                    <button
                      key={loc}
                      type="button"
                      role="option"
                      aria-selected={loc === departure}
                      className={`nw-dropdown-item ${loc === departure ? 'selected' : ''}`}
                      onClick={() => handleDepartureSelect(loc)}
                    >
                      {loc}
                    </button>
                  ))}
                </div>
              )}
            </div>

            {/* Swap */}
            <button
              type="button"
              className="nw-swap-btn"
              onClick={handleSwap}
              aria-label="Đảo ngược nơi đi và nơi đến"
              title="Đổi chiều"
            >
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.2">
                <path d="M7 16V4M7 4L3 8M7 4l4 4M17 8v12m0 0l4-4m-4 4l-4-4" />
              </svg>
            </button>

            {/* Nơi đến */}
            <div className="nw-search-field">
              <button
                type="button"
                className="nw-field-trigger"
                onClick={() => setOpenDropdown(openDropdown === 'destination' ? null : 'destination')}
                aria-expanded={openDropdown === 'destination'}
              >
                <span className="nw-field-icon" aria-hidden="true">
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.9">
                    <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z" />
                    <circle cx="12" cy="10" r="3" />
                  </svg>
                </span>
                <span className="nw-field-text">
                  <span className="nw-field-label">Nơi đến</span>
                  <span className="nw-field-value">{destination || 'Chọn điểm đến'}</span>
                </span>
              </button>

              {openDropdown === 'destination' && (
                <div className="nw-dropdown" role="listbox" aria-label="Chọn điểm đến">
                  {destinationOptions.map((loc) => (
                    <button
                      key={loc}
                      type="button"
                      role="option"
                      aria-selected={loc === destination}
                      className={`nw-dropdown-item ${loc === destination ? 'selected' : ''}`}
                      onClick={() => {
                        setDestination(loc);
                        setOpenDropdown(null);
                      }}
                    >
                      {loc}
                    </button>
                  ))}
                </div>
              )}
            </div>

            <div className="nw-search-divider" aria-hidden="true" />

            {/* Ngày đi */}
            <div
              className="nw-search-field nw-search-field--date"
              onClick={() => dateInputRef.current?.showPicker?.()}
            >
              <label className="nw-field-trigger" htmlFor="nw-home-date">
                <span className="nw-field-icon" aria-hidden="true">
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.9">
                    <rect x="3" y="4" width="18" height="18" rx="2" />
                    <line x1="16" y1="2" x2="16" y2="6" />
                    <line x1="8" y1="2" x2="8" y2="6" />
                    <line x1="3" y1="10" x2="21" y2="10" />
                  </svg>
                </span>
                <span className="nw-field-text">
                  <span className="nw-field-label">Ngày đi</span>
                  <span className="nw-field-value tabular-nums">
                    {formatVietnameseDateShort(travelDate)}
                  </span>
                </span>
              </label>
              <input
                ref={dateInputRef}
                id="nw-home-date"
                type="date"
                className="nw-date-native"
                min={todayISO()}
                value={travelDate}
                onChange={(e) => setTravelDate(e.target.value)}
              />
            </div>

            {/* CTA */}
            <button type="submit" className="nw-search-submit">
              <svg width="17" height="17" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.4" aria-hidden="true">
                <circle cx="11" cy="11" r="7" />
                <line x1="21" y1="21" x2="16.65" y2="16.65" />
              </svg>
              <span>Tìm chuyến</span>
            </button>
          </form>

          {searchError && (
            <p className="nw-search-error" role="alert">
              {searchError}
            </p>
          )}

          {/* 4-COLUMN VALUE PROPOSITION STRIP */}
          <div className="nw-usp-strip" aria-label="Ưu điểm dịch vụ">
            <div className="nw-usp-item">
              <span className="nw-usp-icon" aria-hidden="true">
                <svg width="19" height="19" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                  <polygon points="13 2 3 14 12 14 11 22 21 10 12 10 13 2" />
                </svg>
              </span>
              <div className="nw-usp-text">
                <strong>Đặt vé tức thì</strong>
                <span className="nw-usp-desc">Giữ chỗ ngay, xác nhận liền</span>
              </div>
            </div>

            <div className="nw-usp-item">
              <span className="nw-usp-icon" aria-hidden="true">
                <svg width="19" height="19" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                  <rect x="3" y="6" width="18" height="11" rx="2" />
                  <path d="M3 12h18M7 17v2M17 17v2" />
                </svg>
              </span>
              <div className="nw-usp-text">
                <strong>Đa dạng nhà xe</strong>
                <span className="nw-usp-desc">Nhiều lựa chọn, nhiều khung giờ</span>
              </div>
            </div>

            <div className="nw-usp-item">
              <span className="nw-usp-icon" aria-hidden="true">
                <svg width="19" height="19" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                  <path d="M4 7h16v3a2 2 0 0 0 0 4v3H4v-3a2 2 0 0 0 0-4zM12 7v10" />
                </svg>
              </span>
              <div className="nw-usp-text">
                <strong>Vé điện tử</strong>
                <span className="nw-usp-desc">Không cần in vé, tra cứu tức thì</span>
              </div>
            </div>

            <div className="nw-usp-item">
              <span className="nw-usp-icon" aria-hidden="true">
                <svg width="19" height="19" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                  <path d="M3 18v-6a9 9 0 0 1 18 0v6" />
                  <path d="M21 19a2 2 0 0 1-2 2h-1a2 2 0 0 1-2-2v-3a2 2 0 0 1 2-2h3zM3 19a2 2 0 0 0 2 2h1a2 2 0 0 0 2-2v-3a2 2 0 0 0-2-2H3z" />
                </svg>
              </span>
              <div className="nw-usp-text">
                <strong>Hỗ trợ 24/7</strong>
                <span className="nw-usp-desc">Luôn đồng hành cùng bạn</span>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* ==================== 2. POPULAR ROUTES SHOWCASE (REAL DB DATA) ==================== */}
      <section id="tuyen-pho-bien" className="nw-routes-section">
        <div className="nw-container">
          <div className="nw-section-head">
            <h2>Khám phá các tuyến phổ biến</h2>
            <Link to="/tim-chuyen" className="nw-section-link">
              Xem tất cả tuyến
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" aria-hidden="true">
                <line x1="5" y1="12" x2="19" y2="12" />
                <polyline points="12 5 19 12 12 19" />
              </svg>
            </Link>
          </div>

          {loadingInitial ? (
            <div className="nw-routes-grid" aria-busy="true">
              {[0, 1, 2, 3].map((i) => (
                <div key={i} className="nw-route-card nw-route-card--skeleton" />
              ))}
            </div>
          ) : popularRoutes.length === 0 ? (
            <div className="nw-routes-empty">
              <p>Hiện chưa có tuyến đường nào khả dụng. Bạn có thể tìm chuyến trực tiếp ở thanh tìm kiếm.</p>
              <Link to="/tim-chuyen" className="nw-search-submit">
                Mở trang tìm chuyến
              </Link>
            </div>
          ) : (
            <div className="nw-routes-grid">
              {popularRoutes.map((route, idx) => {
                const dep = route.departure ?? route.diemDi ?? 'TP HCM';
                const dest = route.destination ?? route.diemDen ?? 'Điểm đến';
                const distance = route.distance ?? route.quangDuong;
                const duration = formatDurationLabel(route.estimatedDuration ?? route.thoiGianDuKien);
                const img = getDestinationImage(dest, idx);

                return (
                  <button
                    key={route.routeId ?? route.maTuyen ?? `${dep}-${dest}`}
                    type="button"
                    className="nw-route-card"
                    onClick={() => goToSearch(dep, dest)}
                  >
                    <img src={img} alt={`${dep} đến ${dest}`} className="nw-route-card-img" loading="lazy" />
                    <div className="nw-route-card-scrim" aria-hidden="true" />

                    <div className="nw-route-card-top">
                      <span className="nw-route-origin">
                        <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.2" aria-hidden="true">
                          <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z" />
                          <circle cx="12" cy="10" r="3" />
                        </svg>
                        {dep} →
                      </span>
                      <h3 className="nw-route-dest">{dest}</h3>
                    </div>

                    <div className="nw-route-card-bottom">
                      <div className="nw-route-meta tabular-nums">
                        {distance ? <span className="nw-route-distance">{distance} km</span> : null}
                        {duration ? (
                          <span className="nw-route-duration">
                            <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" aria-hidden="true">
                              <circle cx="12" cy="12" r="9" />
                              <polyline points="12 7 12 12 15 14" />
                            </svg>
                            {duration}
                          </span>
                        ) : null}
                      </div>
                      <span className="nw-route-arrow" aria-hidden="true">
                        <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.2">
                          <line x1="5" y1="12" x2="19" y2="12" />
                          <polyline points="12 5 19 12 12 19" />
                        </svg>
                      </span>
                    </div>
                  </button>
                );
              })}
            </div>
          )}
        </div>
      </section>

      {/* ==================== 3. STORYTELLING JOURNEY TIMELINE ==================== */}
      <section className="nw-journey-section" aria-label="Quy trình đặt vé và trải nghiệm">
        <div className="nw-journey-horizon" aria-hidden="true">
          <svg viewBox="0 0 1440 160" preserveAspectRatio="none">
            <path
              d="M0 130 C240 90 420 50 680 78 C920 104 1140 52 1440 96"
              stroke="url(#nwHorizonGlow)"
              strokeWidth="1.6"
              fill="none"
            />
            <defs>
              <linearGradient id="nwHorizonGlow" x1="0" y1="0" x2="1440" y2="0" gradientUnits="userSpaceOnUse">
                <stop offset="0%" stopColor="rgba(242,163,75,0)" />
                <stop offset="30%" stopColor="rgba(242,163,75,0.65)" />
                <stop offset="75%" stopColor="rgba(242,163,75,0.85)" />
                <stop offset="100%" stopColor="rgba(242,163,75,0)" />
              </linearGradient>
            </defs>
          </svg>
        </div>

        <div className="nw-container nw-journey-inner">
          <div className="nw-journey-headline">
            <h2>
              Một hành trình,
              <br />
              nhiều hơn là di chuyển.
            </h2>
            <svg className="nw-amber-brush nw-amber-brush--sm" viewBox="0 0 180 12" fill="none" aria-hidden="true">
              <path d="M3 9C48 2.8 118 2.4 177 7.6" stroke="#F2A34B" strokeWidth="3.5" strokeLinecap="round" />
            </svg>
          </div>

          <ol className="nw-journey-steps">
            <li>
              <span className="nw-step-icon" aria-hidden="true">
                <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.9" strokeLinecap="round" strokeLinejoin="round">
                  <path d="M4 7h16v3a2 2 0 0 0 0 4v3H4v-3a2 2 0 0 0 0-4zM12 7v10" />
                </svg>
              </span>
              <strong>Đặt vé</strong>
              <span className="nw-step-desc">Chọn tuyến, giữ chỗ dễ dàng</span>
            </li>
            <li>
              <span className="nw-step-icon" aria-hidden="true">
                <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.9" strokeLinecap="round" strokeLinejoin="round">
                  <rect x="3" y="6" width="18" height="11" rx="2" />
                  <path d="M3 12h18M7 17v2M17 17v2" />
                </svg>
              </span>
              <strong>Lên xe</strong>
              <span className="nw-step-desc">Đón xe thuận tiện, đúng giờ</span>
            </li>
            <li>
              <span className="nw-step-icon" aria-hidden="true">
                <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.9" strokeLinecap="round" strokeLinejoin="round">
                  <path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z" />
                </svg>
              </span>
              <strong>Thư giãn</strong>
              <span className="nw-step-desc">Không gian thoải mái, nghỉ ngơi trọn vẹn</span>
            </li>
            <li>
              <span className="nw-step-icon" aria-hidden="true">
                <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.9" strokeLinecap="round" strokeLinejoin="round">
                  <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z" />
                  <circle cx="12" cy="10" r="3" />
                </svg>
              </span>
              <strong>Đến nơi</strong>
              <span className="nw-step-desc">An toàn, đúng lịch trình</span>
            </li>
          </ol>
        </div>

        {busCompanies.length > 0 && (
          <div className="nw-container nw-partners-bar">
            <span className="nw-partners-label">Nhà xe đối tác tại Bến xe Miền Đông</span>
            <div className="nw-partners-list">
              {busCompanies.slice(0, 6).map((company) => (
                <span key={company.companyId ?? company.maNx} className="nw-partner-chip">
                  {company.companyName ?? company.tenNx}
                </span>
              ))}
            </div>
          </div>
        )}
      </section>

      {/* ==================== 4. FOOTER ==================== */}
      <footer className="nw-footer">
        <div className="nw-container nw-footer-inner">
          <div className="nw-footer-brand">
            <BrandLogo size={36} />
            <div>
              <strong>CoachBooking</strong>
              <span>501 Hoàng Hữu Nam, TP. Thủ Đức · Hotline 1900 6067</span>
            </div>
          </div>
          <div className="nw-footer-links">
            <Link to="/tim-chuyen">Tìm chuyến xe</Link>
            <Link to="/tra-cuu-ve">Tra cứu vé</Link>
            <Link to="/ve-cua-toi">Vé của tôi</Link>
            <Link to="/login">Đăng nhập</Link>
          </div>
          <p className="nw-footer-copy">© 2026 CoachBooking · Bến xe Miền Đông</p>
        </div>
      </footer>
    </div>
  );
}
