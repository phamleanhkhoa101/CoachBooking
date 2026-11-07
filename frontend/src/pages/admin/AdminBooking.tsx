import { Booking } from '../customer/Booking';

/** Màn hình bán vé tại quầy cho nhân viên / quản trị viên. */
export function AdminBooking() {
  return <Booking mode="nhanvien" />;
}
