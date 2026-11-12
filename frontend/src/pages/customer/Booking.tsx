import { useCallback, useEffect, useMemo, useState } from 'react';
import { ApiError, api, buildQuery, getErrorMessage } from '../../api/client';
import type {
  BookingRequest,
  BookingResponse,
  Profile,
  Route,
  SeatHoldResponse,
  SeatMapResponse,
  Trip,
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
  const { user, nguoiDung } = useAuth();
  const currentUser = user ?? nguoiDung;
  const { showToast } = useToast();

  const [routeList, setRouteList] = useState<Route[]>([]);
  const [departure, setDeparture] = useState('');
  const [destination, setDestination] = useState('');
  const [travelDate, setTravelDate] = useState(todayISO());
  const [fromHour, setFromHour] = useState('');
  const [toHour, setToHour] = useState('');

  const [trips, setTrips] = useState<Trip[]>([]);
  const [searched, setSearched] = useState(false);
  const [loadingTrips, setLoadingTrips] = useState(false);

  const [selectedTrip, setSelectedTrip] = useState<Trip | null>(null);
  const [seatMap, setSeatMap] = useState<SeatMapResponse | null>(null);
  const [loadingSeats, setLoadingSeats] = useState(false);
  const [selectedSeats, setSelectedSeats] = useState<number[]>([]);

  const [profile, setProfile] = useState<Profile | null>(null);
  const [customerName, setCustomerName] = useState('');
  const [phone, setPhone] = useState('');
  const [dateOfBirth, setDateOfBirth] = useState('');
  const [gender, setGender] = useState('true');
  const [paymentMethod, setPaymentMethod] = useState(
    isStaff ? 'Tiền mặt' : 'Chuyển khoản',
  );
  const [submitting, setSubmitting] = useState(false);
  const [bookingError, setBookingError] = useState('');
  const [result, setResult] = useState<BookingResponse | null>(null);

  const methods = useMemo(
    () => (isStaff ? STAFF_METHODS : ['Chuyển khoản']),
    [isStaff],
  );

  useEffect(() => {
    void (async () => {
      try {
        const list = await api.get<Route[]>('/routes');
        setRouteList(Array.isArray(list) ? list : []);
      } catch (error) {
        showToast(getErrorMessage(error), 'error');
      }
    })();
  }, [showToast]);

  useEffect(() => {
    if (isStaff) return;
    void (async () => {
      try {
        setProfile(await api.get<Profile>('/profile'));
      } catch (error) {
        showToast(getErrorMessage(error), 'error');
      }
    })();
  }, [isStaff, showToast]);

  const departureOptions = useMemo(() => {
    const unique = Array.from(
      new Set(routeList.map((item) => item.departure ?? item.diemDi).filter(Boolean)),
    );
    return unique.map((value) => ({ value: value!, label: value! }));
  }, [routeList]);

  const destinationOptions = useMemo(() => {
    const unique = Array.from(
      new Set(
        routeList
          .filter((item) => {
            const dep = item.departure ?? item.diemDi;
            return departure ? dep === departure : true;
          })
          .map((item) => item.destination ?? item.diemDen)
          .filter(Boolean),
      ),
    );
    return unique.map((value) => ({ value: value!, label: value! }));
  }, [routeList, departure]);

  const handleSearch = useCallback(async () => {
    setLoadingTrips(true);
    try {
      const data = await api.get<Trip[]>(
        `/trips/search${buildQuery({
          departure,
          destination,
          date: travelDate,
          fromHour,
          toHour,
          diemDi: departure,
          diemDen: destination,
          ngay: travelDate,
          tuGio: fromHour,
          denGio: toHour,
        })}`,
      );
      setTrips(Array.isArray(data) ? data : []);
      setSearched(true);
    } catch (error) {
      setTrips([]);
      setSearched(true);
      showToast(getErrorMessage(error), 'error');
    } finally {
      setLoadingTrips(false);
    }
  }, [departure, destination, travelDate, fromHour, toHour, showToast]);

  const loadSeats = useCallback(
    async (tripId: number) => {
      setLoadingSeats(true);
      try {
        const data = await api.get<SeatMapResponse>(`/trips/${tripId}/seats`);
        setSeatMap(data);
      } catch (error) {
        setSeatMap(null);
        showToast(getErrorMessage(error), 'error');
      } finally {
        setLoadingSeats(false);
      }
    },
    [showToast],
  );

  const chooseTrip = async (trip: Trip) => {
    setSelectedTrip(trip);
    setSelectedSeats([]);
    setBookingError('');
    setResult(null);
    const id = trip.tripId ?? trip.maChuyen;
    if (id) {
      await loadSeats(id);
    }
  };

  const toggleSeat = (seat: number) => {
    setSelectedSeats((prev) =>
      prev.includes(seat) ? prev.filter((item) => item !== seat) : [...prev, seat].sort((a, b) => a - b),
    );
  };

  const ticketPrice =
    seatMap?.price ??
    seatMap?.ticketPrice ??
    selectedTrip?.actualPrice ??
    0;
  const totalAmount = ticketPrice * selectedSeats.length;

  const hasCustomerProfile =
    !isStaff &&
    ((currentUser?.customerId ?? currentUser?.maKh ?? null) !== null ||
      (profile?.customerId ?? profile?.maKh ?? null) !== null);
  const needCustomerInfo = isStaff || !hasCustomerProfile;

  const handleBooking = async () => {
    const tripId = selectedTrip?.tripId ?? selectedTrip?.maChuyen;
    if (!selectedTrip || !tripId) {
      setBookingError('Vui lòng chọn chuyến xe trước.');
      return;
    }
    if (selectedSeats.length === 0) {
      setBookingError('Vui lòng chọn ít nhất một ghế trên sơ đồ.');
      return;
    }
    if (needCustomerInfo) {
      if (!customerName.trim()) {
        setBookingError('Vui lòng nhập tên khách hàng.');
        return;
      }
      if (!/^\d{10,11}$/.test(phone.trim())) {
        setBookingError('Số điện thoại phải gồm 10 đến 11 chữ số.');
        return;
      }
      if (!dateOfBirth) {
        setBookingError('Vui lòng nhập ngày sinh khách hàng.');
        return;
      }
    }

    const body: BookingRequest = {
      tripId,
      seatNumbers: selectedSeats,
      paymentMethod,
    };
    if (needCustomerInfo) {
      body.customerName = customerName.trim();
      body.phone = phone.trim();
      body.dateOfBirth = dateOfBirth;
      body.gender = gender === 'true';
    }

    setSubmitting(true);
    setBookingError('');
    try {
      if (!isStaff) {
        const hold = await api.post<SeatHoldResponse>('/seat-holds', {
          tripId,
          seatNumbers: selectedSeats,
        });
        body.holdToken = hold.holdToken;
      }
      const res = await api.post<BookingResponse>('/bookings', body);
      setResult(res);
      const invoiceId = res.invoiceId ?? res.maHd;
      showToast(`Đặt vé thành công, hóa đơn #${invoiceId}.`);
      setSelectedSeats([]);
      if (needCustomerInfo) {
        setCustomerName('');
        setPhone('');
        setDateOfBirth('');
        setGender('true');
      }
      if (!isStaff && !hasCustomerProfile) {
        try {
          setProfile(await api.get<Profile>('/profile'));
        } catch {
          // Non-blocking
        }
      }
      await loadSeats(tripId);
      await handleSearch();
    } catch (error) {
      const message = getErrorMessage(error);
      setBookingError(message);
      showToast(message, 'error');
      if (error instanceof ApiError && error.status === 409) {
        setSelectedSeats([]);
        await loadSeats(tripId);
      }
    } finally {
      setSubmitting(false);
    }
  };

  const columns: DataTableColumn<Trip>[] = [
    {
      key: 'departureTime',
      label: 'Giờ đi',
      width: '110px',
      render: (row) => {
        const time = row.departureTime ?? row.ngayGioKhoiHanh;
        return (
          <div className="cell-stack">
            <strong>{formatTime(time)}</strong>
            <span className="text-muted">{formatDate(time)}</span>
          </div>
        );
      },
    },
    {
      key: 'route',
      label: 'Tuyến',
      render: (row) => {
        const dep = row.departure ?? row.diemDi;
        const dest = row.destination ?? row.diemDen;
        const arr = row.estimatedArrivalTime ?? row.ngayGioDenDuKien;
        return (
          <div className="cell-stack">
            <strong>
              {dep} → {dest}
            </strong>
            <span className="text-muted">Đến dự kiến {formatTime(arr)}</span>
          </div>
        );
      },
    },
    {
      key: 'companyName',
      label: 'Nhà xe',
      render: (row) => row.companyName ?? row.tenNhaXe,
    },
    {
      key: 'typeName',
      label: 'Loại xe',
      render: (row) => row.typeName ?? row.tenLoaiXe,
    },
    {
      key: 'seatCapacity',
      label: 'Số chỗ',
      align: 'right',
      width: '80px',
      render: (row) => row.seatCapacity ?? row.soCho,
    },
    {
      key: 'bookedSeats',
      label: 'Đã đặt',
      align: 'right',
      width: '90px',
      render: (row) => {
        const booked = row.ticketsSold ?? row.soVeDaBan ?? row.bookedSeats ?? 0;
        const cap = row.seatCapacity ?? row.soCho ?? 0;
        return (
          <span className={booked >= cap ? 'text-danger' : undefined}>
            {booked}/{cap}
          </span>
        );
      },
    },
    {
      key: 'actualPrice',
      label: 'Giá vé',
      align: 'right',
      render: (row) => (
        <strong className="text-primary">
          {formatVnd(row.actualPrice ?? row.giaVeThucTe)}
        </strong>
      ),
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
            value={departure}
            onChange={(value) => {
              setDeparture(value);
              setDestination('');
            }}
            options={departureOptions}
            placeholder="Tất cả điểm đi"
          />
          <FormField
            label="Điểm đến"
            type="select"
            value={destination}
            onChange={setDestination}
            options={destinationOptions}
            placeholder="Tất cả điểm đến"
          />
          <FormField label="Ngày đi" type="date" value={travelDate} onChange={setTravelDate} />
          <FormField
            label="Từ giờ"
            type="select"
            value={fromHour}
            onChange={setFromHour}
            placeholder="Không lọc"
            options={Array.from({ length: 24 }, (_, hour) => ({
              value: String(hour),
              label: `${String(hour).padStart(2, '0')}:00`,
            }))}
          />
          <FormField
            label="Đến giờ"
            type="select"
            value={toHour}
            onChange={setToHour}
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
              disabled={loadingTrips}
            >
              {loadingTrips ? 'Đang tìm…' : 'Tìm chuyến'}
            </button>
          </div>
        </div>
      </section>

      {searched || loadingTrips ? (
        <section className="card">
          <div className="card-header">
            <h2>Chuyến xe phù hợp</h2>
            <span className="text-muted">{trips.length} chuyến</span>
          </div>
          <DataTable<Trip>
            columns={columns}
            rows={trips}
            rowKey={(row) => String(row.tripId ?? row.maChuyen)}
            loading={loadingTrips}
            emptyText="Không tìm thấy chuyến phù hợp. Hãy thử đổi điểm đi, điểm đến hoặc ngày."
            actions={(row) => {
              const currentId = selectedTrip?.tripId ?? selectedTrip?.maChuyen;
              const rowId = row.tripId ?? row.maChuyen;
              const isSelected = currentId === rowId;
              return (
                <button
                  type="button"
                  className={`btn btn-sm ${isSelected ? 'btn-secondary' : 'btn-primary'}`}
                  onClick={() => void chooseTrip(row)}
                >
                  {isSelected ? 'Đang chọn' : 'Chọn chuyến'}
                </button>
              );
            }}
          />
        </section>
      ) : null}

      {selectedTrip ? (
        <div className="booking-grid">
          <section className="card">
            <div className="card-header">
              <h2>Sơ đồ ghế</h2>
              <span className="text-muted">
                {selectedTrip.departure ?? selectedTrip.diemDi} →{' '}
                {selectedTrip.destination ?? selectedTrip.diemDen} ·{' '}
                {formatDateTime(selectedTrip.departureTime ?? selectedTrip.ngayGioKhoiHanh)}
              </span>
            </div>
            {loadingSeats ? (
              <div className="empty-state">
                <span className="spinner" /> Đang tải sơ đồ ghế…
              </div>
            ) : seatMap ? (
              <SeatMap
                seatCapacity={seatMap.seatCapacity}
                vehicleTypeName={seatMap.typeName ?? seatMap.vehicleTypeName}
                bookedSeats={[...(seatMap.bookedSeats ?? []), ...(seatMap.heldSeats ?? [])]}
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
                {profile ? (
                  <div className="info-grid">
                    <div>
                      <span className="text-muted">Họ tên</span>
                      <strong>{profile.fullName ?? profile.hoTen}</strong>
                    </div>
                    <div>
                      <span className="text-muted">Số điện thoại</span>
                      <strong>{profile.phone ?? profile.sdt ?? '—'}</strong>
                    </div>
                    <div>
                      <span className="text-muted">Email</span>
                      <strong>{profile.email}</strong>
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
                    value={customerName}
                    onChange={setCustomerName}
                    placeholder="Nguyễn Văn A"
                    required
                  />
                  <FormField
                    label="Số điện thoại"
                    value={phone}
                    onChange={setPhone}
                    placeholder="09xxxxxxxx"
                    required
                  />
                  <FormField
                    label="Ngày sinh"
                    type="date"
                    value={dateOfBirth}
                    onChange={setDateOfBirth}
                  />
                  <FormField
                    label="Giới tính"
                    type="select"
                    value={gender}
                    onChange={setGender}
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
                value={paymentMethod}
                onChange={setPaymentMethod}
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
                <strong>{formatVnd(ticketPrice)}</strong>
              </div>
              <div className="summary-row summary-total">
                <span>Tổng tiền</span>
                <strong>{formatVnd(totalAmount)}</strong>
              </div>
            </div>

            <button
              type="button"
              className="btn btn-primary btn-block"
              onClick={() => void handleBooking()}
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
                    <strong>#{result.invoiceId ?? result.maHd}</strong>
                  </div>
                  <div>
                    <span className="text-muted">Số vé</span>
                    <strong>{result.ticketCount ?? result.soVe}</strong>
                  </div>
                  <div>
                    <span className="text-muted">Tổng tiền</span>
                    <strong>{formatVnd(result.totalAmount ?? result.tongTien)}</strong>
                  </div>
                  <div>
                    <span className="text-muted">Phương thức</span>
                    <strong>{result.paymentMethod ?? result.phuongThuc}</strong>
                  </div>
                </div>
                {!(result.isPaid ?? result.trangThai) ? (
                  <p className="text-muted">
                    Hóa đơn chưa thanh toán — bạn có thể thanh toán trong mục Lịch sử đặt vé.
                  </p>
                ) : null}
              </div>
            ) : null}
          </section>
        </div>
      ) : null}
    </div>
  );
}
