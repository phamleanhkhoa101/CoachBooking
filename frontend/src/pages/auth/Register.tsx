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
  const { nguoiDung } = useAuth();
  const [tenDangNhap, setTenDangNhap] = useState('');
  const [email, setEmail] = useState('');
  const [matKhau, setMatKhau] = useState('');
  const [xacNhanMatKhau, setXacNhanMatKhau] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [busy, setBusy] = useState(false);

  if (isAdminPortal()) {
    return <Navigate to="/login" replace />;
  }

  if (nguoiDung) {
    return <Navigate to={homePathFor(nguoiDung.quyen)} replace />;
  }

  const handleSubmit = async () => {
    if (tenDangNhap.trim().length < 6) {
      setError('Tên đăng nhập phải có ít nhất 6 ký tự.');
      return;
    }
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.trim())) {
      setError('Email không đúng định dạng.');
      return;
    }
    if (!isStrongPassword(matKhau)) {
      setError(PASSWORD_HINT);
      return;
    }
    if (matKhau !== xacNhanMatKhau) {
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
          body: JSON.stringify({
            tenDangNhap: tenDangNhap.trim(),
            email: email.trim(),
            matKhau,
            xacNhanMatKhau,
          }),
        },
        { redirectOn401: false },
      );
      setSuccess(data?.message ?? 'Đăng ký thành công.');
      setTenDangNhap('');
      setEmail('');
      setMatKhau('');
      setXacNhanMatKhau('');
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
            <span>Tạo tài khoản khách hàng</span>
          </div>
        </div>

        <h1>Đăng ký</h1>
        <p className="auth-subtitle">Sau khi đăng ký, bạn có thể tự đặt vé và quản lý lịch sử chuyến đi.</p>

        {error ? <div className="alert alert-error">{error}</div> : null}
        {success ? <div className="alert alert-success">{success}</div> : null}

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
            value={matKhau}
            onChange={setMatKhau}
            placeholder="Nhập mật khẩu"
            help={PASSWORD_HINT}
            autoComplete="new-password"
            required
          />
          <FormField
            label="Xác nhận mật khẩu"
            type="password"
            value={xacNhanMatKhau}
            onChange={setXacNhanMatKhau}
            placeholder="Nhập lại mật khẩu"
            autoComplete="new-password"
            required
          />
          <button type="submit" className="btn btn-primary btn-block" disabled={busy}>
            {busy ? 'Đang xử lý…' : 'Đăng ký'}
          </button>
        </form>

        <div className="auth-links">
          <span>
            Đã có tài khoản? <Link to="/login">Đăng nhập</Link>
          </span>
        </div>
      </div>
    </div>
  );
}
