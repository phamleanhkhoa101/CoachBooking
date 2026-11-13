import { useState } from 'react';
import { Link } from 'react-router-dom';
import { apiRequest, getErrorMessage } from '../../api/client';
import { FormField } from '../../components/common/FormField';

export function ForgotPassword() {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [error, setError] = useState('');
  const [temporaryPassword, setTemporaryPassword] = useState('');
  const [busy, setBusy] = useState(false);

  const handleSubmit = async () => {
    if (!username.trim() || !email.trim()) {
      setError('Vui lòng nhập tên đăng nhập và email.');
      return;
    }
    setBusy(true);
    setError('');
    setTemporaryPassword('');
    try {
      const data = await apiRequest<{ temporaryPassword?: string; matKhauTam?: string }>(
        '/auth/forgot-password',
        {
          method: 'POST',
          body: JSON.stringify({ username: username.trim(), email: email.trim() }),
        },
        { redirectOn401: false },
      );
      setTemporaryPassword(data?.temporaryPassword ?? data?.matKhauTam ?? '');
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
            Khôi phục<br />
            <span>tài khoản</span>
          </h2>
          <p className="auth-panel-desc">
            Nhập tên đăng nhập và email đã đăng ký. Hệ thống sẽ cấp mật khẩu tạm để bạn đăng nhập lại.
          </p>
          <ul className="auth-panel-features">
            <li>Xác minh danh tính qua email</li>
            <li>Nhận mật khẩu tạm trong vài giây</li>
            <li>Đăng nhập và đổi mật khẩu mới ngay</li>
          </ul>
        </div>
        <div className="auth-panel-footer">
          © {new Date().getFullYear()} CoachBooking · Bến xe Miền Đông
        </div>
      </aside>

      {/* Panel phải: Form */}
      <div className="auth-form-panel">
        <div className="auth-form-box">
          <span className="auth-eyebrow">Khôi phục mật khẩu</span>
          <h1>Quên mật khẩu?</h1>
          <p className="auth-subtitle">
            Nhập tên đăng nhập và email đã đăng ký để lấy mật khẩu tạm.
          </p>

          {error ? <div className="alert alert-error">{error}</div> : null}

          {temporaryPassword ? (
            <div className="alert alert-success">
              <strong>Mật khẩu tạm của bạn:</strong>
              <br />
              <span className="mono" style={{ fontSize: '1.1rem', letterSpacing: '0.08em' }}>
                {temporaryPassword}
              </span>
              <br />
              <span style={{ fontSize: '0.82rem', opacity: 0.8, marginTop: 6, display: 'block' }}>
                Hãy đăng nhập và đổi mật khẩu ngay sau đó.
              </span>
            </div>
          ) : null}

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
              required
            />
            <FormField
              label="Email"
              type="email"
              value={email}
              onChange={setEmail}
              placeholder="Email đã đăng ký"
              required
            />
            <button type="submit" className="btn btn-primary btn-block" disabled={busy}>
              {busy ? 'Đang xử lý…' : 'Lấy mật khẩu tạm'}
            </button>
          </form>

          <div className="auth-links">
            <Link to="/login">← Quay lại đăng nhập</Link>
          </div>
        </div>
      </div>
    </div>
  );
}
