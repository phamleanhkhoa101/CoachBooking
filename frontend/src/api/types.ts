// Kiểu dữ liệu dùng chung, bám sát docs/API.md

export type Role = 'Admin' | 'NhanVien' | 'KhachHang';
export type Quyen = Role;

export type Row = Record<string, any>;

export interface User {
  accountId?: number;
  username?: string;
  email: string;
  role?: Role;
  roleId?: number;
  employeeId?: number | null;
  customerId?: number | null;
  fullName?: string | null;
  isActive?: boolean;

  // Vietnamese fields / aliases
  maTk?: number;
  tenDangNhap?: string;
  quyen?: Role;
  maQuyen?: number;
  maNv?: number | null;
  maKh?: number | null;
  hoTen?: string | null;
  trangThai?: boolean;
}
export type NguoiDung = User;

export interface LoginResponse {
  token: string;
  user: User;
}

export interface BusCompany {
  companyId: number;
  companyName: string;
  phone: string | null;
  email: string | null;
  isActive: boolean;

  // Vietnamese fields / aliases
  maNx?: number;
  tenNx?: string;
  sdt?: string | null;
  trangThai?: boolean;
}
export type NhaXe = BusCompany;

export interface VehicleType {
  typeId: number;
  typeName: string;
  seatCapacity: number;
  isActive: boolean;

  // Vietnamese fields / aliases
  maLx?: number;
  tenLx?: string;
  soCho?: number;
  trangThai?: boolean;
}
export type LoaiXe = VehicleType;

export interface Vehicle {
  vehicleId: number;
  companyId: number;
  typeId: number;
  licensePlate: string;
  isActive: boolean;

  // Vietnamese fields / aliases
  maXe?: number;
  maNx?: number;
  maLx?: number;
  bienSoXe?: string;
  trangThai?: boolean;
}
export type Xe = Vehicle;

export interface Route {
  routeId: number;
  departure: string;
  destination: string;
  distance: number | null;
  estimatedDuration: string | null;
  isActive: boolean;

  // Vietnamese fields / aliases
  maTuyen?: number;
  diemDi?: string;
  diemDen?: string;
  quangDuong?: number | null;
  thoiGianDuKien?: string | null;
  trangThai?: boolean;
}
export type TuyenDuong = Route;

export interface Driver {
  driverId: number;
  fullName: string;
  dateOfBirth: string | null;
  citizenId: string | null;
  gender: boolean | null;
  phone: string | null;
  address: string | null;
  licenseNumber: string | null;
  issueDate: string | null;
  isActive: boolean;

  // Vietnamese fields / aliases
  maTx?: number;
  tenTx?: string;
  namSinh?: string | null;
  cccd?: string | null;
  gioiTinh?: boolean | null;
  sdt?: string | null;
  diaChi?: string | null;
  bangLai?: string | null;
  ngayCap?: string | null;
  trangThai?: boolean;
}
export type TaiXe = Driver;

export interface TicketPrice {
  priceId: number;
  companyId: number;
  typeId: number;
  routeId: number;
  price: number;

  // Vietnamese fields / aliases
  maGv?: number;
  maNx?: number;
  maLx?: number;
  maTuyen?: number;
  giaVe?: number;
}
export type GiaVe = TicketPrice;

export interface Trip {
  tripId: number;
  vehicleId: number;
  licensePlate?: string;
  companyId?: number;
  companyName?: string;
  typeId?: number;
  typeName?: string;
  seatCapacity?: number;
  routeId: number;
  departure?: string;
  destination?: string;
  driverId: number;
  driverName?: string;
  departureTime: string;
  estimatedArrivalTime?: string;
  actualPrice: number;
  ticketsSold?: number;
  bookedSeats?: number;
  bookedSeatsCount?: number;

  // Vietnamese fields / aliases
  maChuyen?: number;
  maXe?: number;
  bienSoXe?: string;
  maNhaXe?: number;
  tenNhaXe?: string;
  maLoaiXe?: number;
  tenLoaiXe?: string;
  soCho?: number;
  maTuyen?: number;
  diemDi?: string;
  diemDen?: string;
  maTx?: number;
  tenTx?: string;
  ngayGioKhoiHanh?: string;
  ngayGioDenDuKien?: string;
  giaVeThucTe?: number;
  soVeDaBan?: number;
  soGheDaDat?: number;
}
export type ChuyenXe = Trip;
export type ChuyenTimKiem = Trip;
export type TripSearchResponse = Trip;

export interface SeatMapResponse {
  tripId: number;
  vehicleTypeName: string;
  typeName?: string;
  seatCapacity: number;
  ticketPrice: number;
  price?: number;
  bookedSeats: number[];
  departure?: string;
  destination?: string;
  departureTime?: string;

  // Vietnamese fields / aliases
  maChuyen?: number;
  tenLoaiXe?: string;
  soCho?: number;
  giaVe?: number;
  choDaDat?: number[];
  gheDaDat?: number[];
  diemDi?: string;
  diemDen?: string;
  ngayGioKhoiHanh?: string;
}
export type GheResponse = SeatMapResponse;

export interface BookingRequest {
  tripId?: number;
  seatNumbers?: number[];
  customerName?: string;
  phone?: string;
  dateOfBirth?: string;
  gender?: boolean;
  paymentMethod?: string;

  // Vietnamese fields / aliases
  maChuyen?: number;
  choNgois?: number[];
  tenKhachHang?: string;
  sdt?: string;
  namSinh?: string;
  gioiTinh?: boolean;
  phuongThuc?: string;
}
export type DatVeRequest = BookingRequest;

export interface BookingResponse {
  invoiceId: number;
  totalAmount: number;
  ticketCount: number;
  paymentMethod: string;
  isPaid: boolean;

  // Vietnamese fields / aliases
  maHd?: number;
  tongTien?: number;
  soVe?: number;
  phuongThuc?: string;
  trangThai?: boolean;
}
export type DatVeResponse = BookingResponse;

export interface InvoiceItem {
  invoiceId: number;
  customerName: string;
  employeeName: string | null;
  createdAt: string;
  totalAmount: number;
  isPaid: boolean;
  ticketCount: number;
  paymentMethod: string | null;

  // Vietnamese fields / aliases
  maHd?: number;
  tenKhachHang?: string;
  tenNhanVien?: string | null;
  ngayLap?: string;
  tongTien?: number;
  trangThai?: boolean;
  soVe?: number;
  phuongThuc?: string | null;
}
export type HoaDonItem = InvoiceItem;

export interface TicketItem {
  ticketId: number;
  tripId: number;
  departure: string;
  destination: string;
  departureTime: string;
  seatNumber: number;
  price: number;
  typeName?: string;
  vehicleTypeName?: string;
  licensePlate?: string;

  // Vietnamese fields / aliases
  maVe?: number;
  maChuyen?: number;
  diemDi?: string;
  diemDen?: string;
  ngayGioKhoiHanh?: string;
  choNgoi?: number;
  soGhe?: number;
  giaVe?: number;
  gia?: number;
  tenLoaiXe?: string;
  bienSoXe?: string;
}
export type VeItem = TicketItem;

export interface PaymentInfo {
  paymentId: number;
  totalAmount: number;
  paymentDate: string;
  paymentMethod: string;

  // Vietnamese fields / aliases
  maTt?: number;
  tongTien?: number;
  ngayTt?: string;
  phuongThuc?: string;
}
export type ThanhToanInfo = PaymentInfo;

export interface InvoiceDetail {
  invoiceId: number;
  customerName: string;
  customerPhone: string | null;
  phone?: string | null;
  soDienThoai?: string | null;
  employeeName: string | null;
  createdAt: string;
  totalAmount: number;
  isPaid: boolean;
  tickets: TicketItem[];
  payment: PaymentInfo | null;

  // Vietnamese fields / aliases
  maHd?: number;
  tenKhachHang?: string;
  sdtKhachHang?: string | null;
  tenNhanVien?: string | null;
  ngayLap?: string;
  tongTien?: number;
  trangThai?: boolean;
  veXes?: TicketItem[];
  chiTietVe?: TicketItem[];
  thanhToan?: PaymentInfo | null;
}
export type HoaDonDetail = InvoiceDetail;
export type HoaDonKhachHang = InvoiceItem & { veXes?: TicketItem[]; tickets?: TicketItem[]; ngayTao?: string };

export interface Profile {
  type: string;
  customerId: number | null;
  employeeId: number | null;
  fullName: string;
  dateOfBirth: string | null;
  phone: string | null;
  gender: boolean | null;
  address: string | null;
  email: string;
  username: string;

  // Vietnamese fields / aliases
  loai?: string;
  maKh?: number | null;
  maNv?: number | null;
  hoTen?: string;
  namSinh?: string | null;
  ngaySinh?: string | null;
  sdt?: string | null;
  gioiTinh?: boolean | null;
  diaChi?: string | null;
  tenDangNhap?: string;
}
export type HoSo = Profile;

export interface DailyRevenue {
  date: string;
  revenue: number;
  ticketCount: number;

  // Vietnamese fields / aliases
  ngay?: string;
  doanhThu?: number;
  soVe?: number;
}
export type ThongKeNgay = DailyRevenue;

export interface PopularRoute {
  routeName: string;
  ticketCount: number;

  // Vietnamese fields / aliases
  tenTuyen?: string;
  soVe?: number;
}
export type TuyenPhoBien = PopularRoute;

export interface Statistics {
  customerCount: number;
  tripCount: number;
  totalRevenue: number;
  ticketCount: number;
  dailyRevenue: DailyRevenue[];
  popularRoutes: PopularRoute[];

  // Vietnamese fields / aliases
  soKhachHang?: number;
  soChuyenXe?: number;
  tongDoanhThu?: number;
  soVe?: number;
  doanhThuTheoNgay?: DailyRevenue[];
  tuyenPhoBien?: PopularRoute[];
}
export type ThongKe = Statistics;

export interface RoleItem {
  roleId: number;
  roleName: string;
  description: string | null;

  // Vietnamese fields / aliases
  maQuyen?: number;
  tenQuyen?: string;
  moTa?: string | null;
}
export type QuyenItem = RoleItem;

export interface ApiErrorBody {
  message: string;
  fieldErrors?: Record<string, string>;
}
