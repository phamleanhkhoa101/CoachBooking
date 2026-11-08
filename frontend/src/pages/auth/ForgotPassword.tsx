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
            tenDangNhap: username.trim(),
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
          Nhập tên đăng nhập và email đã đăng ký. Ở môi trường demo, hệ thống trả mật khẩu tạm trực
          tiếp thay vì gửi email.
        </p>

        {error ? <div className="alert alert-error">{error}</div> : null}
        {temporaryPassword ? (
          <div className="alert alert-success">
            Mật khẩu tạm của bạn là: <strong className="mono">{temporaryPassword}</strong>
            <br />
            Hãy đăng nhập và đổi mật khẩu ngay sau đó.
          </div>
        ) : null}

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
          <Link to="/login">← Về trang đăng nhập</Link>
        </div>
      </div>
    </div>
  );
}
