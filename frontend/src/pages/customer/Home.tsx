import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { api, getErrorMessage } from '../../api/client';
import type { ChuyenTimKiem, NhaXe, TuyenDuong } from '../../api/types';
import { useAuth } from '../../auth/AuthContext';
import { formatVnd, todayISO } from '../../utils/format';
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
  const { nguoiDung, logout } = useAuth();
  const navigate = useNavigate();

  // Dữ liệu thực từ database
  const [tuyenList, setTuyenList] = useState<TuyenDuong[]>([]);
  const [nhaXeList, setNhaXeList] = useState<NhaXe[]>([]);
  const [loadingInitial, setLoadingInitial] = useState(true);

  // Form tìm kiếm - khởi tạo mặc định TP HCM -> Đà Lạt
  const [diemDi, setDiemDi] = useState('TP HCM');
  const [diemDen, setDiemDen] = useState('Đà Lạt');
  const [ngay, setNgay] = useState(todayISO());

  // Custom Dropdowns cho Điểm đi & Điểm đến
  const [openDiemDi, setOpenDiemDi] = useState(false);
  const [openDiemDen, setOpenDiemDen] = useState(false);
  const diemDiRef = useRef<HTMLDivElement>(null);
  const diemDenRef = useRef<HTMLDivElement>(null);

  // Kết quả tìm kiếm thực tế
  const [chuyens, setChuyens] = useState<ChuyenTimKiem[]>([]);
  const [searched, setSearched] = useState(false);
  const [loadingChuyen, setLoadingChuyen] = useState(false);
  const [searchError, setSearchError] = useState('');

  // Dropdown menu người dùng
  const [userMenuOpen, setUserMenuOpen] = useState(false);
  const userMenuRef = useRef<HTMLDivElement>(null);

  // Đóng dropdowns khi click bên ngoài
  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (userMenuRef.current && !userMenuRef.current.contains(e.target as Node)) {
        setUserMenuOpen(false);
      }
      if (diemDiRef.current && !diemDiRef.current.contains(e.target as Node)) {
        setOpenDiemDi(false);
      }
      if (diemDenRef.current && !diemDenRef.current.contains(e.target as Node)) {
        setOpenDiemDen(false);
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
      api.get<TuyenDuong[]>('/tuyen-duong', { redirectOn401: false }).catch(() => [] as TuyenDuong[]),
      api.get<NhaXe[]>('/nha-xe', { redirectOn401: false }).catch(() => [] as NhaXe[]),
    ]).then(([tuyenData, nhaXeData]) => {
      if (!isMounted) return;
      const activeTuyen = (tuyenData || []).filter((t) => t.trangThai);
      const activeNhaXe = (nhaXeData || []).filter((nx) => nx.trangThai);

      setTuyenList(activeTuyen);
      setNhaXeList(activeNhaXe);
      setLoadingInitial(false);

      if (activeTuyen.length > 0) {
        // Tự động chọn tuyến đầu tiên hoặc tuyến xuất bến từ TP.HCM / Bến xe Miền Đông
        const preferred = activeTuyen.find((t) =>
          t.diemDi.toLowerCase().includes('tp hcm') ||
          t.diemDi.toLowerCase().includes('miền đông') ||
          t.diemDi.toLowerCase().includes('hồ chí minh'),
        );
        const selected = preferred || activeTuyen[0];
        setDiemDi(selected.diemDi);
        setDiemDen(selected.diemDen);
      }
    });

    return () => {
      isMounted = false;
    };
  }, []);

  // Danh sách các điểm đi duy nhất có trong database (hoặc fallback nếu chưa tải xong)
  const diemDiOptions = useMemo(() => {
    if (tuyenList.length === 0) return DEFAULT_LOCATIONS;
    const set = new Set<string>();
    tuyenList.forEach((t) => set.add(t.diemDi));
    return Array.from(set);
  }, [tuyenList]);

  // Danh sách các điểm đến tương ứng với điểm đi đã chọn
  const diemDenOptions = useMemo(() => {
    if (tuyenList.length === 0) {
      return DEFAULT_LOCATIONS.filter((loc) => loc !== diemDi);
    }
    const filtered = tuyenList.filter((t) => t.diemDi === diemDi).map((t) => t.diemDen);
    if (filtered.length === 0) {
      const allTo = Array.from(new Set(tuyenList.map((t) => t.diemDen))).filter((d) => d !== diemDi);
      return allTo.length > 0 ? allTo : DEFAULT_LOCATIONS.filter((loc) => loc !== diemDi);
    }
    return Array.from(new Set(filtered));
  }, [tuyenList, diemDi]);

  // Cập nhật điểm đến phù hợp khi người dùng chọn điểm đi
  const handleDiemDiChange = (newDiemDi: string) => {
    setDiemDi(newDiemDi);
    if (tuyenList.length > 0) {
      const validDiemDen = tuyenList.filter((t) => t.diemDi === newDiemDi).map((t) => t.diemDen);
      if (validDiemDen.length > 0 && !validDiemDen.includes(diemDen)) {
        setDiemDen(validDiemDen[0]);
      }
    } else if (diemDen === newDiemDi) {
      const fallback = DEFAULT_LOCATIONS.find((loc) => loc !== newDiemDi) || 'Đà Lạt';
      setDiemDen(fallback);
    }
  };

  // Danh sách các tuyến phổ biến thực tế từ database
  const popularRoutesFromDb = useMemo(() => {
    return tuyenList.slice(0, 6);
  }, [tuyenList]);

  const handleSwap = () => {
    const temp = diemDi;
    setDiemDi(diemDen);
    setDiemDen(temp);
  };

  const handleSearch = useCallback(
    async (overrideDiemDi?: string, overrideDiemDen?: string) => {
      const from = overrideDiemDi ?? diemDi;
      const to = overrideDiemDen ?? diemDen;

      if (!from || !to) {
        setSearchError('Vui lòng chọn Điểm đi và Điểm đến để tra cứu chuyến xe.');
        return;
      }

      setLoadingChuyen(true);
      setSearchError('');
      setSearched(true);

      try {
        const queryParams = new URLSearchParams();
        if (from) queryParams.set('diemDi', from);
        if (to) queryParams.set('diemDen', to);
        if (ngay) queryParams.set('ngay', ngay);

        const data = await api.get<ChuyenTimKiem[]>(`/chuyen-xe/tim?${queryParams.toString()}`, {
          redirectOn401: false,
        });
        setChuyens(data || []);
      } catch (err) {
        setSearchError(getErrorMessage(err));
        setChuyens([]);
      } finally {
        setLoadingChuyen(false);
      }
    },
    [diemDi, diemDen, ngay],
  );

  const handleSelectQuickRoute = (route: TuyenDuong) => {
    setDiemDi(route.diemDi);
    setDiemDen(route.diemDen);
    handleSearch(route.diemDi, route.diemDen);
    window.scrollTo({ top: 400, behavior: 'smooth' });
  };

  const handleSelectChuyen = (chuyen: ChuyenTimKiem) => {
    if (nguoiDung) {
      if (nguoiDung.quyen === 'KhachHang') {
        navigate('/app/dat-ve', { state: { selectedChuyen: chuyen } });
      } else {
        navigate('/admin/ban-ve');
      }
    } else {
      navigate('/login');
    }
  };

  return (
    <div className="home-container">
      {/* 1. TOP NAVIGATION BAR */}
      <header className="home-header">
        <div className="home-header-inner">
          <div className="home-brand">
            <Link to="/" className="brand-link">
              <div className="brand-badge-logo">BX</div>
              <div className="brand-text">
                <span className="brand-title">CoachBooking</span>
                <span className="brand-subtitle">Bến xe Miền Đông Mới</span>
              </div>
            </Link>
          </div>

          <nav className="home-nav">
            <Link to="/" className="nav-item active">
              Trang chủ
            </Link>
            <a href="#tuyen-duong" className="nav-item">
              Lịch trình tuyến xe
            </a>
            <a href="#doi-tac" className="nav-item">
              Nhà xe đối tác ({nhaXeList.length})
            </a>
          </nav>

          <div className="home-actions">
            {nguoiDung ? (
              <div className="user-dropdown-wrap" ref={userMenuRef}>
                <button
                  type="button"
                  className={`btn-user-profile ${userMenuOpen ? 'active' : ''}`}
                  onClick={() => setUserMenuOpen((prev) => !prev)}
                  aria-expanded={userMenuOpen}
                  aria-haspopup="true"
                >
                  <span className="user-avatar-badge">
                    {(nguoiDung.hoTen || nguoiDung.tenDangNhap).charAt(0).toUpperCase()}
                  </span>
                  <span className="user-name">{nguoiDung.hoTen || nguoiDung.tenDangNhap}</span>
                  <svg
                    className={`user-chevron ${userMenuOpen ? 'open' : ''}`}
                    width="14"
                    height="14"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    strokeWidth="2.5"
                  >
                    <polyline points="6 9 12 15 18 9" />
                  </svg>
                </button>

                {userMenuOpen && (
                  <div className="user-dropdown-menu">
                    <div className="user-menu-header">
                      <div className="user-menu-avatar">
                        {(nguoiDung.hoTen || nguoiDung.tenDangNhap).charAt(0).toUpperCase()}
                      </div>
                      <div className="user-menu-info">
                        <span className="user-menu-name">{nguoiDung.hoTen || nguoiDung.tenDangNhap}</span>
                        <span className="user-menu-role">
                          {nguoiDung.quyen === 'KhachHang'
                            ? 'Khách hàng thân thiết'
                            : nguoiDung.quyen === 'Admin'
                              ? 'Quản trị viên bến'
                              : 'Nhân viên bán vé'}
                        </span>
                      </div>
                    </div>

                    <div className="user-menu-divider" />

                    {nguoiDung.quyen === 'KhachHang' ? (
                      <>
                        <Link
                          to="/app/ho-so"
                          className="user-menu-item"
                          onClick={() => setUserMenuOpen(false)}
                        >
                          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                            <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
                            <circle cx="12" cy="7" r="4" />
                          </svg>
                          <span>Thông tin</span>
                        </Link>

                        <Link
                          to="/app/lich-su"
                          className="user-menu-item"
                          onClick={() => setUserMenuOpen(false)}
                        >
                          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                            <rect x="3" y="4" width="18" height="16" rx="2" />
                            <path d="M7 8h10M7 12h10M7 16h6" />
                          </svg>
                          <span>Vé đã đặt</span>
                        </Link>
                      </>
                    ) : (
                      <>
                        <Link
                          to="/admin"
                          className="user-menu-item"
                          onClick={() => setUserMenuOpen(false)}
                        >
                          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                            <rect x="3" y="3" width="7" height="7" />
                            <rect x="14" y="3" width="7" height="7" />
                            <rect x="14" y="14" width="7" height="7" />
                            <rect x="3" y="14" width="7" height="7" />
                          </svg>
                          <span>Bàn làm việc quản trị</span>
                        </Link>

                        <Link
                          to="/admin/ho-so"
                          className="user-menu-item"
                          onClick={() => setUserMenuOpen(false)}
                        >
                          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                            <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
                            <circle cx="12" cy="7" r="4" />
                          </svg>
                          <span>Thông tin</span>
                        </Link>
                      </>
                    )}

                    <div className="user-menu-divider" />

                    <button
                      type="button"
                      className="user-menu-item logout"
                      onClick={() => {
                        setUserMenuOpen(false);
                        logout();
                      }}
                    >
                      <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                        <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4M16 17l5-5-5-5M21 12H9" />
                      </svg>
                      <span>Đăng xuất</span>
                    </button>
                  </div>
                )}
              </div>
            ) : (
              <div className="guest-auth-buttons">
                <Link to="/login" className="btn-home-login">
                  Đăng nhập
                </Link>
                <Link to="/register" className="btn-home-register">
                  Đăng ký
                </Link>
              </div>
            )}
          </div>
        </div>
      </header>

      {/* 2. HERO SECTION VỚI DỮ LIỆU THẬT */}
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
            Hơn {tuyenList.length > 0 ? tuyenList.length : '14'} tuyến liên tỉnh phục vụ bởi {nhaXeList.length > 0 ? nhaXeList.length : '5'} đơn vị vận tải uy tín.
            Đặt vé và giữ chỗ trực tuyến an toàn theo giá niêm yết chính thức.
          </p>

          {/* Dải cam kết */}
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
            {/* FORM TÌM KIẾM THEO TUYẾN THỰC TẾ TRONG DATABASE */}
            <form
              className="widget-search-form"
              onSubmit={(e) => {
                e.preventDefault();
                handleSearch();
              }}
            >
              {/* ĐIỂM ĐI - CUSTOM DROPDOWN */}
              <div className={`form-field-group field-from ${openDiemDi ? 'field-open' : ''}`} ref={diemDiRef}>
                <label>ĐIỂM ĐI</label>
                <button
                  type="button"
                  className="custom-select-trigger"
                  onClick={() => {
                    setOpenDiemDi((prev) => !prev);
                    setOpenDiemDen(false);
                  }}
                  aria-expanded={openDiemDi}
                >
                  <div className="trigger-left">
                    <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#1e40af" strokeWidth="2">
                      <circle cx="12" cy="12" r="10" />
                      <circle cx="12" cy="12" r="3" fill="#1e40af" />
                    </svg>
                    <span className="trigger-value">{diemDi}</span>
                  </div>
                  <svg
                    className={`trigger-chevron ${openDiemDi ? 'open' : ''}`}
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
                  {LOCATION_METAS[diemDi]?.desc || 'Điểm khởi hành tại bến'}
                </span>

                <button
                  type="button"
                  className="btn-swap-stations"
                  onClick={() => {
                    handleSwap();
                    setOpenDiemDi(false);
                    setOpenDiemDen(false);
                  }}
                  title="Đảo chiều điểm đi và đến"
                >
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
                    <path d="M7 16V4M7 4L3 8M7 4L11 8M17 8V20M17 20L21 16M17 20L13 16" />
                  </svg>
                </button>

                {openDiemDi && (
                  <div className="custom-dropdown-panel">
                    <div className="dropdown-panel-header">
                      <span>CHỌN ĐIỂM KHỞI HÀNH</span>
                      <span className="dropdown-panel-badge">{diemDiOptions.length} điểm</span>
                    </div>
                    <div className="dropdown-panel-list">
                      {diemDiOptions.map((opt) => {
                        const isSelected = diemDi === opt;
                        const meta = LOCATION_METAS[opt];
                        return (
                          <button
                            key={opt}
                            type="button"
                            className={`dropdown-option-item ${isSelected ? 'active' : ''}`}
                            onClick={() => {
                              handleDiemDiChange(opt);
                              setOpenDiemDi(false);
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

              {/* ĐIỂM ĐẾN - CUSTOM DROPDOWN */}
              <div className={`form-field-group field-to ${openDiemDen ? 'field-open' : ''}`} ref={diemDenRef}>
                <label>ĐIỂM ĐẾN</label>
                <button
                  type="button"
                  className="custom-select-trigger"
                  onClick={() => {
                    setOpenDiemDen((prev) => !prev);
                    setOpenDiemDi(false);
                  }}
                  aria-expanded={openDiemDen}
                >
                  <div className="trigger-left">
                    <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#ea580c" strokeWidth="2">
                      <path d="M12 2a8 8 0 0 0-8 8c0 5.25 8 12 8 12s8-6.75 8-12a8 8 0 0 0-8-8z" />
                      <circle cx="12" cy="10" r="3" />
                    </svg>
                    <span className="trigger-value">{diemDen}</span>
                  </div>
                  <svg
                    className={`trigger-chevron ${openDiemDen ? 'open' : ''}`}
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
                  {LOCATION_METAS[diemDen]?.desc || 'Bến xe đến / Điểm trả'}
                </span>

                {openDiemDen && (
                  <div className="custom-dropdown-panel">
                    <div className="dropdown-panel-header">
                      <span>CHỌN ĐIỂM ĐẾN</span>
                      <span className="dropdown-panel-badge">{diemDenOptions.length} tuyến</span>
                    </div>
                    <div className="dropdown-panel-list">
                      {diemDenOptions.map((opt) => {
                        const isSelected = diemDen === opt;
                        const meta = LOCATION_METAS[opt];
                        return (
                          <button
                            key={opt}
                            type="button"
                            className={`dropdown-option-item ${isSelected ? 'active' : ''}`}
                            onClick={() => {
                              setDiemDen(opt);
                              setOpenDiemDen(false);
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

              <div className="form-field-group field-date">
                <div className="field-label-row">
                  <label>NGÀY ĐI</label>
                  <div className="quick-date-links">
                    <button type="button" onClick={() => setNgay(todayISO())}>
                      Hôm nay
                    </button>
                    <span>•</span>
                    <button
                      type="button"
                      onClick={() => {
                        const d = new Date();
                        d.setDate(d.getDate() + 1);
                        setNgay(d.toISOString().slice(0, 10));
                      }}
                    >
                      Ngày mai
                    </button>
                  </div>
                </div>

                <div className="date-picker-wrap">
                  <div className="trigger-left">
                    <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#1e40af" strokeWidth="2">
                      <rect x="3" y="4" width="18" height="18" rx="2" ry="2" />
                      <line x1="16" y1="2" x2="16" y2="6" />
                      <line x1="8" y1="2" x2="8" y2="6" />
                      <line x1="3" y1="10" x2="21" y2="10" />
                    </svg>
                    <span className="trigger-value">{formatVietnameseDate(ngay).formatted}</span>
                  </div>
                  <input
                    type="date"
                    className="hidden-date-input"
                    value={ngay}
                    min={todayISO()}
                    onChange={(e) => setNgay(e.target.value)}
                    required
                    title="Chọn ngày khởi hành"
                  />
                </div>
                <span className="field-subtext">{formatVietnameseDate(ngay).weekday}</span>
              </div>

              <div className="form-action-group">
                <button type="submit" className="btn-search-hero" disabled={loadingChuyen || loadingInitial}>
                  {loadingChuyen ? (
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
                  {popularRoutesFromDb.map((route) => (
                    <button
                      key={route.maTuyen}
                      type="button"
                      className="suggestion-pill"
                      onClick={() => handleSelectQuickRoute(route)}
                    >
                      <span>{route.diemDi} ➔ {route.diemDen}</span>
                      {route.quangDuong && (
                        <span className="pill-distance">({route.quangDuong} km)</span>
                      )}
                    </button>
                  ))}
                </div>
              </div>
            )}
          </div>
        </div>
      </section>

      {/* 3. KẾT QUẢ TÌM KIẾM CHUYẾN XE (KẾT NỐI API /api/chuyen-xe/tim THẬT) */}
      {searched && (
        <section className="search-results-section" id="ket-qua">
          <div className="results-container">
            <div className="results-header">
              <div>
                <h2>
                  Kết quả chuyến xe: <span className="text-primary">{diemDi} ➔ {diemDen}</span>
                </h2>
                <p className="results-sub">
                  Ngày khởi hành: <strong>{ngay}</strong> · Tìm thấy <strong>{chuyens.length}</strong> chuyến phù hợp trong hệ thống
                </p>
              </div>
              <button
                type="button"
                className="btn-clear-search"
                onClick={() => {
                  setSearched(false);
                  setChuyens([]);
                }}
              >
                Đóng kết quả
              </button>
            </div>

            {searchError && <div className="search-error-alert">{searchError}</div>}

            {loadingChuyen ? (
              <div className="results-loading">
                <span className="spinner-large" />
                <p>Đang tra cứu dữ liệu chuyến xe từ hệ thống bến...</p>
              </div>
            ) : chuyens.length === 0 ? (
              <div className="no-trips-card">
                <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="#94a3b8" strokeWidth="1.5">
                  <circle cx="12" cy="12" r="10" />
                  <line x1="8" y1="12" x2="16" y2="12" />
                </svg>
                <h3>Chưa có chuyến xe nào xuất bến vào ngày {ngay}</h3>
                <p>Hiện chưa có chuyến xe nào chạy tuyến này vào ngày đã chọn. Vui lòng chọn ngày khác hoặc thử tuyến đường khác.</p>
              </div>
            ) : (
              <div className="trip-cards-grid">
                {chuyens.map((chuyen) => {
                  const conTrong = chuyen.soCho - (chuyen.soGheDaDat ?? chuyen.soVeDaBan ?? 0);
                  return (
                    <div key={chuyen.maChuyen} className="trip-card">
                      <div className="trip-card-main">
                        <div className="operator-info">
                          <div className="operator-badge">
                            {chuyen.tenNhaXe ? chuyen.tenNhaXe.charAt(0).toUpperCase() : 'BX'}
                          </div>
                          <div>
                            <strong className="operator-name">{chuyen.tenNhaXe}</strong>
                            <span className="vehicle-type-label">
                              {chuyen.tenLoaiXe} ({chuyen.soCho} chỗ) · BKS: {chuyen.bienSoXe} · Tài xế: {chuyen.tenTx}
                            </span>
                          </div>
                        </div>

                        <div className="trip-timeline">
                          <div className="timeline-node">
                            <span className="node-time">
                              {new Date(chuyen.ngayGioKhoiHanh).toLocaleTimeString('vi-VN', {
                                hour: '2-digit',
                                minute: '2-digit',
                              })}
                            </span>
                            <span className="node-place">{chuyen.diemDi}</span>
                          </div>
                          <div className="timeline-divider">
                            <span className="divider-line" />
                            <span className="bus-icon-node">🚌</span>
                          </div>
                          <div className="timeline-node">
                            <span className="node-time">
                              {chuyen.ngayGioDenDuKien
                                ? new Date(chuyen.ngayGioDenDuKien).toLocaleTimeString('vi-VN', {
                                    hour: '2-digit',
                                    minute: '2-digit',
                                  })
                                : '--:--'}
                            </span>
                            <span className="node-place">{chuyen.diemDen}</span>
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
                          <strong className="price-val">{formatVnd(chuyen.giaVeThucTe)}</strong>
                        </div>
                        <span className={`seats-left-badge ${conTrong <= 5 ? 'seats-urgent' : ''}`}>
                          Còn {conTrong} / {chuyen.soCho} chỗ trống
                        </span>
                        <button
                          type="button"
                          className="btn-select-trip"
                          onClick={() => handleSelectChuyen(chuyen)}
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

      {/* 4. CÁC ĐƠN VỊ VẬN TẢI ĐỐI TÁC THỰC TẾ TRONG DATABASE */}
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
            {nhaXeList.map((op) => (
              <div key={op.maNx} className="operator-card">
                <div className="operator-card-top">
                  <div className="operator-avatar" style={{ backgroundColor: '#1e40af20', color: '#1e40af' }}>
                    {op.tenNx.charAt(0)}
                  </div>
                  <div className="operator-badge-active">
                    <span>Đang hoạt động</span>
                  </div>
                </div>
                <h4 className="op-name">{op.tenNx}</h4>
                <p className="op-type">
                  {op.sdt ? `Hotline: ${op.sdt}` : 'Liên hệ bến xe'}
                </p>
                <div className="operator-card-bottom">
                  <span>{op.email ? op.email : 'contact@benxemiendong.vn'}</span>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* 5. TẠI SAO CHỌN COACHBOOKING SAMCO */}
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
                <div className="brand-badge-logo">BX</div>
                <strong className="footer-brand-name">CoachBooking</strong>
              </div>
              <p className="footer-brand-desc">
                Hệ thống đặt vé xe khách trực tuyến Bến xe Miền Đông Mới.
                Đơn vị chủ quản: Tổng công ty Cơ khí Giao thông Vận tải Sài Gòn (SAMCO).
              </p>
              <div className="footer-contact-info">
                <p>📍 Địa chỉ: 501 Hoàng Hữu Nam, P. Long Bình, TP. Thủ Đức, TP.HCM</p>
                <p>📞 Hotline: 1900 8888 · Email: hotro@coachbooking.vn</p>
              </div>
            </div>

            <div className="footer-col-links">
              <h4>Về Bến xe Miền Đông</h4>
              <ul>
                <li><a href="#tuyen-duong">Lịch trình các tuyến xe ({tuyenList.length})</a></li>
                <li><a href="#doi-tac">Nhà xe đang hoạt động ({nhaXeList.length})</a></li>
                <li><a href="tel:19008888">Hotline điều hành 24/7</a></li>
              </ul>
            </div>

            <div className="footer-col-links">
              <h4>Hỗ trợ hành khách</h4>
              <ul>
                <li><Link to="/app/lich-su">Tra cứu vé đã đặt</Link></li>
                <li><Link to="/login">Đăng nhập tài khoản</Link></li>
                <li><Link to="/register">Đăng ký thành viên</Link></li>
              </ul>
            </div>

            <div className="footer-col-links">
              <h4>Phương thức thanh toán</h4>
              <p className="footer-subtext">Hỗ trợ thanh toán bảo mật bằng Tiền mặt tại quầy, Chuyển khoản ngân hàng hoặc Thẻ ATM</p>
              <div className="payment-badges-row">
                <span className="pay-badge">Chuyển khoản</span>
                <span className="pay-badge">Thẻ ATM</span>
                <span className="pay-badge">Tiền mặt</span>
              </div>
            </div>
          </div>

          <div className="footer-bottom-bar">
            <span>© 2026 CoachBooking · Bến xe Miền Đông.</span>
            <div className="footer-legal-links">
              <a href="#dieu-khoan">Điều khoản sử dụng</a>
              <span>·</span>
              <a href="#bao-mat">Bảo mật</a>
              <span>·</span>
              <a href="#quy-che">Quy chế hoạt động bến</a>
            </div>
          </div>
        </div>
      </footer>
    </div>
  );
}
