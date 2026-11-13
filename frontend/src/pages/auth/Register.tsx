import { useState } from 'react';
import { Link, Navigate } from 'react-router-dom';
import { apiRequest, getErrorMessage } from '../../api/client';
import { homePathFor, useAuth } from '../../auth/AuthContext';
import { isAdminPortal } from '../../auth/portal';
import { FormField } from '../../components/common/FormField';

const PASSWORD_HINT = 'Tối thiểu 8 ký tự, gồm chữ hoa, chữ thường, chữ số và ký tự đặc biệt.';

function isStrongPassword(value: string): boolean {
  return (
    value.length >= 8 &&
    /[A-Z]/.test(value) &&
    /[a-z]/.test(value) &&
    /[0-9]/.test(value) &&
    /[^A-Za-z0-9]/.test(value)
  );
}

export function Register() {
  const { user } = useAuth();
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [agreeTerms, setAgreeTerms] = useState(true);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [busy, setBusy] = useState(false);

  if (isAdminPortal()) {
    return <Navigate to="/login" replace />;
  }

  if (user) {
    const role = user.role;
    return <Navigate to={homePathFor(role)} replace />;
  }

  const handleSubmit = async () => {
    if (username.trim().length < 6) {
      setError('Tên đăng nhập phải có ít nhất 6 ký tự.');
      return;
    }
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.trim())) {
      setError('Email không đúng định dạng.');
      return;
    }
    if (!isStrongPassword(password)) {
      setError(PASSWORD_HINT);
      return;
    }
    if (password !== confirmPassword) {
      setError('Mật khẩu xác nhận không khớp.');
      return;
    }
    if (!agreeTerms) {
      setError('Vui lòng đồng ý với Điều khoản dịch vụ & Chính sách bảo mật.');
      return;
    }

    setBusy(true);
    setError('');
    setSuccess('');
    try {
      const data = await apiRequest<{ message: string }>(
        '/auth/register',
        {
          method: 'POST',
          body: JSON.stringify({
            username: username.trim(),
            email: email.trim(),
            password,
            confirmPassword,
          }),
        },
        { redirectOn401: false },
      );
      setSuccess(data?.message ?? 'Đăng ký thành công! Bạn có thể đăng nhập ngay.');
      setUsername('');
      setEmail('');
      setPassword('');
      setConfirmPassword('');
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setBusy(false);
    }
  };

  const handleGoogleSignUp = () => {
    setError('Tính năng liên kết Google đang được phát triển. Vui lòng đăng ký qua form bên dưới.');
  };

  return (
    <div className="auth-page auth-page-single">
      <div className="auth-card auth-card-wide">
        <div className="auth-brand">
          <div className="brand-logo">BX</div>
          <div>
            <strong>CoachBooking</strong>
            <span>Bến xe Miền Đông Mới • SAMCO</span>
          </div>
        </div>

        <h1>Đăng ký tài khoản</h1>
        <p className="auth-subtitle">
          Tạo tài khoản để đặt vé trực tuyến, tích điểm và quản lý lịch sử chuyến đi.
        </p>

        {error ? <div className="alert alert-error">{error}</div> : null}
        {success ? <div className="alert alert-success">{success}</div> : null}

        <button
          type="button"
          className="btn-google-full"
          onClick={handleGoogleSignUp}
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
          <span>Đăng ký nhanh bằng Google</span>
        </button>

        <div className="auth-divider">hoặc đăng ký bằng tài khoản mới</div>

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
            placeholder="Ít nhất 6 ký tự"
            autoComplete="username"
            required
          />
          <FormField
            label="Email"
            type="email"
            value={email}
            onChange={setEmail}
            placeholder="ban@example.com"
            autoComplete="email"
            required
          />
          <FormField
            label="Mật khẩu"
            type="password"
            value={password}
            onChange={setPassword}
            placeholder="Nhập mật khẩu"
            help={PASSWORD_HINT}
            autoComplete="new-password"
            required
          />
          <FormField
            label="Xác nhận mật khẩu"
            type="password"
            value={confirmPassword}
            onChange={setConfirmPassword}
            placeholder="Nhập lại mật khẩu"
            autoComplete="new-password"
            required
          />

          <div style={{ marginTop: '-4px' }}>
            <label className="auth-checkbox-label">
              <input
                type="checkbox"
                checked={agreeTerms}
                onChange={(e) => setAgreeTerms(e.target.checked)}
              />
              <span style={{ fontSize: '0.84rem', lineHeight: '1.4' }}>
                Tôi đồng ý với Điều khoản dịch vụ và Chính sách bảo mật của Bến xe Miền Đông
              </span>
            </label>
          </div>

          <button type="submit" className="btn btn-primary btn-block" disabled={busy}>
            {busy ? 'Đang tạo tài khoản…' : 'Tạo tài khoản'}
          </button>
        </form>

        <div className="auth-links">
          <span>
            Đã có tài khoản? <Link to="/login">Đăng nhập ngay</Link>
          </span>
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
