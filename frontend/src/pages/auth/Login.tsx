import { useState } from 'react';
import { Link, Navigate, useNavigate } from 'react-router-dom';
import { getErrorMessage } from '../../api/client';
import { homePathFor, useAuth } from '../../auth/AuthContext';
import { isAdminPortal } from '../../auth/portal';
import { FormField } from '../../components/common/FormField';

export function Login() {
  const { user, login } = useAuth();
  const navigate = useNavigate();
  const admin = isAdminPortal();

  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(true);
  const [error, setError] = useState('');
  const [busy, setBusy] = useState(false);

  if (user) {
    const role = user.role;
    return <Navigate to={homePathFor(role)} replace />;
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
      const role = loggedUser.role;
      navigate(homePathFor(role), { replace: true });
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setBusy(false);
    }
  };

  const handleGoogleLogin = () => {
    // Thông báo môi trường demo
    setError('Tính năng đăng nhập Google đang được tích hợp. Vui lòng đăng nhập bằng tài khoản hệ thống.');
  };

  return (
    <div className="auth-page auth-page-single">
      <div className="auth-card">
        <div className="auth-brand">
          <div className="brand-logo">{admin ? 'AD' : 'BX'}</div>
          <div>
            <strong>{admin ? 'CoachBooking Quản trị' : 'CoachBooking'}</strong>
            <span>
              {admin
                ? 'Hệ thống quản lý bán vé xe miền Đông'
                : 'Bến xe Miền Đông Mới • SAMCO'}
            </span>
          </div>
        </div>

        <h1>{admin ? 'Đăng nhập Quản trị' : 'Đăng nhập'}</h1>
        <p className="auth-subtitle">
          {admin
            ? 'Sử dụng tài khoản quản trị hoặc nhân viên để tiếp tục.'
            : 'Đăng nhập tài khoản để tiếp tục đặt vé và quản lý chuyến đi.'}
        </p>

        {error ? <div className="alert alert-error">{error}</div> : null}

        <form
          className="auth-form"
          onSubmit={(event) => {
            event.preventDefault();
            void handleSubmit();
          }}
        >
          <FormField
            label="Tên đăng nhập"
            value={username}
            onChange={setUsername}
            placeholder="Nhập tên đăng nhập hoặc email"
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

          <div className="auth-options-row">
            <label className="auth-checkbox-label">
              <input
                type="checkbox"
                checked={rememberMe}
                onChange={(e) => setRememberMe(e.target.checked)}
              />
              <span>Ghi nhớ đăng nhập</span>
            </label>
            <Link to="/forgot-password" className="auth-forgot-link">
              Quên mật khẩu?
            </Link>
          </div>

          <button type="submit" className="btn btn-primary btn-block" disabled={busy}>
            {busy ? 'Đang đăng nhập…' : 'Đăng nhập'}
          </button>
        </form>

        {!admin && (
          <>
            <div className="auth-divider">hoặc tiếp tục với</div>
            <button
              type="button"
              className="btn-google-full"
              onClick={handleGoogleLogin}
            >
              <svg className="google-icon-svg" viewBox="0 0 24 24">
                <path
                  fill="#4285F4"
                  d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z"
                />
                <path
                  fill="#34A853"
                  d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z"
                />
                <path
                  fill="#FBBC05"
                  d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.06H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.94l2.85-2.22.81-.63z"
                />
                <path
                  fill="#EA4335"
                  d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.06l3.66 2.84c.87-2.6 3.3-4.52 6.16-4.52z"
                />
              </svg>
              <span>Đăng nhập bằng Google</span>
            </button>
          </>
        )}

        <div className="auth-links">
          {!admin ? (
            <span>
              Chưa có tài khoản? <Link to="/register">Đăng ký ngay</Link>
            </span>
          ) : (
            <Link to="/forgot-password">Quên mật khẩu?</Link>
          )}
        </div>

        <div className="auth-trust-note">
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect>
            <path d="M7 11V7a5 5 0 0 1 10 0v4"></path>
          </svg>
          <span>Hệ thống bảo mật xác thực SAMCO • SSL 256-bit</span>
        </div>
      </div>
    </div>
  );
}
