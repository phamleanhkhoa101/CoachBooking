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
          body: JSON.stringify({
            username: username.trim(),
            email: email.trim(),
          }),
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
    <div className="auth-page auth-page-single">
      <div className="auth-card">
        <div className="auth-brand">
          <div className="brand-logo">BX</div>
          <div>
            <strong>CoachBooking</strong>
            <span>Khôi phục mật khẩu</span>
          </div>
        </div>

        <h1>Quên mật khẩu</h1>
        <p className="auth-subtitle">
          Nhập tên đăng nhập và email đã đăng ký để nhận mật khẩu tạm thời.
        </p>

        {error ? <div className="alert alert-error">{error}</div> : null}
        {temporaryPassword ? (
          <div className="alert alert-success">
            Mật khẩu tạm của bạn là: <strong className="mono">{temporaryPassword}</strong>
            <br />
            <span style={{ fontSize: '0.85rem', marginTop: '4px', display: 'inline-block' }}>
              Hãy đăng nhập và đổi mật khẩu ngay sau đó.
            </span>
          </div>
        ) : null}

        <div
          style={{
            padding: '10px 14px',
            marginBottom: '16px',
            borderRadius: '8px',
            background: '#f0fdf4',
            border: '1px solid #bbf7d0',
            color: '#166534',
            fontSize: '0.82rem',
            lineHeight: '1.45',
          }}
        >
          ℹ️ Ở môi trường thử nghiệm, hệ thống sẽ hiển thị trực tiếp mật khẩu tạm thời ngay tại đây thay vì gửi qua email.
        </div>

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
