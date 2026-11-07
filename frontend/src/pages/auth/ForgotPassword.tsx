import { useState } from 'react';
import { Link } from 'react-router-dom';
import { apiRequest, getErrorMessage } from '../../api/client';
import { FormField } from '../../components/common/FormField';

export function ForgotPassword() {
  const [tenDangNhap, setTenDangNhap] = useState('');
  const [email, setEmail] = useState('');
  const [error, setError] = useState('');
  const [matKhauTam, setMatKhauTam] = useState('');
  const [busy, setBusy] = useState(false);

  const handleSubmit = async () => {
    if (!tenDangNhap.trim() || !email.trim()) {
      setError('Vui lòng nhập tên đăng nhập và email.');
      return;
    }
    setBusy(true);
    setError('');
    setMatKhauTam('');
    try {
      const data = await apiRequest<{ matKhauTam: string }>(
        '/auth/quen-mat-khau',
        {
          method: 'POST',
          body: JSON.stringify({ tenDangNhap: tenDangNhap.trim(), email: email.trim() }),
        },
        { redirectOn401: false },
      );
      setMatKhauTam(data?.matKhauTam ?? '');
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
        {matKhauTam ? (
          <div className="alert alert-success">
            Mật khẩu tạm của bạn là: <strong className="mono">{matKhauTam}</strong>
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
            value={tenDangNhap}
            onChange={setTenDangNhap}
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
