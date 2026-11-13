import { useState } from 'react';
import { Link, Navigate } from 'react-router-dom';
import { apiRequest, getErrorMessage } from '../../api/client';
import { homePathFor, useAuth } from '../../auth/AuthContext';
import { isAdminPortal } from '../../auth/portal';
import { FormField } from '../../components/common/FormField';

const PASSWORD_HINT =
  'Tối thiểu 8 ký tự, gồm chữ hoa, chữ thường, chữ số và ký tự đặc biệt.';

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
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [busy, setBusy] = useState(false);

  if (isAdminPortal()) return <Navigate to="/login" replace />;
  if (user) return <Navigate to={homePathFor(user.role)} replace />;

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
    setBusy(true);
    setError('');
    setSuccess('');
    try {
      const data = await apiRequest<{ message: string }>(
        '/auth/register',
        {
          method: 'POST',
          body: JSON.stringify({ username: username.trim(), email: email.trim(), password, confirmPassword }),
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

  return (
    <div className="auth-root">
      {/* Panel trái */}
      <aside className="auth-panel">
        <div className="auth-panel-top">
          <div className="auth-panel-logo">BX</div>
          <div>
            <div className="auth-panel-name">CoachBooking</div>
            <div className="auth-panel-sub">Bến xe Miền Đông · SAMCO</div>
          </div>
        </div>
        <div className="auth-panel-body">
          <h2 className="auth-panel-headline">
            Tạo tài khoản<br />
            <span>miễn phí</span>
          </h2>
          <p className="auth-panel-desc">
            Chỉ mất một phút để có tài khoản và bắt đầu đặt vé xe khách trực tuyến ngay hôm nay.
          </p>
          <ul className="auth-panel-features">
            <li>Đặt vé online 24/7, không xếp hàng</li>
            <li>Chọn chỗ ngồi ưa thích trên sơ đồ xe</li>
            <li>Nhận xác nhận vé ngay tức thì</li>
            <li>Quản lý & tra cứu vé mọi lúc, mọi nơi</li>
          </ul>
        </div>
        <div className="auth-panel-footer">
          © {new Date().getFullYear()} CoachBooking · Bến xe Miền Đông
        </div>
      </aside>

      {/* Panel phải: Form */}
      <div className="auth-form-panel">
        <div className="auth-form-box">
          <span className="auth-eyebrow">Khách hàng mới</span>
          <h1>Đăng ký</h1>
          <p className="auth-subtitle">
            Điền thông tin bên dưới để tạo tài khoản đặt vé.
          </p>

          {error ? <div className="alert alert-error">{error}</div> : null}
          {success ? <div className="alert alert-success">{success}</div> : null}

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
            <button type="submit" className="btn btn-primary btn-block" disabled={busy}>
              {busy ? 'Đang tạo tài khoản…' : 'Tạo tài khoản'}
            </button>
          </form>

          <div className="auth-links">
            <span>
              Đã có tài khoản? <Link to="/login">Đăng nhập</Link>
            </span>
          </div>
        </div>
      </div>
    </div>
  );
}
