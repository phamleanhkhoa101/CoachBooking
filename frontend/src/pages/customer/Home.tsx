import { useCallback, useEffect, useMemo, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { api, getErrorMessage } from '../../api/client';
import type { ChuyenTimKiem, TuyenDuong } from '../../api/types';
import { useAuth } from '../../auth/AuthContext';
import { formatVnd, todayISO } from '../../utils/format';
import './Home.css';

interface QuickRoute {
  diemDi: string;
  diemDen: string;
  giaTu: number;
}

const POPULAR_ROUTES: QuickRoute[] = [
  { diemDi: 'Bến xe Miền Đông', diemDen: 'Đà Lạt', giaTu: 240000 },
  { diemDi: 'Bến xe Miền Đông', diemDen: 'Nha Trang', giaTu: 260000 },
  { diemDi: 'Bến xe Miền Đông', diemDen: 'Vũng Tàu', giaTu: 130000 },
  { diemDi: 'Bến xe Miền Đông', diemDen: 'Buôn Ma Thuột', giaTu: 280000 },
  { diemDi: 'Bến xe Miền Đông', diemDen: 'Quy Nhơn', giaTu: 320000 },
  { diemDi: 'Bến xe Miền Đông', diemDen: 'Phan Thiết', giaTu: 180000 },
];

const PROMOTIONS = [
  {
    id: 'NEWPASS50',
    tag: 'Khách Mới',
    title: 'Ưu đãi bạn mới - Giảm 50.000đ',
    desc: 'Áp dụng cho mọi chuyến xe xuất phát từ Bến xe Miền Đông Mới khi thanh toán lần đầu.',
    code: 'NEWPASS50',
    expiry: 'HSD: 31/12/2026',
    pillClass: 'promo-badge-orange',
  },
  {
    id: 'ROUNDTRIP15',
    tag: 'Tiết Kiệm 15%',
    title: 'Đặt vé khứ hồi - Giảm 15%',
    desc: 'Giảm trực tiếp 15% tổng hóa đơn chuyến xe khứ hồi khi đặt trước vé xuất bến 2 chiều.',
    code: 'ROUNDTRIP15',
    expiry: 'HSD: Vô thời hạn',
    pillClass: 'promo-badge-blue',
  },
  {
    id: 'VIETQR5',
    tag: 'Hoàn Tiền',
    title: 'Thanh toán VietQR - Hoàn 5%',
    desc: 'Hoàn tiền tức thì 5% vào ví khi quét mã QR Napas 24/7 trực tiếp trên ứng dụng.',
    code: 'VIETQR5',
    expiry: 'Mỗi ngày',
    pillClass: 'promo-badge-green',
  },
];

const OPERATORS = [
  {
    initials: 'PT',
    color: '#ff6600',
    name: 'Phương Trang (FUTA)',
    type: 'Limousine 34 Phòng, Giường nằm',
    rating: '4.9',
    trips: '180 chuyến/ngày',
    dest: 'Đi Miền Tây, Đà Lạt',
  },
  {
    initials: 'SE',
    color: '#1e40af',
    name: 'Samco Express',
    type: 'Chuyên tuyến cao tốc Phan Thiết',
    rating: '4.8',
    trips: '65 chuyến/ngày',
    dest: 'Tuyến Đông Nam Bộ',
  },
  {
    initials: 'HM',
    color: '#0d9488',
    name: 'Hoa Mai Limousine',
    type: 'VIP 9 chỗ, trung chuyển tận nơi',
    rating: '4.9',
    trips: '90 chuyến/ngày',
    dest: 'Sài Gòn - Vũng Tàu',
  },
  {
    initials: 'KS',
    color: '#9333ea',
    name: 'Kumho Samco',
    type: 'Hạng thương gia cao cấp',
    rating: '4.7',
    trips: '75 chuyến/ngày',
    dest: 'Tây Nguyên & Miền Trung',
  },
];

export function Home() {
  const { nguoiDung, logout } = useAuth();
  const navigate = useNavigate();

  // Danh mục tuyến & tìm kiếm
  const [tuyenList, setTuyenList] = useState<TuyenDuong[]>([]);
  const [diemDi, setDiemDi] = useState('Bến xe Miền Đông');
  const [diemDen, setDiemDen] = useState('Đà Lạt');
  const [ngay, setNgay] = useState(todayISO());
  const [tripType, setTripType] = useState<'oneway' | 'roundtrip'>('oneway');
  const [categoryTab, setCategoryTab] = useState<'intercity' | 'contract' | 'cargo'>('intercity');

  // Kết quả tra cứu nhanh
  const [chuyens, setChuyens] = useState<ChuyenTimKiem[]>([]);
  const [searched, setSearched] = useState(false);
  const [loadingChuyen, setLoadingChuyen] = useState(false);
  const [searchError, setSearchError] = useState('');
  const [copiedCode, setCopiedCode] = useState<string | null>(null);

  useEffect(() => {
    api
      .get<TuyenDuong[]>('/tuyen-duong')
      .then((data) => {
        const active = (data || []).filter((t) => t.trangThai);
        setTuyenList(active);
        if (active.length > 0) {
          // Khởi tạo điểm đi/đến hợp lệ nếu có
          const hasMienDong = active.find((t) =>
            t.diemDi.toLowerCase().includes('miền đông') || t.diemDi.toLowerCase().includes('hồ chí minh'),
          );
          if (hasMienDong) {
            setDiemDi(hasMienDong.diemDi);
            setDiemDen(hasMienDong.diemDen);
          } else {
            setDiemDi(active[0].diemDi);
            setDiemDen(active[0].diemDen);
          }
        }
      })
      .catch(() => {
        // Fallback danh sách nếu chưa có data
      });
  }, []);

  const diemDiOptions = useMemo(() => {
    const set = new Set<string>();
    tuyenList.forEach((t) => set.add(t.diemDi));
    if (!set.has('Bến xe Miền Đông')) set.add('Bến xe Miền Đông');
    return Array.from(set);
  }, [tuyenList]);

  const diemDenOptions = useMemo(() => {
    const list = tuyenList.filter((t) => !diemDi || t.diemDi === diemDi).map((t) => t.diemDen);
    const set = new Set(list);
    if (!set.has('Đà Lạt')) set.add('Đà Lạt');
    if (!set.has('Nha Trang')) set.add('Nha Trang');
    if (!set.has('Vũng Tàu')) set.add('Vũng Tàu');
    return Array.from(set);
  }, [tuyenList, diemDi]);

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
        setSearchError('Vui lòng chọn Điểm đi và Điểm đến');
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

        const data = await api.get<ChuyenTimKiem[]>(`/chuyen-xe/tim?${queryParams.toString()}`);
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

  const handleSelectQuickRoute = (r: QuickRoute) => {
    setDiemDi(r.diemDi);
    setDiemDen(r.diemDen);
    handleSearch(r.diemDi, r.diemDen);
    window.scrollTo({ top: 350, behavior: 'smooth' });
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

  const handleCopyCode = (code: string) => {
    navigator.clipboard.writeText(code);
    setCopiedCode(code);
    setTimeout(() => setCopiedCode(null), 2500);
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
                <span className="brand-subtitle">Bến xe Miền Đông Mới · SAMCO</span>
              </div>
            </Link>
          </div>

          <nav className="home-nav">
            <Link to="/" className="nav-item active">
              Trang chủ
            </Link>
            <a href="#tuyen-duong" className="nav-item">
              Lịch trình bến xe
            </a>
            <a href="#khuyen-mai" className="nav-item">
              <span>Khuyến mãi hot</span>
              <span className="nav-chip-new">Mới</span>
            </a>
            <a href="#doi-tac" className="nav-item">
              Nhà xe đối tác
            </a>
            <a href="#lien-he" className="nav-item">
              Hỗ trợ
            </a>
          </nav>

          <div className="home-actions">
            <a href="tel:19008888" className="action-hotline">
              <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                <path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6 19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 22 16.92z" />
              </svg>
              <div>
                <span className="hotline-sub">Tổng đài 24/7</span>
                <span className="hotline-num">1900 8888</span>
              </div>
            </a>

            {nguoiDung ? (
              <div className="user-dropdown-wrap">
                <Link
                  to={nguoiDung.quyen === 'KhachHang' ? '/app/dat-ve' : '/admin'}
                  className="btn-user-profile"
                >
                  <span className="user-avatar-badge">
                    {(nguoiDung.hoTen || nguoiDung.tenDangNhap).charAt(0).toUpperCase()}
                  </span>
                  <span className="user-name">{nguoiDung.hoTen || nguoiDung.tenDangNhap}</span>
                </Link>
                <button onClick={logout} className="btn-home-logout" title="Đăng xuất">
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4M16 17l5-5-5-5M21 12H9" />
                  </svg>
                </button>
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

      {/* 2. HERO SECTION */}
      <section className="hero-section">
        <div className="hero-overlay-gradient" />
        <div className="hero-content">
          <div className="hero-badge-tag">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="currentColor">
              <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z" />
            </svg>
            HỆ THỐNG VÉ CHÍNH THỨC · TỔNG CÔNG TY SAMCO
          </div>

          <h1 className="hero-title">
            Kết Nối Mọi Hành Trình <br />
            <span className="highlight-text">Từ Bến Xe Miền Đông</span>
          </h1>

          <p className="hero-subtitle">
            Hơn 500+ chuyến xe chất lượng cao mỗi ngày đi Tây Nguyên, Miền Trung, Đông Nam Bộ và Miền Tây.
            Đặt vé trực tuyến tiện lợi - 100% Giữ chỗ chính hãng.
          </p>

          {/* Dải cam kết uy tín */}
          <div className="hero-trust-badges">
            <div className="trust-badge">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
                <path d="M20 6L9 17l-5-5" />
              </svg>
              <span>Cam kết giữ đúng chỗ</span>
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
              <span>Hoàn hủy vé dễ dàng online</span>
            </div>
            <div className="trust-badge">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
                <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2" />
                <circle cx="9" cy="7" r="4" />
                <path d="M23 21v-2a4 4 0 0 0-3-3.87" />
                <path d="M16 3.13a4 4 0 0 1 0 7.75" />
              </svg>
              <span>Hơn 1.200.000 lượt khách</span>
            </div>
          </div>

          {/* FLOATING BOOKING WIDGET */}
          <div className="hero-booking-widget">
            <div className="widget-header-tabs">
              <div className="category-tabs">
                <button
                  type="button"
                  className={`tab-btn ${categoryTab === 'intercity' ? 'active' : ''}`}
                  onClick={() => setCategoryTab('intercity')}
                >
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <rect x="3" y="4" width="18" height="16" rx="2" />
                    <path d="M3 10h18M7 16h.01M17 16h.01" />
                  </svg>
                  Xe khách liên tỉnh
                </button>
                <button
                  type="button"
                  className={`tab-btn ${categoryTab === 'contract' ? 'active' : ''}`}
                  onClick={() => setCategoryTab('contract')}
                >
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <circle cx="12" cy="12" r="10" />
                    <path d="M12 6v6l4 2" />
                  </svg>
                  Thuê xe hợp đồng du lịch
                </button>
                <button
                  type="button"
                  className={`tab-btn ${categoryTab === 'cargo' ? 'active' : ''}`}
                  onClick={() => setCategoryTab('cargo')}
                >
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <path d="M1 3h15v13H1zM16 8h4l3 3v5h-7V8zM5.5 19a2.5 2.5 0 1 0 0-5 2.5 2.5 0 0 0 0 5zM18.5 19a2.5 2.5 0 1 0 0-5 2.5 2.5 0 0 0 0 5z" />
                  </svg>
                  Gửi hàng theo xe bến
                </button>
              </div>

              <div className="triptype-toggle">
                <button
                  type="button"
                  className={`pill-btn ${tripType === 'oneway' ? 'active' : ''}`}
                  onClick={() => setTripType('oneway')}
                >
                  Một chiều
                </button>
                <button
                  type="button"
                  className={`pill-btn ${tripType === 'roundtrip' ? 'active' : ''}`}
                  onClick={() => setTripType('roundtrip')}
                >
                  Khứ hồi
                </button>
              </div>
            </div>

            {/* FORM 5 Ô TRÊN 1 HÀNG */}
            <form
              className="widget-search-form"
              onSubmit={(e) => {
                e.preventDefault();
                handleSearch();
              }}
            >
              <div className="form-field-group field-from">
                <label>ĐIỂM ĐI</label>
                <div className="input-with-icon">
                  <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#1e40af" strokeWidth="2">
                    <circle cx="12" cy="12" r="10" />
                    <circle cx="12" cy="12" r="3" fill="#1e40af" />
                  </svg>
                  <input
                    type="text"
                    list="diem-di-list"
                    value={diemDi}
                    onChange={(e) => setDiemDi(e.target.value)}
                    placeholder="Điểm đi (Bến xe Miền Đông)"
                    required
                  />
                  <datalist id="diem-di-list">
                    {diemDiOptions.map((opt) => (
                      <option key={opt} value={opt} />
                    ))}
                  </datalist>
                </div>
                <span className="field-subtext">Bến xe Miền Đông Mới, TP. Thủ Đức</span>

                <button
                  type="button"
                  className="btn-swap-stations"
                  onClick={handleSwap}
                  title="Đảo chiều điểm đi và đến"
                >
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
                    <path d="M7 16V4M7 4L3 8M7 4L11 8M17 8V20M17 20L21 16M17 20L13 16" />
                  </svg>
                </button>
              </div>

              <div className="form-field-group field-to">
                <label>ĐIỂM ĐẾN</label>
                <div className="input-with-icon">
                  <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#ea580c" strokeWidth="2">
                    <path d="M12 2a8 8 0 0 0-8 8c0 5.25 8 12 8 12s8-6.75 8-12a8 8 0 0 0-8-8z" />
                    <circle cx="12" cy="10" r="3" />
                  </svg>
                  <input
                    type="text"
                    list="diem-den-list"
                    value={diemDen}
                    onChange={(e) => setDiemDen(e.target.value)}
                    placeholder="Đà Lạt, Nha Trang..."
                    required
                  />
                  <datalist id="diem-den-list">
                    {diemDenOptions.map((opt) => (
                      <option key={opt} value={opt} />
                    ))}
                  </datalist>
                </div>
                <span className="field-subtext">Bến xe khách / Trung tâm liên tỉnh</span>
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
                <div className="input-with-icon">
                  <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <rect x="3" y="4" width="18" height="18" rx="2" ry="2" />
                    <line x1="16" y1="2" x2="16" y2="6" />
                    <line x1="8" y1="2" x2="8" y2="6" />
                    <line x1="3" y1="10" x2="21" y2="10" />
                  </svg>
                  <input type="date" value={ngay} onChange={(e) => setNgay(e.target.value)} required />
                </div>
              </div>

              <div className="form-field-group field-return">
                <label>NGÀY VỀ</label>
                {tripType === 'roundtrip' ? (
                  <div className="input-with-icon">
                    <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                      <rect x="3" y="4" width="18" height="18" rx="2" ry="2" />
                      <line x1="3" y1="10" x2="21" y2="10" />
                    </svg>
                    <input type="date" defaultValue={ngay} />
                  </div>
                ) : (
                  <button
                    type="button"
                    className="btn-add-return"
                    onClick={() => setTripType('roundtrip')}
                  >
                    + Thêm ngày về
                  </button>
                )}
              </div>

              <div className="form-action-group">
                <button type="submit" className="btn-search-hero" disabled={loadingChuyen}>
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

            {/* GỢI Ý TUYẾN ĐƯỜNG PHỔ BIẾN */}
            <div className="quick-suggestions-row">
              <span className="suggestion-label">
                <svg width="16" height="16" viewBox="0 0 24 24" fill="#ea580c">
                  <path d="M17.5 12a5.5 5.5 0 1 1-11 0c0-1.87 1.02-3.62 2.6-4.57.3-.18.7.02.73.37.1 1.07.72 2.02 1.67 2.45.2.09.4-.04.4-.26 0-.36-.07-.71-.2-1.04C10.5 6.47 12.35 4 14.5 4c.26 0 .44.25.37.5-.47 1.62-.2 3.39.73 4.75.14.21.43.25.62.1.4-.3.73-.67 1-1.08.15-.24.48-.22.61.03 1.05 2.05.67 4.7-.33 6.7z" />
                </svg>
                Tuyến xe nổi bật hôm nay:
              </span>
              <div className="suggestion-pills">
                {POPULAR_ROUTES.map((route) => (
                  <button
                    key={`${route.diemDi}-${route.diemDen}`}
                    type="button"
                    className="suggestion-pill"
                    onClick={() => handleSelectQuickRoute(route)}
                  >
                    <span>{route.diemDen}</span>
                    <strong className="pill-price">từ {formatVnd(route.giaTu)}</strong>
                  </button>
                ))}
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* 3. KẾT QUẢ TÌM KIẾM CHUYẾN XE (NẾU ĐÃ BẤM TÌM HOẶC CHỌN TUYẾN) */}
      {searched && (
        <section className="search-results-section" id="ket-qua">
          <div className="results-container">
            <div className="results-header">
              <div>
                <h2>
                  Kết quả chuyến xe: <span className="text-primary">{diemDi} ➔ {diemDen}</span>
                </h2>
                <p className="results-sub">
                  Ngày khởi hành: <strong>{ngay}</strong> · Tìm thấy <strong>{chuyens.length}</strong> chuyến phù hợp
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
                <p>Đang tải dữ liệu chuyến xe từ hệ thống bến...</p>
              </div>
            ) : chuyens.length === 0 ? (
              <div className="no-trips-card">
                <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="#94a3b8" strokeWidth="1.5">
                  <circle cx="12" cy="12" r="10" />
                  <line x1="8" y1="12" x2="16" y2="12" />
                </svg>
                <h3>Chưa có chuyến xe nào phù hợp</h3>
                <p>Hiện chưa có chuyến xe nào chạy tuyến này vào ngày đã chọn. Vui lòng thử ngày khác hoặc chọn tuyến đường khác.</p>
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
                            {chuyen.tenNhaXe ? chuyen.tenNhaXe.charAt(0) : 'BX'}
                          </div>
                          <div>
                            <strong className="operator-name">{chuyen.tenNhaXe}</strong>
                            <span className="vehicle-type-label">
                              {chuyen.tenLoaiXe} ({chuyen.soCho} chỗ) · BKS: {chuyen.bienSoXe}
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
                          <span className="amenity-chip">Wifi 5G</span>
                          <span className="amenity-chip">Nước suối & khăn</span>
                          <span className="amenity-chip">Cổng sạc USB</span>
                          <span className="amenity-chip">Đón tại bến</span>
                        </div>
                      </div>

                      <div className="trip-card-action">
                        <div className="price-tag">
                          <span className="price-label">Giá vé niêm yết</span>
                          <strong className="price-val">{formatVnd(chuyen.giaVeThucTe)}</strong>
                        </div>
                        <span className={`seats-left-badge ${conTrong <= 5 ? 'seats-urgent' : ''}`}>
                          Còn {conTrong} chỗ trống
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

      {/* 4. KHUYẾN MÃI & ƯU ĐÃI ĐỘC QUYỀN */}
      <section className="section-promos" id="khuyen-mai">
        <div className="section-container">
          <div className="section-header">
            <div>
              <span className="section-badge-chip">ƯU ĐÃI TUYẾN ĐƯỜNG SAMCO</span>
              <h2 className="section-title">Ưu đãi & Khuyến mãi độc quyền</h2>
              <p className="section-subtitle">
                Đặt vé trực tuyến tại CoachBooking để nhận ngay trợ giá và quà tặng hấp dẫn
              </p>
            </div>
            <a href="#khuyen-mai" className="section-view-all">
              Xem tất cả ưu đãi ➔
            </a>
          </div>

          <div className="promo-cards-grid">
            {PROMOTIONS.map((promo) => (
              <div key={promo.id} className="promo-card">
                <div className="promo-card-body">
                  <div className="promo-meta-top">
                    <span className={`promo-tag ${promo.pillClass}`}>{promo.tag}</span>
                    <span className="promo-expiry">{promo.expiry}</span>
                  </div>
                  <h3 className="promo-title">{promo.title}</h3>
                  <p className="promo-desc">{promo.desc}</p>
                </div>
                <div className="promo-card-footer">
                  <div className="promo-code-block">
                    <span className="code-label">MÃ GIẢM GIÁ</span>
                    <strong className="code-val">{promo.code}</strong>
                  </div>
                  <button
                    type="button"
                    className="btn-copy-code"
                    onClick={() => handleCopyCode(promo.code)}
                  >
                    {copiedCode === promo.code ? 'Đã chép! ✓' : 'Sao chép mã'}
                  </button>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* 5. CÁC NHÀ XE HÀNG ĐẦU BẾN XE MIỀN ĐÔNG */}
      <section className="section-operators" id="doi-tac">
        <div className="section-container">
          <div className="text-center-wrapper">
            <span className="section-badge-chip">MẠNG LƯỚI ĐỐI TÁC</span>
            <h2 className="section-title">Các nhà xe hàng đầu tại Bến xe Miền Đông</h2>
            <p className="section-subtitle">
              Đội xe hiện đại, giường phòng limousine cao cấp, xuất bến đúng giờ với dịch vụ tiêu chuẩn
            </p>
          </div>

          <div className="operators-grid">
            {OPERATORS.map((op) => (
              <div key={op.name} className="operator-card">
                <div className="operator-card-top">
                  <div className="operator-avatar" style={{ backgroundColor: `${op.color}20`, color: op.color }}>
                    {op.initials}
                  </div>
                  <div className="operator-rating">
                    <span>★</span>
                    <strong>{op.rating}</strong>
                  </div>
                </div>
                <h4 className="op-name">{op.name}</h4>
                <p className="op-type">{op.type}</p>
                <div className="operator-card-bottom">
                  <span>{op.trips}</span>
                  <strong className="op-dest">{op.dest}</strong>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* 6. TẠI SAO CHỌN COACHBOOKING */}
      <section className="section-features">
        <div className="section-container">
          <div className="text-center-wrapper">
            <span className="section-badge-chip">TIỆN ÍCH VƯỢT TRỘI</span>
            <h2 className="section-title">Tại sao chọn đặt vé trên CoachBooking?</h2>
            <p className="section-subtitle">
              Mang đến trải nghiệm đặt vé nhanh chóng, an toàn, minh bạch cho mọi hành trình liên tỉnh
            </p>
          </div>

          <div className="features-grid">
            <div className="feature-card">
              <div className="feature-icon icon-blue">
                <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                  <path d="M4 15s1-1 4-1 5 2 8 2 4-1 4-1V3s-1 1-4 1-5-2-8-2-4 1-4 1zM4 22v-7" />
                </svg>
              </div>
              <h3>Đa dạng chuyến xe</h3>
              <p>
                Kết nối hơn 120 tuyến đường liên tỉnh khắp cả nước xuất bến trực tiếp từ Bến xe Miền Đông Mới,
                đáp ứng mọi nhu cầu di chuyển công tác, du lịch và về quê.
              </p>
            </div>

            <div className="feature-card">
              <div className="feature-icon icon-orange">
                <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                  <path d="M7 13h10M7 17h10M5 5h14a2 2 0 0 1 2 2v10a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V7a2 2 0 0 1 2-2z" />
                </svg>
              </div>
              <h3>Chọn ghế trực quan</h3>
              <p>
                Sơ đồ 2 tầng cập nhật theo thời gian thực, xem rõ vị trí tầng trên, tầng dưới, cửa sổ hay lối đi
                trước khi tiến hành thanh toán giữ chỗ.
              </p>
            </div>

            <div className="feature-card">
              <div className="feature-icon icon-green">
                <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                  <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z" />
                </svg>
              </div>
              <h3>An tâm hành trình</h3>
              <p>
                Nhà xe uy tín, kiểm duyệt xuất bến đúng quy chuẩn an toàn kỹ thuật và điều hành giao thông của
                Tổng công ty Samco. Đội ngũ tổng đài hỗ trợ 24/7.
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* 7. FOOTER */}
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
              <h4>Về CoachBooking</h4>
              <ul>
                <li><a href="#gioi-thieu">Giới thiệu Bến xe Miền Đông</a></li>
                <li><a href="#tuyen-duong">Lịch trình các tuyến xe</a></li>
                <li><a href="#gia-ve">Bảng giá vé niêm yết</a></li>
                <li><a href="#doi-tac">Dành cho đối tác nhà xe</a></li>
              </ul>
            </div>

            <div className="footer-col-links">
              <h4>Hỗ trợ khách hàng</h4>
              <ul>
                <li><a href="#tra-cuu">Tra cứu thông tin vé</a></li>
                <li><a href="#chinh-sach">Chính sách hủy và đổi vé</a></li>
                <li><a href="#thanh-toan">Hướng dẫn thanh toán</a></li>
                <li><a href="#bao-mat">Chính sách bảo mật thông tin</a></li>
              </ul>
            </div>

            <div className="footer-col-links">
              <h4>Phương thức thanh toán</h4>
              <p className="footer-subtext">Hỗ trợ thanh toán bảo mật 24/7 qua Napas, VietQR, Ví điện tử và Thẻ ngân hàng</p>
              <div className="payment-badges-row">
                <span className="pay-badge">VietQR</span>
                <span className="pay-badge">Napas</span>
                <span className="pay-badge">MoMo</span>
                <span className="pay-badge">ATM / Visa</span>
              </div>
            </div>
          </div>

          <div className="footer-bottom-bar">
            <span>© 2026 CoachBooking · Bến xe Miền Đông. Bản quyền thuộc về Tổng công ty SAMCO.</span>
            <div className="footer-legal-links">
              <a href="#dieu-khoan">Điều khoản sử dụng</a>
              <span>·</span>
              <a href="#bao-mat">Bảo mật</a>
              <span>·</span>
              <a href="#quy-che">Quy chế hoạt động</a>
            </div>
          </div>
        </div>
      </footer>
    </div>
  );
}
