import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import { AuthProvider, RequireRole, homePathFor, useAuth } from './auth/AuthContext';
import { AppLayout } from './components/AppLayout';
import { ToastProvider } from './components/Toast';
import { isAdminPortal } from './auth/portal';
import { Accounts } from './pages/Accounts';
import { AdminDashboard } from './pages/AdminDashboard';
import { AdminBooking } from './pages/AdminBooking';
import { BusCompanies } from './pages/BusCompanies';
import { CustomerHistory } from './pages/CustomerHistory';
import { CustomerBooking } from './pages/CustomerBooking';
import { Customers } from './pages/Customers';
import { Drivers } from './pages/Drivers';
import { Employees } from './pages/Employees';
import { ForgotPassword } from './pages/ForgotPassword';
import { Invoices } from './pages/Invoices';
import { Login } from './pages/Login';
import { Prices } from './pages/Prices';
import { Profile } from './pages/Profile';
import { Register } from './pages/Register';
import { RoutesPage } from './pages/Routes';
import { Trips } from './pages/Trips';
import { VehicleTypes } from './pages/VehicleTypes';
import { Vehicles } from './pages/Vehicles';

function HomeRedirect() {
  const { nguoiDung, loading } = useAuth();
  if (loading) {
    return (
      <div className="page-loading">
        <span className="spinner" /> Đang tải…
      </div>
    );
  }
  if (!nguoiDung) return <Navigate to="/login" replace />;
  if (isAdminPortal()) {
    return <Navigate to="/admin" replace />;
  }
  return <Navigate to={homePathFor(nguoiDung.quyen)} replace />;
}

export function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <ToastProvider>
          <Routes>
            <Route path="/" element={<HomeRedirect />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/forgot-password" element={<ForgotPassword />} />

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

            <Route path="*" element={<HomeRedirect />} />
          </Routes>
        </ToastProvider>
      </AuthProvider>
    </BrowserRouter>
  );
}
