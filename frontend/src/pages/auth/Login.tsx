import { useState } from 'react';
import { Link, Navigate, useLocation, useNavigate } from 'react-router-dom';
import { getErrorMessage } from '../../api/client';
import { homePathFor, useAuth } from '../../auth/AuthContext';
import { isAdminPortal } from '../../auth/portal';
import { BrandLogo } from '../../components/common/BrandLogo';

export function Login() {
  const { user, login } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const admin = isAdminPortal();
  const initialNotice = (location.state as { notice?: string } | null)?.notice ?? '';
  const initialError = new URLSearchParams(location.search).get('error') ?? '';

  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [rememberMe, setRememberMe] = useState(true);
  const [notice, setNotice] = useState(initialNotice);
  const [error, setError] = useState(initialError);
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
    setNotice('');
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
    setError('');
    setNotice('');
    window.location.href = '/api/oauth2/authorization/google';
  };

  return (
    <div className="auth-page auth-page-single">
      <div className="auth-card">
        {/* Logo Bus centered & Brand Title */}
        <div className="auth-brand-center">
          <BrandLogo size={56} className="auth-logo-badge" />
          <h2 className="auth-brand-title">
            {admin ? 'CoachBooking Quản trị' : 'CoachBooking'}
          </h2>
        </div>

        <div className="auth-card-divider" />

        <div className="auth-heading-center">
          <h1>{admin ? 'Đăng nhập Quản trị' : 'Đăng nhập'}</h1>
        </div>

        {error ? <div className="alert alert-error">{error}</div> : null}
        {notice ? <div className="alert alert-success">{notice}</div> : null}

        <form
          className="auth-form"
          onSubmit={(event) => {
            event.preventDefault();
            void handleSubmit();
          }}
        >
          {/* Tên đăng nhập */}
          <div className="auth-input-group">
            <label className="auth-input-label" htmlFor="username">
              Tên đăng nhập <span className="required-star">*</span>
            </label>
            <div className="auth-input-wrapper">
              <span className="auth-input-icon">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                  <path d="M19 21v-2a4 4 0 0 0-4-4H9a4 4 0 0 0-4 4v2"></path>
                  <circle cx="12" cy="7" r="4"></circle>
                </svg>
              </span>
              <input
                id="username"
                type="text"
                className="auth-input"
                placeholder="Nhập tên đăng nhập hoặc email"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                autoComplete="username"
                required
              />
            </div>
          </div>

          {/* Mật khẩu */}
          <div className="auth-input-group">
            <label className="auth-input-label" htmlFor="password">
              Mật khẩu <span className="required-star">*</span>
            </label>
            <div className="auth-input-wrapper">
              <span className="auth-input-icon">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                  <rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect>
                  <path d="M7 11V7a5 5 0 0 1 10 0v4"></path>
                </svg>
              </span>
              <input
                id="password"
                type={showPassword ? 'text' : 'password'}
                className="auth-input"
                placeholder="Nhập mật khẩu"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                autoComplete="current-password"
                required
              />
              <button
                type="button"
                className="auth-input-toggle"
                onClick={() => setShowPassword(!showPassword)}
                title={showPassword ? 'Ẩn mật khẩu' : 'Hiện mật khẩu'}
              >
                {showPassword ? (
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                    <path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24"></path>
                    <line x1="1" y1="1" x2="23" y2="23"></line>
                  </svg>
                ) : (
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                    <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"></path>
                    <circle cx="12" cy="12" r="3"></circle>
                  </svg>
                )}
              </button>
            </div>
          </div>

          {/* Ghi nhớ & Quên mật khẩu */}
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

          {/* Nút Đăng nhập */}
          <button type="submit" className="btn-auth-submit" disabled={busy}>
            <span>{busy ? 'Đang đăng nhập…' : 'Đăng nhập'}</span>
            {!busy && (
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                <line x1="5" y1="12" x2="19" y2="12"></line>
                <polyline points="12 5 19 12 12 19"></polyline>
              </svg>
            )}
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
              <span>Google</span>
            </button>
          </>
        )}

        <div className="auth-card-footer">
          Chưa có tài khoản?{' '}
          <Link to="/register">Đăng ký ngay</Link>
        </div>
      </div>
    </div>
  );
}
