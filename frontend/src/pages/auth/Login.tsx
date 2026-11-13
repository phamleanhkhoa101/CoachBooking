import { useState } from 'react';
import { Link, Navigate, useNavigate } from 'react-router-dom';
import { getErrorMessage } from '../../api/client';
import { homePathFor, useAuth } from '../../auth/AuthContext';
import { isAdminPortal } from '../../auth/portal';
import { FormField } from '../../components/common/FormField';

/** Panel trái chung cho tất cả trang auth (ẩn trên mobile). */
function AuthPanel({ admin }: { admin: boolean }) {
  return (
    <aside className="auth-panel">
      <div className="auth-panel-top">
        <div className="auth-panel-logo">{admin ? 'AD' : 'BX'}</div>
        <div>
          <div className="auth-panel-name">CoachBooking</div>
          <div className="auth-panel-sub">Bến xe Miền Đông · SAMCO</div>
        </div>
      </div>

      <div className="auth-panel-body">
        {admin ? (
          <>
            <h2 className="auth-panel-headline">
              Hệ thống<br />
              <span>Quản trị</span> vé xe
            </h2>
            <p className="auth-panel-desc">
              Dành cho quản trị viên và nhân viên bán vé. Quản lý chuyến xe, ghế ngồi, hóa đơn và doanh thu toàn hệ thống.
            </p>
            <ul className="auth-panel-features">
              <li>Quản lý chuyến xe & lịch trình</li>
              <li>Bán vé tại quầy & theo dõi ghế thời gian thực</li>
              <li>Thống kê doanh thu theo ngày / tháng</li>
              <li>Quản lý tài xế, nhà xe, khách hàng</li>
            </ul>
          </>
        ) : (
          <>
            <h2 className="auth-panel-headline">
              Kết nối mọi<br />
              <span>hành trình</span>
            </h2>
            <p className="auth-panel-desc">
              Đặt vé xe khách trực tuyến từ Bến xe Miền Đông. Giá niêm yết, giữ chỗ nhanh, xác nhận tức thì.
            </p>
            <ul className="auth-panel-features">
              <li>Tìm & đặt vé trong vài giây</li>
              <li>Chọn ghế trực quan theo sơ đồ xe</li>
              <li>Hủy vé linh hoạt trước giờ khởi hành</li>
              <li>Lịch sử chuyến đi luôn trong tầm tay</li>
            </ul>
          </>
        )}
      </div>

      <div className="auth-panel-footer">
        © {new Date().getFullYear()} CoachBooking · Bến xe Miền Đông
      </div>
    </aside>
  );
}

export function Login() {
  const { user, login } = useAuth();
  const navigate = useNavigate();
  const admin = isAdminPortal();

  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [busy, setBusy] = useState(false);

  if (user) {
    return <Navigate to={homePathFor(user.role)} replace />;
  }

  const handleSubmit = async () => {
    if (!username.trim() || !password) {
      setError('Vui lòng nhập tên đăng nhập và mật khẩu.');
      return;
    }
    setBusy(true);
    setError('');
    try {
      const loggedUser = await login(username.trim(), password, admin ? 'admin' : 'user');
      navigate(homePathFor(loggedUser.role), { replace: true });
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setBusy(false);
    }
  };

  return (
    <div className="auth-root">
      <AuthPanel admin={admin} />

      <div className="auth-form-panel">
        <div className="auth-form-box">
          <span className="auth-eyebrow">
            {admin ? 'Quản trị viên / Nhân viên' : 'Khách hàng'}
          </span>
          <h1>Đăng nhập</h1>
          <p className="auth-subtitle">
            {admin
              ? 'Nhập thông tin tài khoản quản trị để tiếp tục.'
              : 'Chào mừng trở lại! Nhập tài khoản để đặt vé.'}
          </p>

          {error ? <div className="alert alert-error">{error}</div> : null}

          <form
            className="auth-form"
            onSubmit={(e) => {
              e.preventDefault();
              void handleSubmit();
            }}
          >
            <FormField
              label="Tên đăng nhập"
              value={username}
              onChange={setUsername}
              placeholder="Nhập tên đăng nhập"
              autoComplete="username"
              required
            />
            <FormField
              label="Mật khẩu"
              value={password}
              onChange={setPassword}
              type="password"
              placeholder="Nhập mật khẩu"
              autoComplete="current-password"
              required
            />
            <button type="submit" className="btn btn-primary btn-block" disabled={busy}>
              {busy ? 'Đang đăng nhập…' : 'Đăng nhập'}
            </button>
          </form>

          <div className="auth-links">
            <Link to="/forgot-password">Quên mật khẩu?</Link>
            {!admin && (
              <span>
                Chưa có tài khoản? <Link to="/register">Đăng ký</Link>
              </span>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
