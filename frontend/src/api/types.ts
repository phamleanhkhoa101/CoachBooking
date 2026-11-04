// Kiểu dữ liệu dùng chung, bám sát docs/API.md

export type Quyen = 'Admin' | 'NhanVien' | 'KhachHang';

export type Row = Record<string, any>;

export interface NguoiDung {
  maTk: number;
  tenDangNhap: string;
  email: string;
  quyen: Quyen;
  maQuyen: number;
  maNv: number | null;
  maKh: number | null;
  hoTen: string | null;
  trangThai: boolean;
}

export interface LoginResponse {
  token: string;
  nguoiDung: NguoiDung;
}

export interface NhaXe {
  maNx: number;
  tenNx: string;
  sdt: string | null;
  email: string | null;
  trangThai: boolean;
}

export interface LoaiXe {
  maLx: number;
  tenLx: string;
  soCho: number;
  trangThai: boolean;
}

export interface Xe {
  maXe: number;
  maNx: number;
  maLx: number;
  bienSoXe: string;
  trangThai: boolean;
}

export interface TuyenDuong {
  maTuyen: number;
  diemDi: string;
  diemDen: string;
  quangDuong: number | null;
  thoiGianDuKien: string | null;
  trangThai: boolean;
}

export interface TaiXe {
  maTx: number;
  tenTx: string;
  namSinh: string | null;
  cccd: string | null;
  gioiTinh: boolean | null;
  sdt: string | null;
  diaChi: string | null;
  bangLai: string | null;
  ngayCap: string | null;
  trangThai: boolean;
}

export interface GiaVe {
  maGv: number;
  maNx: number;
  maLx: number;
  maTuyen: number;
  giaVe: number;
}

export interface ChuyenXe {
  maChuyen: number;
  maXe: number;
  bienSoXe: string;
  maNhaXe: number;
  tenNhaXe: string;
  maLoaiXe: number;
  tenLoaiXe: string;
  soCho: number;
  maTuyen: number;
  diemDi: string;
  diemDen: string;
  maTx: number;
  tenTx: string;
  ngayGioKhoiHanh: string;
  ngayGioDenDuKien: string;
  giaVeThucTe: number;
  soVeDaBan: number;
}

/** Kết quả của GET /chuyen-xe/tim (kèm số ghế đã đặt). */
export interface ChuyenTimKiem extends ChuyenXe {
  soGheDaDat?: number;
}

export interface GheResponse {
  maChuyen: number;
  tenLoaiXe: string;
  soCho: number;
  giaVe: number;
  gheDaDat: number[];
  diemDi: string;
  diemDen: string;
  ngayGioKhoiHanh: string;
}

export interface DatVeRequest {
  maChuyen: number;
  choNgois: number[];
  tenKhachHang?: string;
  sdt?: string;
  namSinh?: string;
  gioiTinh?: boolean;
  phuongThuc: string;
}

export interface DatVeResponse {
  maHd: number;
  tongTien: number;
  soVe: number;
  phuongThuc: string;
  trangThai: boolean;
}

export interface HoaDonItem {
  maHd: number;
  tenKhachHang: string;
  tenNhanVien: string | null;
  ngayLap: string;
  tongTien: number;
  trangThai: boolean;
  soVe: number;
  phuongThuc: string | null;
}

export interface VeItem {
  maVe: number;
  maChuyen: number;
  diemDi: string;
  diemDen: string;
  ngayGioKhoiHanh: string;
  choNgoi: number;
  giaVe: number;
}

export interface ThanhToanInfo {
  maTt: number;
  tongTien: number;
  ngayTt: string;
  phuongThuc: string;
}

export interface HoaDonDetail {
  maHd: number;
  tenKhachHang: string;
  sdtKhachHang: string | null;
  tenNhanVien: string | null;
  ngayLap: string;
  tongTien: number;
  trangThai: boolean;
  veXes: VeItem[];
  thanhToan: ThanhToanInfo | null;
}

/** Hóa đơn trong lịch sử khách hàng (có thể kèm danh sách vé). */
export interface HoaDonKhachHang extends HoaDonItem {
  veXes?: VeItem[];
}

export interface HoSo {
  loai: string;
  maKh: number | null;
  maNv: number | null;
  hoTen: string;
  namSinh: string | null;
  sdt: string | null;
  gioiTinh: boolean | null;
  diaChi: string | null;
  email: string;
  tenDangNhap: string;
}

export interface ThongKeNgay {
  ngay: string;
  doanhThu: number;
  soVe: number;
}

export interface TuyenPhoBien {
  tenTuyen: string;
  soVe: number;
}

export interface ThongKe {
  soKhachHang: number;
  soChuyenXe: number;
  tongDoanhThu: number;
  soVe: number;
  doanhThuTheoNgay: ThongKeNgay[];
  tuyenPhoBien: TuyenPhoBien[];
}

export interface QuyenItem {
  maQuyen: number;
  tenQuyen: string;
  moTa: string | null;
}

export interface ApiErrorBody {
  message: string;
}
