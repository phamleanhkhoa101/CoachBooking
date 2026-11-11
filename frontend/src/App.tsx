import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import { AuthProvider, RequireRole, useAuth } from './auth/AuthContext';
import { AppLayout } from './components/layout/AppLayout';
import { CustomerLayout } from './components/layout/CustomerLayout';
import { ToastProvider } from './components/common/Toast';
import { isAdminPortal } from './auth/portal';
import { Accounts } from './pages/admin/system/Accounts';
import { AdminDashboard } from './pages/admin/AdminDashboard';
import { AdminBooking } from './pages/admin/AdminBooking';
import { BusCompanies } from './pages/admin/operations/BusCompanies';
import { CustomerHistory } from './pages/customer/CustomerHistory';
import { CustomerBooking } from './pages/customer/CustomerBooking';
import { Customers } from './pages/admin/system/Customers';
import { Drivers } from './pages/admin/operations/Drivers';
import { Employees } from './pages/admin/system/Employees';
import { ForgotPassword } from './pages/auth/ForgotPassword';
import { Home } from './pages/customer/Home';
import { Invoices } from './pages/admin/Invoices';
import { Login } from './pages/auth/Login';
import { Prices } from './pages/admin/operations/Prices';
import { Profile } from './pages/admin/Profile';
import { Register } from './pages/auth/Register';
import { RoutesPage } from './pages/admin/operations/Routes';
import { Trips } from './pages/admin/operations/Trips';
import { VehicleTypes } from './pages/admin/operations/VehicleTypes';
import { Vehicles } from './pages/admin/operations/Vehicles';
import { TripSearchPage } from './pages/customer/TripSearchPage';
import { SeatSelectionPage } from './pages/customer/SeatSelectionPage';
import { CheckoutPage } from './pages/customer/CheckoutPage';
import { ETicketPage } from './pages/customer/ETicketPage';
import { TicketLookupPage } from './pages/customer/TicketLookupPage';
import { CustomerTicketsPage } from './pages/customer/CustomerTicketsPage';

function HomeRoute() {
  const { nguoiDung, loading } = useAuth();
  if (loading) {
    return (
      <div className="page-loading">
        <span className="spinner" /> Đang tải…
      </div>
    );
  }
  // Nếu là cổng Admin (port 3001) thì chuyển hướng vào /admin hoặc /login
  if (isAdminPortal()) {
    if (!nguoiDung) return <Navigate to="/login" replace />;
    return <Navigate to="/admin" replace />;
  }
  // Cổng Khách hàng (port 3000): hiển thị Trang chủ (Home)
  return <Home />;
}

export function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <ToastProvider>
          <Routes>
            <Route path="/" element={<HomeRoute />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/forgot-password" element={<ForgotPassword />} />

            {/* Cổng Đặt vé Khách hàng & Dịch vụ bến xe */}
            <Route element={<CustomerLayout />}>
              <Route path="/tim-chuyen" element={<TripSearchPage />} />
              <Route path="/dat-ve/chon-ghe/:tripId" element={<SeatSelectionPage />} />
              <Route path="/dat-ve/thanh-toan" element={<CheckoutPage />} />
              <Route path="/ve-dien-tu/:invoiceId" element={<ETicketPage />} />
              <Route path="/tra-cuu-ve" element={<TicketLookupPage />} />
              <Route path="/ve-cua-toi" element={<CustomerTicketsPage />} />
            </Route>

            <Route
              path="/admin"
              element={
                <RequireRole roles={['Admin', 'NhanVien']}>
                  <AppLayout />
                </RequireRole>
              }
            >
              <Route index element={<AdminDashboard />} />
              <Route path="ban-ve" element={<AdminBooking />} />
              <Route path="hoa-don" element={<Invoices />} />
              <Route path="khach-hang" element={<Customers />} />
              <Route path="chuyen-xe" element={<Trips />} />
              <Route path="nha-xe" element={<BusCompanies />} />
              <Route path="loai-xe" element={<VehicleTypes />} />
              <Route path="xe" element={<Vehicles />} />
              <Route path="tuyen-duong" element={<RoutesPage />} />
              <Route path="gia-ve" element={<Prices />} />
              <Route path="tai-xe" element={<Drivers />} />
              <Route path="ho-so" element={<Profile />} />
              <Route
                path="nhan-vien"
                element={
                  <RequireRole roles={['Admin']}>
                    <Employees />
                  </RequireRole>
                }
              />
              <Route
                path="tai-khoan"
                element={
                  <RequireRole roles={['Admin']}>
                    <Accounts />
                  </RequireRole>
                }
              />
              <Route path="*" element={<Navigate to="/admin" replace />} />
            </Route>

            <Route
              path="/app"
              element={
                <RequireRole roles={['KhachHang']}>
                  <AppLayout />
                </RequireRole>
              }
            >
              <Route index element={<Navigate to="/app/dat-ve" replace />} />
              <Route path="dat-ve" element={<CustomerBooking />} />
              <Route path="lich-su" element={<CustomerHistory />} />
              <Route path="ho-so" element={<Profile />} />
              <Route path="*" element={<Navigate to="/app/dat-ve" replace />} />
            </Route>

            <Route path="*" element={<HomeRoute />} />
          </Routes>
        </ToastProvider>
      </AuthProvider>
    </BrowserRouter>
  );
}
