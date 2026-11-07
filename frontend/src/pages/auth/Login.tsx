import { useState } from 'react';
import { Link, Navigate, useNavigate } from 'react-router-dom';
import { getErrorMessage } from '../../api/client';
import { homePathFor, useAuth } from '../../auth/AuthContext';
import { isAdminPortal } from '../../auth/portal';
import { FormField } from '../../components/common/FormField';

export function Login() {
  const { nguoiDung, login } = useAuth();
  const navigate = useNavigate();
  const admin = isAdminPortal();

  const [tenDangNhap, setTenDangNhap] = useState('');
  const [matKhau, setMatKhau] = useState('');
  const [error, setError] = useState('');
  const [busy, setBusy] = useState(false);

  if (nguoiDung) {
    return <Navigate to={homePathFor(nguoiDung.quyen)} replace />;
  }

  const handleSubmit = async () => {
    if (!tenDangNhap.trim() || !matKhau) {
      setError('Vui lòng nhập tên đăng nhập và mật khẩu.');
      return;
    }
    setBusy(true);
    setError('');
    try {
      const user = await login(tenDangNhap.trim(), matKhau, admin ? 'admin' : 'user');
      navigate(homePathFor(user.quyen), { replace: true });
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
          <div className="brand-logo">{admin ? 'AD' : 'BX'}</div>
          <div>
            <strong>{admin ? 'CoachBooking Quản trị' : 'CoachBooking'}</strong>
            <span>
              {admin
                ? 'Hệ thống quản lý bán vé xe miền Đông'
                : 'Hệ thống quản lý bán vé xe miền Đông'}
            </span>
          </div>
        </div>

        <h1>{admin ? 'Đăng nhập Quản trị' : 'Đăng nhập'}</h1>
        <p className="auth-subtitle">
          {admin
            ? 'Sử dụng tài khoản quản trị hoặc nhân viên để tiếp tục.'
            : 'Sử dụng tài khoản khách hàng để tiếp tục.'}
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
            value={tenDangNhap}
            onChange={setTenDangNhap}
            placeholder="Nhập tên đăng nhập"
            autoComplete="username"
            required
          />
          <FormField
            label="Mật khẩu"
            value={matKhau}
            onChange={setMatKhau}
            type="password"
            placeholder="Nhập mật khẩu"
            autoComplete="current-password"
            required
          />
          <button type="submit" className="btn btn-primary btn-block" disabled={busy}>
            {busy ? 'Đang đăng nhập…' : 'Đăng nhập'}
          </button>
        </form>

        <div className="auth-links">
          <Link to="/forgot-password">Quên mật khẩu?</Link>
          {!admin ? (
            <span>
              Chưa có tài khoản? <Link to="/register">Đăng ký</Link>
            </span>
          ) : null}
        </div>
      </div>
    </div>
  );
}
