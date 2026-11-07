/**
 * Xác định phiên bản cổng hiện tại dựa trên port hoặc query parameter.
 * - Port 3001 (hoặc hostname bắt đầu bằng admin / query portal=admin) là Cổng Quản trị (Admin/Nhân viên).
 * - Port 3000 (hoặc mặc định) là Cổng Khách hàng.
 */
export function isAdminPortal(): boolean {
  if (typeof window === 'undefined') return false;
  return (
    window.location.port === '3001' ||
    window.location.hostname.startsWith('admin') ||
    new URLSearchParams(window.location.search).get('portal') === 'admin'
  );
}

