import { useCallback, useEffect, useMemo, useState } from 'react';
import { ApiError, api, buildQuery, getErrorMessage } from '../../api/client';
import type {
  ChuyenTimKiem,
  DatVeRequest,
  DatVeResponse,
  GheResponse,
  HoSo,
  TuyenDuong,
} from '../../api/types';
import { useAuth } from '../../auth/AuthContext';
import { DataTable, type DataTableColumn } from '../../components/table/DataTable';
import { FormField } from '../../components/common/FormField';
import { SeatMap } from './components/SeatMap';
import { useToast } from '../../components/common/Toast';
import { formatDate, formatDateTime, formatTime, formatVnd, todayISO } from '../../utils/format';

const STAFF_METHODS = ['Tiền mặt', 'Chuyển khoản', 'Thẻ ATM'];

interface BookingProps {
  mode: 'nhanvien' | 'khachhang';
}

export function Booking({ mode }: BookingProps) {
  const isStaff = mode === 'nhanvien';
  const { nguoiDung } = useAuth();
  const { showToast } = useToast();

  const [tuyenList, setTuyenList] = useState<TuyenDuong[]>([]);
  const [diemDi, setDiemDi] = useState('');
  const [diemDen, setDiemDen] = useState('');
  const [ngay, setNgay] = useState(todayISO());
  const [tuGio, setTuGio] = useState('');
  const [denGio, setDenGio] = useState('');

  const [chuyens, setChuyens] = useState<ChuyenTimKiem[]>([]);
  const [searched, setSearched] = useState(false);
  const [loadingChuyen, setLoadingChuyen] = useState(false);

  const [selectedChuyen, setSelectedChuyen] = useState<ChuyenTimKiem | null>(null);
  const [ghe, setGhe] = useState<GheResponse | null>(null);
  const [loadingGhe, setLoadingGhe] = useState(false);
  const [selectedSeats, setSelectedSeats] = useState<number[]>([]);

  const [hoSo, setHoSo] = useState<HoSo | null>(null);
  const [tenKhachHang, setTenKhachHang] = useState('');
  const [sdt, setSdt] = useState('');
  const [namSinh, setNamSinh] = useState('');
  const [gioiTinh, setGioiTinh] = useState('true');
  const [phuongThuc, setPhuongThuc] = useState('Tiền mặt');
  const [submitting, setSubmitting] = useState(false);
  const [bookingError, setBookingError] = useState('');
  const [result, setResult] = useState<DatVeResponse | null>(null);

  const methods = useMemo(
    () => (isStaff ? STAFF_METHODS : [...STAFF_METHODS, 'Thanh toán sau']),
    [isStaff],
  );

  useEffect(() => {
    void (async () => {
      try {
        const list = await api.get<TuyenDuong[]>('/tuyen-duong');
        setTuyenList(Array.isArray(list) ? list : []);
      } catch (error) {
        showToast(getErrorMessage(error), 'error');
      }
    })();
  }, [showToast]);

  useEffect(() => {
    if (isStaff) return;
    void (async () => {
      try {
        setHoSo(await api.get<HoSo>('/ho-so'));
      } catch (error) {
        showToast(getErrorMessage(error), 'error');
      }
    })();
  }, [isStaff, showToast]);

  const diemDiOptions = useMemo(() => {
    const unique = Array.from(new Set(tuyenList.map((item) => item.diemDi).filter(Boolean)));
    return unique.map((value) => ({ value, label: value }));
  }, [tuyenList]);

  const diemDenOptions = useMemo(() => {
    const unique = Array.from(
      new Set(
        tuyenList
          .filter((item) => (diemDi ? item.diemDi === diemDi : true))
          .map((item) => item.diemDen)
          .filter(Boolean),
      ),
    );
    return unique.map((value) => ({ value, label: value }));
  }, [tuyenList, diemDi]);

  const handleSearch = useCallback(async () => {
    setLoadingChuyen(true);
    try {
      const data = await api.get<ChuyenTimKiem[]>(
        `/chuyen-xe/tim${buildQuery({ diemDi, diemDen, ngay, tuGio, denGio })}`,
      );
      setChuyens(Array.isArray(data) ? data : []);
      setSearched(true);
    } catch (error) {
      setChuyens([]);
      setSearched(true);
      showToast(getErrorMessage(error), 'error');
    } finally {
      setLoadingChuyen(false);
    }
  }, [diemDi, diemDen, ngay, tuGio, denGio, showToast]);

  const loadGhe = useCallback(
    async (maChuyen: number) => {
      setLoadingGhe(true);
      try {
        setGhe(await api.get<GheResponse>(`/chuyen-xe/${maChuyen}/ghe`));
      } catch (error) {
        setGhe(null);
        showToast(getErrorMessage(error), 'error');
      } finally {
        setLoadingGhe(false);
      }
    },
    [showToast],
  );

  const chooseChuyen = async (chuyen: ChuyenTimKiem) => {
    setSelectedChuyen(chuyen);
    setSelectedSeats([]);
    setBookingError('');
    setResult(null);
    await loadGhe(chuyen.maChuyen);
  };

  const toggleSeat = (seat: number) => {
    setSelectedSeats((prev) =>
      prev.includes(seat) ? prev.filter((item) => item !== seat) : [...prev, seat].sort((a, b) => a - b),
    );
  };

  const giaVe = ghe?.giaVe ?? selectedChuyen?.giaVeThucTe ?? 0;
  const tongTien = giaVe * selectedSeats.length;

  // Khách vừa đăng ký có thể chưa có hồ sơ khách hàng (maKh = null):
  // khi đó phải nhập thông tin để backend tạo hồ sơ ở lần đặt vé đầu tiên.
  const hasCustomerProfile =
    !isStaff && ((nguoiDung?.maKh ?? null) !== null || (hoSo?.maKh ?? null) !== null);
  const needCustomerInfo = isStaff || !hasCustomerProfile;

  const handleDatVe = async () => {
    if (!selectedChuyen) {
      setBookingError('Vui lòng chọn chuyến xe trước.');
      return;
    }
    if (selectedSeats.length === 0) {
      setBookingError('Vui lòng chọn ít nhất một ghế trên sơ đồ.');
      return;
    }
    if (needCustomerInfo) {
      if (!tenKhachHang.trim()) {
        setBookingError('Vui lòng nhập tên khách hàng.');
        return;
      }
      if (!/^\d{10,11}$/.test(sdt.trim())) {
        setBookingError('Số điện thoại phải gồm 10 đến 11 chữ số.');
        return;
      }
      if (!namSinh) {
        setBookingError('Vui lòng nhập ngày sinh khách hàng.');
        return;
      }
    }

    const body: DatVeRequest = {
      maChuyen: selectedChuyen.maChuyen,
      choNgois: selectedSeats,
      phuongThuc,
    };
    if (needCustomerInfo) {
      body.tenKhachHang = tenKhachHang.trim();
      body.sdt = sdt.trim();
      body.namSinh = namSinh;
      body.gioiTinh = gioiTinh === 'true';
    }

    setSubmitting(true);
    setBookingError('');
    try {
      const res = await api.post<DatVeResponse>('/dat-ve', body);
      setResult(res);
      showToast(`Đặt vé thành công, hóa đơn #${res.maHd}.`);
      setSelectedSeats([]);
      if (needCustomerInfo) {
        setTenKhachHang('');
        setSdt('');
        setNamSinh('');
        setGioiTinh('true');
      }
      if (!isStaff && !hasCustomerProfile) {
        // Hồ sơ khách hàng vừa được tạo ở lần đặt vé đầu tiên.
        try {
          setHoSo(await api.get<HoSo>('/ho-so'));
        } catch {
          // Không chặn luồng đặt vé nếu tải lại hồ sơ thất bại.
        }
      }
      await loadGhe(selectedChuyen.maChuyen);
      await handleSearch();
    } catch (error) {
      const message = getErrorMessage(error);
      setBookingError(message);
      showToast(message, 'error');
      if (error instanceof ApiError && error.status === 409) {
        setSelectedSeats([]);
        await loadGhe(selectedChuyen.maChuyen);
      }
    } finally {
      setSubmitting(false);
    }
  };

  const columns: DataTableColumn<ChuyenTimKiem>[] = [
    {
      key: 'gioDi',
      label: 'Giờ đi',
      width: '110px',
      render: (row) => (
        <div className="cell-stack">
          <strong>{formatTime(row.ngayGioKhoiHanh)}</strong>
          <span className="text-muted">{formatDate(row.ngayGioKhoiHanh)}</span>
        </div>
      ),
    },
    {
      key: 'tuyen',
      label: 'Tuyến',
      render: (row) => (
        <div className="cell-stack">
          <strong>
            {row.diemDi} → {row.diemDen}
          </strong>
          <span className="text-muted">Đến dự kiến {formatTime(row.ngayGioDenDuKien)}</span>
        </div>
      ),
    },
    { key: 'tenNhaXe', label: 'Nhà xe' },
    { key: 'tenLoaiXe', label: 'Loại xe' },
    { key: 'soCho', label: 'Số chỗ', align: 'right', width: '80px' },
    {
      key: 'soGheDaDat',
      label: 'Đã đặt',
      align: 'right',
      width: '90px',
      render: (row) => {
        const booked = row.soGheDaDat ?? row.soVeDaBan ?? 0;
        return (
          <span className={booked >= (row.soCho ?? 0) ? 'text-danger' : undefined}>
            {booked}/{row.soCho}
          </span>
        );
      },
    },
    {
      key: 'giaVeThucTe',
      label: 'Giá vé',
      align: 'right',
      render: (row) => <strong className="text-primary">{formatVnd(row.giaVeThucTe)}</strong>,
    },
  ];

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>{isStaff ? 'Bán vé tại quầy' : 'Đặt vé trực tuyến'}</h1>
          <p className="page-subtitle">
            {isStaff
              ? 'Tìm chuyến, chọn ghế và in vé cho khách hàng.'
              : 'Chọn chuyến xe phù hợp, chọn ghế và thanh toán.'}
          </p>
        </div>
      </div>

      <section className="card">
        <div className="card-header">
          <h2>Tìm chuyến xe</h2>
        </div>
        <div className="search-form">
          <FormField
            label="Điểm đi"
            type="select"
            value={diemDi}
            onChange={(value) => {
              setDiemDi(value);
              setDiemDen('');
            }}
            options={diemDiOptions}
            placeholder="Tất cả điểm đi"
          />
          <FormField
            label="Điểm đến"
            type="select"
            value={diemDen}
            onChange={setDiemDen}
            options={diemDenOptions}
            placeholder="Tất cả điểm đến"
          />
          <FormField label="Ngày đi" type="date" value={ngay} onChange={setNgay} />
          <FormField
            label="Từ giờ"
            type="select"
            value={tuGio}
            onChange={setTuGio}
            placeholder="Không lọc"
            options={Array.from({ length: 24 }, (_, hour) => ({
              value: String(hour),
              label: `${String(hour).padStart(2, '0')}:00`,
            }))}
          />
          <FormField
            label="Đến giờ"
            type="select"
            value={denGio}
            onChange={setDenGio}
            placeholder="Không lọc"
            options={Array.from({ length: 24 }, (_, hour) => ({
              value: String(hour),
              label: `${String(hour).padStart(2, '0')}:00`,
            }))}
          />
          <div className="field field-action">
            <button
              type="button"
              className="btn btn-primary btn-block"
              onClick={() => void handleSearch()}
              disabled={loadingChuyen}
            >
              {loadingChuyen ? 'Đang tìm…' : 'Tìm chuyến'}
            </button>
          </div>
        </div>
      </section>

      {searched || loadingChuyen ? (
        <section className="card">
          <div className="card-header">
            <h2>Chuyến xe phù hợp</h2>
            <span className="text-muted">{chuyens.length} chuyến</span>
          </div>
          <DataTable
            columns={columns}
            rows={chuyens}
            rowKey={(row) => row.maChuyen}
            loading={loadingChuyen}
            emptyText="Không tìm thấy chuyến phù hợp. Hãy thử đổi điểm đi, điểm đến hoặc ngày."
            selectedKey={selectedChuyen?.maChuyen ?? null}
            onRowClick={(row) => void chooseChuyen(row)}
            actions={(row) => (
              <button
                type="button"
                className={`btn btn-sm ${selectedChuyen?.maChuyen === row.maChuyen ? 'btn-secondary' : 'btn-primary'}`}
                onClick={() => void chooseChuyen(row)}
              >
                {selectedChuyen?.maChuyen === row.maChuyen ? 'Đang chọn' : 'Chọn chuyến'}
              </button>
            )}
          />
        </section>
      ) : null}

      {selectedChuyen ? (
        <div className="booking-grid">
          <section className="card">
            <div className="card-header">
              <h2>Sơ đồ ghế</h2>
              <span className="text-muted">
                {selectedChuyen.diemDi} → {selectedChuyen.diemDen} · {formatDateTime(selectedChuyen.ngayGioKhoiHanh)}
              </span>
            </div>
            {loadingGhe ? (
              <div className="empty-state">
                <span className="spinner" /> Đang tải sơ đồ ghế…
              </div>
            ) : ghe ? (
              <SeatMap
                soCho={ghe.soCho}
                tenLoaiXe={ghe.tenLoaiXe}
                gheDaDat={ghe.gheDaDat ?? []}
                selected={selectedSeats}
                onToggle={toggleSeat}
              />
            ) : (
              <div className="empty-state">Không tải được sơ đồ ghế.</div>
            )}
          </section>

          <section className="card">
            <div className="card-header">
              <h2>Thông tin đặt vé</h2>
            </div>

            {bookingError ? <div className="alert alert-error">{bookingError}</div> : null}

            {!isStaff && hasCustomerProfile ? (
              <div className="info-box">
                <span className="info-box-title">Hồ sơ khách hàng</span>
                {hoSo ? (
                  <div className="info-grid">
                    <div>
                      <span className="text-muted">Họ tên</span>
                      <strong>{hoSo.hoTen}</strong>
                    </div>
                    <div>
                      <span className="text-muted">Số điện thoại</span>
                      <strong>{hoSo.sdt ?? '—'}</strong>
                    </div>
                    <div>
                      <span className="text-muted">Email</span>
                      <strong>{hoSo.email}</strong>
                    </div>
                  </div>
                ) : (
                  <p className="text-muted">Đang tải hồ sơ…</p>
                )}
              </div>
            ) : (
              <>
                {!isStaff ? (
                  <div className="alert alert-info">
                    Tài khoản của bạn chưa có hồ sơ khách hàng. Vui lòng nhập thông tin bên dưới — hồ sơ sẽ
                    được tạo tự động khi đặt vé lần đầu.
                  </div>
                ) : null}
                <div className="form-grid">
                  <FormField
                    label="Tên khách hàng"
                    value={tenKhachHang}
                    onChange={setTenKhachHang}
                    placeholder="Nguyễn Văn A"
                    required
                  />
                  <FormField
                    label="Số điện thoại"
                    value={sdt}
                    onChange={setSdt}
                    placeholder="09xxxxxxxx"
                    required
                  />
                  <FormField label="Ngày sinh" type="date" value={namSinh} onChange={setNamSinh} />
                  <FormField
                    label="Giới tính"
                    type="select"
                    value={gioiTinh}
                    onChange={setGioiTinh}
                    options={[
                      { value: 'true', label: 'Nam' },
                      { value: 'false', label: 'Nữ' },
                    ]}
                  />
                </div>
              </>
            )}

            <div className="form-grid booking-pay">
              <FormField
                label="Phương thức thanh toán"
                type="select"
                value={phuongThuc}
                onChange={setPhuongThuc}
                options={methods.map((value) => ({ value, label: value }))}
              />
            </div>

            <div className="ticket-summary">
              <div className="summary-row">
                <span>Ghế đã chọn</span>
                <strong>{selectedSeats.length > 0 ? selectedSeats.join(', ') : 'Chưa chọn'}</strong>
              </div>
              <div className="summary-row">
                <span>Giá vé</span>
                <strong>{formatVnd(giaVe)}</strong>
              </div>
              <div className="summary-row summary-total">
                <span>Tổng tiền</span>
                <strong>{formatVnd(tongTien)}</strong>
              </div>
            </div>

            <button
              type="button"
              className="btn btn-primary btn-block"
              onClick={() => void handleDatVe()}
              disabled={submitting || selectedSeats.length === 0}
            >
              {submitting ? 'Đang đặt vé…' : 'Xác nhận đặt vé'}
            </button>

            {result ? (
              <div className="alert alert-success result-box">
                <strong>Đặt vé thành công!</strong>
                <div className="result-grid">
                  <div>
                    <span className="text-muted">Mã hóa đơn</span>
                    <strong>#{result.maHd}</strong>
                  </div>
                  <div>
                    <span className="text-muted">Số vé</span>
                    <strong>{result.soVe}</strong>
                  </div>
                  <div>
                    <span className="text-muted">Tổng tiền</span>
                    <strong>{formatVnd(result.tongTien)}</strong>
                  </div>
                  <div>
                    <span className="text-muted">Phương thức</span>
                    <strong>{result.phuongThuc}</strong>
                  </div>
                </div>
                {!result.trangThai ? (
                  <p className="text-muted">Hóa đơn chưa thanh toán — bạn có thể thanh toán trong mục Lịch sử đặt vé.</p>
                ) : null}
              </div>
            ) : null}
          </section>
        </div>
      ) : null}
    </div>
  );
}
