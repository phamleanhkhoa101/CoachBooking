import { Link, Outlet } from 'react-router-dom';
import { CustomerNavbar } from './CustomerNavbar';
import './CustomerLayout.css';

export function CustomerLayout() {
  return (
    <div className="customer-layout">
      {/* 1. TOP NAVBAR (Shared across all customer pages) */}
      <CustomerNavbar />

      {/* 2. BODY CONTENT */}
      <main className="customer-main-content">
        <Outlet />
      </main>

      {/* 3. FOOTER */}
      <footer className="customer-footer">
        <div className="footer-container">
          <div className="footer-cols">
            <div className="footer-col-main">
              <div className="footer-brand">
                <div className="brand-badge-box">BX</div>
                <div className="brand-text-col">
                  <span className="brand-title">CoachBooking</span>
                  <span className="brand-sub">Bến xe Miền Đông Mới</span>
                </div>
              </div>
              <p className="footer-desc">
                Cổng thông tin đặt vé xe khách liên tỉnh trực tuyến hiện đại, kết nối các nhà xe uy tín tại Bến xe Miền Đông. Đảm bảo đúng chỗ 100%, thanh toán linh hoạt VietQR 24/7.
              </p>
              <div className="footer-contact-info">
                <div className="contact-item">
                  <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z" />
                    <circle cx="12" cy="10" r="3" />
                  </svg>
                  <span>501 Hoàng Hữu Nam, P. Long Bình, TP. Thủ Đức, TP. Hồ Chí Minh</span>
                </div>
                <div className="contact-item">
                  <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6 19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 22 16.92z" />
                  </svg>
                  <span>Hotline: 1900 6067 - 028 3511 6858</span>
                </div>
              </div>
            </div>

            <div className="footer-col">
              <h4>Tuyến đường phổ biến</h4>
              <ul>
                <li><Link to="/tim-chuyen?departure=TP+HCM&destination=Đà+Lạt">TP.HCM ➔ Đà Lạt</Link></li>
                <li><Link to="/tim-chuyen?departure=TP+HCM&destination=Vũng+Tàu">TP.HCM ➔ Vũng Tàu</Link></li>
                <li><Link to="/tim-chuyen?departure=TP+HCM&destination=Nha+Trang">TP.HCM ➔ Nha Trang</Link></li>
                <li><Link to="/tim-chuyen?departure=TP+HCM&destination=Cần+Thơ">TP.HCM ➔ Cần Thơ</Link></li>
              </ul>
            </div>

            <div className="footer-col">
              <h4>Dịch vụ & Tiện ích</h4>
              <ul>
                <li><Link to="/tim-chuyen">Tra cứu lịch trình xe</Link></li>
                <li><Link to="/tra-cuu-ve">Tra cứu hóa đơn & vé</Link></li>
                <li><Link to="/ve-cua-toi">Quản lý vé đã đặt</Link></li>
                <li><a href="#rules">Quy định bến bãi & hành lý</a></li>
              </ul>
            </div>

            <div className="footer-col">
              <h4>Chấp nhận thanh toán</h4>
              <p className="pay-methods-desc">Hỗ trợ đa dạng phương thức thanh toán an toàn bảo mật:</p>
              <div className="payment-badges-grid">
                <span className="pay-badge">VietQR</span>
                <span className="pay-badge">Chuyển khoản</span>
                <span className="pay-badge">Tiền mặt</span>
                <span className="pay-badge">Trả sau 24h</span>
              </div>
            </div>
          </div>

          <div className="footer-bottom">
            <p>© 2026 CoachBooking · Tổng công ty Cơ khí Giao thông Vận tải Sài Gòn (SAMCO). Giữ toàn quyền.</p>
          </div>
        </div>
      </footer>
    </div>
  );
}
