import { useState } from 'react';
import { NavLink, Outlet, useNavigate } from 'react-router-dom';
import { useAuth } from '../auth/AuthContext';
import type { Quyen } from '../api/types';

const ICON_PATHS: Record<string, string> = {
  dashboard: 'M3 3h7v7H3zM14 3h7v7h-7zM14 14h7v7h-7zM3 14h7v7H3z',
  ticket: 'M4 7h16v3a2 2 0 0 0 0 4v3H4v-3a2 2 0 0 0 0-4zM12 7v10',
  receipt: 'M6 3h12v18l-3-2-3 2-3-2-3 2zM9 8h6M9 12h6',
  users: 'M17 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2M9.5 3a4 4 0 1 1 0 8 4 4 0 0 1 0-8M22 21v-2a4 4 0 0 0-3-3.87',
  bus: 'M4 6a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v9H4zM4 15h16M7 19v-2M17 19v-2M8 8h8',
  layers: 'M12 3l9 5-9 5-9-5zM3 13l9 5 9-5',
  seat: 'M6 4v9a2 2 0 0 0 2 2h8M18 9v9M6 20h12',
  route: 'M6 19a3 3 0 1 0 0-6 3 3 0 0 0 0 6M18 11a3 3 0 1 0 0-6 3 3 0 0 0 0 6M9 16h6a3 3 0 0 0 0-6H9a3 3 0 0 1 0-6',
  price: 'M12 2v20M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6',
  driver: 'M12 12a4 4 0 1 0 0-8 4 4 0 0 0 0 8M4 21v-1a6 6 0 0 1 6-6h4a6 6 0 0 1 6 6v1',
  staff: 'M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2M9 11a4 4 0 1 0 0-8 4 4 0 0 0 0 8M19 8v6M22 11h-6',
  account: 'M12 15a4 4 0 1 0 0-8 4 4 0 0 0 0 8M4 21a8 8 0 0 1 16 0',
  history: 'M3 12a9 9 0 1 0 3-6.7M3 4v5h5M12 8v4l3 2',
  logout: 'M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4M16 17l5-5-5-5M21 12H9',
  menu: 'M4 6h16M4 12h16M4 18h16',
};

function Icon({ name }: { name: string }) {
  return (
    <svg
      className="nav-icon"
      viewBox="0 0 24 24"
      width="18"
      height="18"
      fill="none"
      stroke="currentColor"
      strokeWidth="1.7"
      strokeLinecap="round"
      strokeLinejoin="round"
      aria-hidden="true"
    >
      <path d={ICON_PATHS[name] ?? ICON_PATHS.dashboard} />
    </svg>
  );
}

interface NavEntry {
  to: string;
  label: string;
  icon: string;
  end?: boolean;
}

interface NavGroup {
  title?: string;
  items: NavEntry[];
}

const STAFF_NAV: NavGroup[] = [
  {
    items: [{ to: '/admin', label: 'Tổng quan', icon: 'dashboard', end: true }],
  },
  {
    title: 'Nghiệp vụ',
    items: [
      { to: '/admin/ban-ve', label: 'Bán vé', icon: 'ticket' },
      { to: '/admin/hoa-don', label: 'Hóa đơn', icon: 'receipt' },
      { to: '/admin/khach-hang', label: 'Khách hàng', icon: 'users' },
    ],
  },
  {
    title: 'Danh mục',
    items: [
      { to: '/admin/chuyen-xe', label: 'Chuyến xe', icon: 'bus' },
      { to: '/admin/nha-xe', label: 'Nhà xe', icon: 'layers' },
      { to: '/admin/loai-xe', label: 'Loại xe', icon: 'seat' },
      { to: '/admin/xe', label: 'Xe', icon: 'bus' },
      { to: '/admin/tuyen-duong', label: 'Tuyến đường', icon: 'route' },
      { to: '/admin/gia-ve', label: 'Giá vé', icon: 'price' },
      { to: '/admin/tai-xe', label: 'Tài xế', icon: 'driver' },
    ],
  },
  {
    title: 'Tài khoản',
    items: [{ to: '/admin/ho-so', label: 'Hồ sơ cá nhân', icon: 'account' }],
  },
];

const STAFF_SYSTEM_NAV: NavGroup = {
  title: 'Hệ thống',
  items: [
    { to: '/admin/nhan-vien', label: 'Nhân viên', icon: 'staff' },
    { to: '/admin/tai-khoan', label: 'Tài khoản', icon: 'account' },
  ],
};

const CUSTOMER_NAV: NavGroup[] = [
  {
    items: [
      { to: '/app/dat-ve', label: 'Đặt vé', icon: 'ticket' },
      { to: '/app/lich-su', label: 'Lịch sử đặt vé', icon: 'history' },
      { to: '/app/ho-so', label: 'Hồ sơ cá nhân', icon: 'account' },
    ],
  },
];

const ROLE_LABEL: Record<Quyen, string> = {
  Admin: 'Quản trị viên',
  NhanVien: 'Nhân viên',
  KhachHang: 'Khách hàng',
};

export function AppLayout() {
  const { nguoiDung, logout } = useAuth();
  const navigate = useNavigate();
  const [open, setOpen] = useState(false);

  if (!nguoiDung) return null;

  const isStaff = nguoiDung.quyen === 'Admin' || nguoiDung.quyen === 'NhanVien';
  const groups: NavGroup[] = isStaff
    ? nguoiDung.quyen === 'Admin'
      ? [...STAFF_NAV, STAFF_SYSTEM_NAV]
      : STAFF_NAV
    : CUSTOMER_NAV;

  const displayName = nguoiDung.hoTen ?? nguoiDung.tenDangNhap;
  const initials = displayName
    .split(' ')
    .filter(Boolean)
    .slice(-2)
    .map((part) => part.charAt(0).toUpperCase())
    .join('');

  const handleLogout = () => {
    logout();
    navigate('/login', { replace: true });
  };

  return (
    <div className="app-shell">
      <aside className={`sidebar ${open ? 'sidebar-open' : ''}`}>
        <div className="brand">
          <div className="brand-logo">BX</div>
          <div className="brand-text">
            <strong>CoachBooking</strong>
            <span>Quản lý bán vé xe</span>
          </div>
        </div>

        <nav className="nav">
          {groups.map((group, index) => (
            <div className="nav-group" key={group.title ?? `group-${index}`}>
              {group.title ? <span className="nav-group-title">{group.title}</span> : null}
              {group.items.map((item) => (
                <NavLink
                  key={item.to}
                  to={item.to}
                  end={item.end}
                  className={({ isActive }) => `nav-link ${isActive ? 'nav-link-active' : ''}`}
                  onClick={() => setOpen(false)}
                >
                  <Icon name={item.icon} />
                  <span>{item.label}</span>
                </NavLink>
              ))}
            </div>
          ))}
        </nav>

        <div className="sidebar-footer">
          <span>CoachBooking · Spring Boot + React</span>
          <span>Phiên bản 1.0</span>
        </div>
      </aside>

      {open ? <div className="sidebar-overlay" onClick={() => setOpen(false)} /> : null}

      <div className="app-main">
        <header className="topbar">
          <button type="button" className="icon-btn only-mobile" onClick={() => setOpen(true)} aria-label="Mở menu">
            <Icon name="menu" />
          </button>

          <div className="topbar-title">
            <span className="topbar-app">Hệ thống quản lý bán vé xe miền Đông</span>
          </div>

          <div className="topbar-user">
            <div className="avatar" title={displayName}>
              {initials || 'U'}
            </div>
            <div className="user-meta">
              <strong>{displayName}</strong>
              <span className="role-chip">{ROLE_LABEL[nguoiDung.quyen]}</span>
            </div>
            <button type="button" className="btn btn-ghost btn-sm" onClick={handleLogout}>
              <Icon name="logout" />
              <span className="hide-sm">Đăng xuất</span>
            </button>
          </div>
        </header>

        <main className="content">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
