import { useEffect, useRef, useState } from 'react';
import { Link, NavLink, useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../../auth/AuthContext';
import { RoleBadge } from '../common/Badge';
import './CustomerNavbar.css';

export function CustomerNavbar() {
  const { user, logout } = useAuth();
  const [userMenuOpen, setUserMenuOpen] = useState(false);
  const [mobileNavOpen, setMobileNavOpen] = useState(false);
  const userMenuRef = useRef<HTMLDivElement>(null);
  const location = useLocation();
  const navigate = useNavigate();

  // Close menus on route change
  useEffect(() => {
    setUserMenuOpen(false);
    setMobileNavOpen(false);
  }, [location.pathname]);

  // Close user dropdown on click outside
  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (
        userMenuRef.current &&
        !userMenuRef.current.contains(event.target as Node)
      ) {
        setUserMenuOpen(false);
      }
    }
    if (userMenuOpen) {
      document.addEventListener('mousedown', handleClickOutside);
    }
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [userMenuOpen]);

  // Lock body scroll when mobile nav is open
  useEffect(() => {
    if (mobileNavOpen) {
      const prevOverflow = document.body.style.overflow;
      document.body.style.overflow = 'hidden';
      return () => {
        document.body.style.overflow = prevOverflow;
      };
    }
  }, [mobileNavOpen]);

  const handleLogout = () => {
    setUserMenuOpen(false);
    setMobileNavOpen(false);
    logout();
    navigate('/');
  };

  const userDisplayName = user?.fullName ?? user?.username ?? 'Khách';
  const role = user?.role ?? 'KhachHang';

  return (
    <header className="shared-customer-header">
      <div className="shared-customer-header-inner">
        {/* BRAND LOGO */}
        <Link to="/" className="shared-brand-link">
          <div className="shared-brand-badge">BX</div>
          <div className="shared-brand-text">
            <span className="shared-brand-title">CoachBooking</span>
            <span className="shared-brand-subtitle">Bến xe Miền Đông Mới · SAMCO</span>
          </div>
        </Link>

        {/* DESKTOP NAVIGATION LINKS */}
        <nav className="shared-customer-nav" aria-label="Điều hướng chính">
          <NavLink
            to="/"
            end
            className={({ isActive }) => `shared-nav-item ${isActive ? 'active' : ''}`}
          >
            Trang chủ
          </NavLink>
          <NavLink
            to="/tim-chuyen"
            className={({ isActive }) => `shared-nav-item ${isActive ? 'active' : ''}`}
          >
            Tìm chuyến xe
          </NavLink>
          <NavLink
            to="/tra-cuu-ve"
            className={({ isActive }) => `shared-nav-item ${isActive ? 'active' : ''}`}
          >
            Tra cứu vé
          </NavLink>
          <NavLink
            to="/ve-cua-toi"
            className={({ isActive }) => `shared-nav-item ${isActive ? 'active' : ''}`}
          >
            Vé của tôi
          </NavLink>
        </nav>

        {/* ACTIONS: AUTH / USER MENU & MOBILE HAMBURGER */}
        <div className="shared-header-actions">
          {user ? (
            <div className="shared-user-wrap" ref={userMenuRef}>
              <button
                type="button"
                className={`shared-btn-profile ${userMenuOpen ? 'active' : ''}`}
                onClick={() => setUserMenuOpen((prev) => !prev)}
                aria-expanded={userMenuOpen}
                aria-haspopup="true"
              >
                <span className="shared-avatar-badge">
                  {userDisplayName.charAt(0).toUpperCase()}
                </span>
                <span className="shared-user-name">{userDisplayName}</span>
                <svg
                  className={`shared-chevron ${userMenuOpen ? 'open' : ''}`}
                  width="14"
                  height="14"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  strokeWidth="2.5"
                  aria-hidden="true"
                >
                  <polyline points="6 9 12 15 18 9" />
                </svg>
              </button>

              {userMenuOpen && (
                <div className="shared-dropdown-menu" role="menu">
                  <div className="shared-menu-header">
                    <div className="shared-menu-avatar">
                      {userDisplayName.charAt(0).toUpperCase()}
                    </div>
                    <div className="shared-menu-info">
                      <span className="shared-menu-name">{userDisplayName}</span>
                      <RoleBadge role={role} />
                    </div>
                  </div>

                  <div className="shared-menu-divider" />

                  <Link
                    to={role === 'KhachHang' ? '/ho-so' : '/admin/ho-so'}
                    className="shared-menu-item"
                    role="menuitem"
                    onClick={() => setUserMenuOpen(false)}
                  >
                    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                      <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
                      <circle cx="12" cy="7" r="4" />
                    </svg>
                    <span>Thông tin tài khoản</span>
                  </Link>

                  <Link
                    to="/ve-cua-toi"
                    className="shared-menu-item"
                    role="menuitem"
                    onClick={() => setUserMenuOpen(false)}
                  >
                    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                      <rect x="3" y="4" width="18" height="16" rx="2" />
                      <path d="M7 8h10M7 12h10M7 16h6" />
                    </svg>
                    <span>Vé của tôi & Lịch sử</span>
                  </Link>

                  {(role === 'Admin' || role === 'NhanVien') && (
                    <Link
                      to="/admin"
                      className="shared-menu-item"
                      role="menuitem"
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
                  )}

                  <div className="shared-menu-divider" />

                  <button
                    type="button"
                    className="shared-menu-item logout"
                    role="menuitem"
                    onClick={handleLogout}
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
            <div className="shared-guest-buttons">
              <Link to="/login" className="shared-btn-login">
                Đăng nhập
              </Link>
              <Link to="/register" className="shared-btn-register">
                Đăng ký
              </Link>
            </div>
          )}

          {/* MOBILE HAMBURGER BUTTON */}
          <button
            type="button"
            className="mobile-hamburger-btn"
            onClick={() => setMobileNavOpen((prev) => !prev)}
            aria-label={mobileNavOpen ? 'Đóng menu' : 'Mở menu điều hướng'}
            aria-expanded={mobileNavOpen}
          >
            {mobileNavOpen ? (
              <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.2">
                <line x1="18" y1="6" x2="6" y2="18" />
                <line x1="6" y1="6" x2="18" y2="18" />
              </svg>
            ) : (
              <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.2">
                <line x1="3" y1="12" x2="21" y2="12" />
                <line x1="3" y1="6" x2="21" y2="6" />
                <line x1="3" y1="18" x2="21" y2="18" />
              </svg>
            )}
          </button>
        </div>
      </div>

      {/* MOBILE DRAWER OVERLAY & PANEL */}
      {mobileNavOpen && (
        <div className="mobile-drawer-overlay" onClick={() => setMobileNavOpen(false)}>
          <div
            className="mobile-drawer-panel"
            onClick={(event) => event.stopPropagation()}
            role="dialog"
            aria-label="Menu điều hướng di động"
          >
            <div className="mobile-drawer-header">
              <div className="mobile-drawer-brand">
                <div className="shared-brand-badge">BX</div>
                <span>CoachBooking</span>
              </div>
              <button
                type="button"
                className="icon-btn mobile-drawer-close"
                onClick={() => setMobileNavOpen(false)}
                aria-label="Đóng"
              >
                ×
              </button>
            </div>

            {user ? (
              <div className="mobile-user-card">
                <div className="shared-avatar-badge">
                  {userDisplayName.charAt(0).toUpperCase()}
                </div>
                <div className="mobile-user-info">
                  <span className="mobile-user-name">{userDisplayName}</span>
                  <RoleBadge role={role} />
                </div>
              </div>
            ) : null}

            <nav className="mobile-drawer-nav">
              <NavLink
                to="/"
                end
                className={({ isActive }) => `mobile-nav-link ${isActive ? 'active' : ''}`}
                onClick={() => setMobileNavOpen(false)}
              >
                Trang chủ
              </NavLink>
              <NavLink
                to="/tim-chuyen"
                className={({ isActive }) => `mobile-nav-link ${isActive ? 'active' : ''}`}
                onClick={() => setMobileNavOpen(false)}
              >
                Tìm chuyến xe
              </NavLink>
              <NavLink
                to="/tra-cuu-ve"
                className={({ isActive }) => `mobile-nav-link ${isActive ? 'active' : ''}`}
                onClick={() => setMobileNavOpen(false)}
              >
                Tra cứu vé
              </NavLink>
              <NavLink
                to="/ve-cua-toi"
                className={({ isActive }) => `mobile-nav-link ${isActive ? 'active' : ''}`}
                onClick={() => setMobileNavOpen(false)}
              >
                Vé của tôi
              </NavLink>

              {user ? (
                <>
                  <div className="mobile-nav-divider" />
                  <NavLink
                    to={role === 'KhachHang' ? '/ho-so' : '/admin/ho-so'}
                    className={({ isActive }) => `mobile-nav-link ${isActive ? 'active' : ''}`}
                    onClick={() => setMobileNavOpen(false)}
                  >
                    Thông tin tài khoản
                  </NavLink>
                  {(role === 'Admin' || role === 'NhanVien') && (
                    <NavLink
                      to="/admin"
                      className={({ isActive }) => `mobile-nav-link ${isActive ? 'active' : ''}`}
                      onClick={() => setMobileNavOpen(false)}
                    >
                      Bàn làm việc quản trị
                    </NavLink>
                  )}
                  <button
                    type="button"
                    className="mobile-nav-link mobile-logout-btn"
                    onClick={handleLogout}
                  >
                    Đăng xuất
                  </button>
                </>
              ) : (
                <div className="mobile-auth-actions">
                  <Link
                    to="/login"
                    className="btn btn-secondary w-full"
                    onClick={() => setMobileNavOpen(false)}
                  >
                    Đăng nhập
                  </Link>
                  <Link
                    to="/register"
                    className="btn btn-primary w-full"
                    onClick={() => setMobileNavOpen(false)}
                  >
                    Đăng ký tài khoản
                  </Link>
                </div>
              )}
            </nav>
          </div>
        </div>
      )}
    </header>
  );
}
