import { useState } from 'react';
import { Link } from 'react-router-dom';
import { apiRequest, getErrorMessage } from '../../api/client';

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
        {/* Brand Header Centered */}
        <div className="auth-brand-center">
          <div className="auth-logo-badge">
            <svg width="30" height="30" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
              <path d="M8 6v6"></path>
              <path d="M15 6v6"></path>
              <path d="M2 12h19.6"></path>
              <path d="M18 18h3s.5-1.7.8-2.8c.1-.4.2-.8.2-1.2 0-.4-.1-.8-.2-1.2l-1.4-5C20.1 6.8 19.1 6 18 6H4C2.9 6 1.9 6.8 1.6 7.8l-1.4 5c-.1.4-.2.8-.2 1.2 0 .4.1.8.2 1.2.3 1.1.8 2.8.8 2.8h3"></path>
              <circle cx="7" cy="18" r="2"></circle>
              <path d="M9 18h5"></path>
              <circle cx="16" cy="18" r="2"></circle>
            </svg>
          </div>
          <h2 className="auth-brand-title">CoachBooking</h2>
        </div>

        <div className="auth-card-divider" />

        <div className="auth-heading-center">
          <h1>Quên mật khẩu</h1>
        </div>

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


        <form
          className="auth-form"
          onSubmit={(event) => {
            event.preventDefault();
            void handleSubmit();
          }}
        >
          {/* Tên đăng nhập */}
          <div className="auth-input-group">
            <label className="auth-input-label" htmlFor="forgot-username">
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
                id="forgot-username"
                type="text"
                className="auth-input"
                placeholder="Nhập tên đăng nhập"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                required
              />
            </div>
          </div>

          {/* Email */}
          <div className="auth-input-group">
            <label className="auth-input-label" htmlFor="forgot-email">
              Email <span className="required-star">*</span>
            </label>
            <div className="auth-input-wrapper">
              <span className="auth-input-icon">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                  <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"></path>
                  <polyline points="22,6 12,13 2,6"></polyline>
                </svg>
              </span>
              <input
                id="forgot-email"
                type="email"
                className="auth-input"
                placeholder="Email đã đăng ký"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>
          </div>

          <button type="submit" className="btn-auth-submit" disabled={busy}>
            <span>{busy ? 'Đang xử lý…' : 'Lấy mật khẩu tạm'}</span>
            {!busy && (
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                <line x1="5" y1="12" x2="19" y2="12"></line>
                <polyline points="12 5 19 12 12 19"></polyline>
              </svg>
            )}
          </button>
        </form>

        <div className="auth-card-footer">
          <Link to="/login">← Quay lại đăng nhập</Link>
        </div>
      </div>
    </div>
  );
}
