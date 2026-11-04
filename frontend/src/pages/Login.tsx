import { useState } from 'react';
import { Link, Navigate, useNavigate } from 'react-router-dom';
import { getErrorMessage } from '../api/client';
import { homePathFor, useAuth } from '../auth/AuthContext';
import { FormField } from '../components/FormField';

const DEMO_ACCOUNTS = [
  { role: 'Quản trị viên', username: 'admin', password: 'Admin@123' },
  { role: 'Nhân viên', username: 'nv1', password: 'NhanVien@123' },
  { role: 'Khách hàng', username: 'kh1', password: 'KhachHang@123' },
];

export function Login() {
  const { nguoiDung, login } = useAuth();
  const navigate = useNavigate();
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
      const user = await login(tenDangNhap.trim(), matKhau);
      navigate(homePathFor(user.quyen), { replace: true });
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setBusy(false);
    }
  };

  const fillDemo = (username: string, password: string) => {
    setTenDangNhap(username);
    setMatKhau(password);
    setError('');
  };

  return (
    <div className="auth-page">
      <div className="auth-card">
        <div className="auth-brand">
          <div className="brand-logo">BX</div>
          <div>
            <strong>QLBXMD</strong>
            <span>Hệ thống quản lý bán vé xe miền Đông</span>
          </div>
        </div>

        <h1>Đăng nhập</h1>
        <p className="auth-subtitle">Sử dụng tài khoản được cấp để tiếp tục.</p>

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
          <span>
            Chưa có tài khoản? <Link to="/register">Đăng ký</Link>
          </span>
        </div>

        <div className="demo-box">
          <span className="demo-title">Tài khoản demo</span>
          <ul>
            {DEMO_ACCOUNTS.map((account) => (
              <li key={account.username}>
                <span>
                  <strong>{account.username}</strong> / {account.password}
                  <em> · {account.role}</em>
                </span>
                <button type="button" className="btn btn-ghost btn-sm" onClick={() => fillDemo(account.username, account.password)}>
                  Dùng
                </button>
              </li>
            ))}
          </ul>
        </div>
      </div>

      <aside className="auth-aside">
        <h2>Quản lý bán vé xe khách chuyên nghiệp</h2>
        <ul>
          <li>Tra cứu chuyến, chọn ghế trực quan trên sơ đồ xe.</li>
          <li>Quản lý hóa đơn, thanh toán và hủy vé nhanh chóng.</li>
          <li>Thống kê doanh thu, tuyến phổ biến theo khoảng ngày.</li>
          <li>Khách hàng tự đặt vé và xem lịch sử chuyến đi.</li>
        </ul>
      </aside>
    </div>
  );
}
