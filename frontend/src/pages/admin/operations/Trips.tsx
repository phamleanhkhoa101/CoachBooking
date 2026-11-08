import { useCallback, useEffect, useMemo, useState } from 'react';
import { api, getErrorMessage } from '../../../api/client';
import type { Driver, Route, Trip, Vehicle } from '../../../api/types';
import { useAuth } from '../../../auth/AuthContext';
import { Badge } from '../../../components/common/Badge';
import { DataTable, type DataTableColumn } from '../../../components/table/DataTable';
import { FormField } from '../../../components/common/FormField';
import { Modal } from '../../../components/common/Modal';
import { useToast } from '../../../components/common/Toast';
import { formatDateTime, formatVnd, toDateTimeInput } from '../../../utils/format';

interface TripForm {
  vehicleId: string;
  routeId: string;
  driverId: string;
  departureTime: string;
}

const EMPTY_FORM: TripForm = { vehicleId: '', routeId: '', driverId: '', departureTime: '' };

function isUpcoming(value?: string | null): boolean {
  if (!value) return false;
  return new Date(value).getTime() > Date.now();
}

export function Trips() {
  const { user, nguoiDung } = useAuth();
  const { showToast } = useToast();
  const currentUser = user ?? nguoiDung;
  const isAdmin = (currentUser?.role ?? currentUser?.quyen) === 'Admin';

  const [rows, setRows] = useState<Trip[]>([]);
  const [vehicles, setVehicles] = useState<Vehicle[]>([]);
  const [routes, setRoutes] = useState<Route[]>([]);
  const [drivers, setDrivers] = useState<Driver[]>([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');

  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<Trip | null>(null);
  const [form, setForm] = useState<TripForm>(EMPTY_FORM);
  const [formError, setFormError] = useState('');
  const [saving, setSaving] = useState(false);
  const [deleting, setDeleting] = useState<Trip | null>(null);
  const [deleteBusy, setDeleteBusy] = useState(false);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const [tripData, vehicleData, routeData, driverData] = await Promise.all([
        api.get<Trip[]>('/trips'),
        api.get<Vehicle[]>('/vehicles'),
        api.get<Route[]>('/routes'),
        api.get<Driver[]>('/drivers'),
      ]);
      setRows(Array.isArray(tripData) ? tripData : []);
      setVehicles(Array.isArray(vehicleData) ? vehicleData : []);
      setRoutes(Array.isArray(routeData) ? routeData : []);
      setDrivers(Array.isArray(driverData) ? driverData : []);
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setLoading(false);
    }
  }, [showToast]);

  useEffect(() => {
    void load();
  }, [load]);

  const vehicleOptions = useMemo(
    () =>
      vehicles
        .filter((v) => (v.isActive ?? v.trangThai) !== false)
        .map((v) => ({
          value: String(v.vehicleId ?? v.maXe),
          label: v.licensePlate ?? v.bienSoXe ?? '',
        })),
    [vehicles],
  );

  const routeOptions = useMemo(
    () =>
      routes.map((r) => ({
        value: String(r.routeId ?? r.maTuyen),
        label: `${r.departure ?? r.diemDi} → ${r.destination ?? r.diemDen}`,
      })),
    [routes],
  );

  const driverOptions = useMemo(
    () =>
      drivers
        .filter((d) => (d.isActive ?? d.trangThai) !== false)
        .map((d) => ({
          value: String(d.driverId ?? d.maTx),
          label: d.fullName ?? d.tenTx ?? '',
        })),
    [drivers],
  );

  const openCreate = () => {
    setEditing(null);
    setForm(EMPTY_FORM);
    setFormError('');
    setModalOpen(true);
  };

  const openEdit = (row: Trip) => {
    setEditing(row);
    setForm({
      vehicleId: String(row.vehicleId ?? row.maXe ?? ''),
      routeId: String(row.routeId ?? row.maTuyen ?? ''),
      driverId: String(row.driverId ?? row.maTx ?? ''),
      departureTime: toDateTimeInput(row.departureTime ?? row.ngayGioKhoiHanh),
    });
    setFormError('');
    setModalOpen(true);
  };

  const setField = (name: keyof TripForm, value: string) => {
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async () => {
    if (!form.vehicleId || !form.routeId || !form.driverId || !form.departureTime) {
      setFormError('Vui lòng chọn xe, tuyến đường, tài xế và thời điểm khởi hành.');
      return;
    }
    const departureTimeIso =
      form.departureTime.length === 16 ? `${form.departureTime}:00` : form.departureTime;
    const payload = {
      vehicleId: Number(form.vehicleId),
      routeId: Number(form.routeId),
      driverId: Number(form.driverId),
      departureTime: departureTimeIso,
      // Vietnamese fallbacks
      maXe: Number(form.vehicleId),
      maTuyen: Number(form.routeId),
      maTx: Number(form.driverId),
      ngayGioKhoiHanh: departureTimeIso,
    };

    setSaving(true);
    setFormError('');
    try {
      const tripId = editing ? (editing.tripId ?? editing.maChuyen) : null;
      if (editing && tripId) {
        await api.put(`/trips/${tripId}`, payload);
        showToast('Đã cập nhật chuyến xe.');
      } else {
        await api.post('/trips', payload);
        showToast('Đã thêm chuyến xe.');
      }
      setModalOpen(false);
      await load();
    } catch (error) {
      setFormError(getErrorMessage(error));
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async () => {
    if (!deleting) return;
    const tripId = deleting.tripId ?? deleting.maChuyen;
    if (!tripId) return;
    setDeleteBusy(true);
    try {
      await api.del(`/trips/${tripId}`);
      showToast('Đã xóa chuyến xe.');
      setDeleting(null);
      await load();
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setDeleteBusy(false);
    }
  };

  const filtered = useMemo(() => {
    const term = search.trim().toLowerCase();
    if (!term) return rows;
    return rows.filter((row) => {
      const dep = row.departure ?? row.diemDi;
      const dest = row.destination ?? row.diemDen;
      const plate = row.licensePlate ?? row.bienSoXe;
      const company = row.companyName ?? row.tenNhaXe;
      const driver = row.driverName ?? row.tenTx;
      return [dep, dest, plate, company, driver]
        .filter(Boolean)
        .some((val) => String(val).toLowerCase().includes(term));
    });
  }, [rows, search]);

  const columns: DataTableColumn<Trip>[] = [
    {
      key: 'departureTime',
      label: 'Khởi hành',
      width: '150px',
      render: (row) => {
        const depTime = row.departureTime ?? row.ngayGioKhoiHanh;
        const arrTime = row.estimatedArrivalTime ?? row.ngayGioDenDuKien;
        return (
          <div className="cell-stack">
            <strong>{formatDateTime(depTime)}</strong>
            <span className="text-muted">Đến {formatDateTime(arrTime)}</span>
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
        const company = row.companyName ?? row.tenNhaXe;
        return (
          <div className="cell-stack">
            <strong>
              {dep} → {dest}
            </strong>
            <span className="text-muted">{company}</span>
          </div>
        );
      },
    },
    {
      key: 'vehicle',
      label: 'Xe / Loại xe',
      render: (row) => {
        const typeName = row.typeName ?? row.tenLoaiXe;
        const cap = row.seatCapacity ?? row.soCho;
        const plate = row.licensePlate ?? row.bienSoXe;
        return (
          <div className="cell-stack">
            <strong>{plate}</strong>
            <span className="text-muted">
              {typeName} · {cap} chỗ
            </span>
          </div>
        );
      },
    },
    {
      key: 'driverName',
      label: 'Tài xế',
      render: (row) => row.driverName ?? row.tenTx ?? '—',
    },
    {
      key: 'actualPrice',
      label: 'Giá vé',
      align: 'right',
      render: (row) => (
        <strong className="text-primary">{formatVnd(row.actualPrice ?? row.giaVeThucTe)}</strong>
      ),
    },
    {
      key: 'ticketsSold',
      label: 'Đã bán',
      align: 'center',
      render: (row) => {
        const sold = row.ticketsSold ?? row.soVeDaBan ?? row.bookedSeats ?? 0;
        const cap = row.seatCapacity ?? row.soCho ?? 0;
        return `${sold}/${cap}`;
      },
    },
    {
      key: 'status',
      label: 'Trạng thái',
      width: '130px',
      render: (row) => {
        const depTime = row.departureTime ?? row.ngayGioKhoiHanh;
        const upcoming = isUpcoming(depTime);
        return (
          <Badge tone={upcoming ? 'success' : 'neutral'}>
            {upcoming ? 'Sắp chạy' : 'Đã khởi hành'}
          </Badge>
        );
      },
    },
    {
      key: 'actions',
      label: 'Thao tác',
      align: 'right',
      render: (row) =>
        isAdmin ? (
          <div className="table-actions">
            <button
              type="button"
              className="btn btn-sm btn-secondary"
              onClick={() => openEdit(row)}
            >
              Sửa
            </button>
            <button
              type="button"
              className="btn btn-sm btn-danger-ghost"
              onClick={() => setDeleting(row)}
            >
              Xóa
            </button>
          </div>
        ) : null,
    },
  ];

  const modalFooter = (
    <>
      <button
        type="button"
        className="btn btn-secondary"
        onClick={() => setModalOpen(false)}
        disabled={saving}
      >
        Hủy
      </button>
      <button
        type="button"
        className="btn btn-primary"
        onClick={() => void handleSubmit()}
        disabled={saving}
      >
        {saving ? 'Đang lưu…' : editing ? 'Cập nhật' : 'Thêm mới'}
      </button>
    </>
  );

  const deleteFooter = (
    <>
      <button
        type="button"
        className="btn btn-secondary"
        onClick={() => setDeleting(null)}
        disabled={deleteBusy}
      >
        Hủy
      </button>
      <button
        type="button"
        className="btn btn-danger"
        onClick={() => void handleDelete()}
        disabled={deleteBusy}
      >
        {deleteBusy ? 'Đang xóa…' : 'Xác nhận xóa'}
      </button>
    </>
  );

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>Chuyến xe</h1>
          <p className="page-subtitle">
            Lịch chạy của các xe. Giờ đến và giá vé được hệ thống tự tính từ tuyến đường và bảng giá.
          </p>
        </div>
        {isAdmin && (
          <div className="page-actions">
            <button type="button" className="btn btn-primary" onClick={openCreate}>
              + Thêm chuyến xe
            </button>
          </div>
        )}
      </div>

      <div className="page-toolbar">
        <input
          type="search"
          className="search-input"
          placeholder="Tìm theo tuyến, biển số xe, nhà xe, tài xế…"
          value={search}
          onChange={(event) => setSearch(event.target.value)}
        />
        <div className="toolbar-stats">
          <span>{filtered.length} / {rows.length} chuyến</span>
          <button type="button" className="btn btn-secondary btn-sm" onClick={() => void load()}>
            Làm mới
          </button>
        </div>
      </div>

      <DataTable<Trip>
        columns={columns}
        rows={filtered}
        rowKey={(row) => String(row.tripId ?? row.maChuyen)}
        loading={loading}
        emptyText={search ? 'Không có chuyến xe nào khớp tìm kiếm.' : 'Chưa có chuyến xe nào.'}
      />

      <Modal
        open={modalOpen}
        title={editing ? `Sửa chuyến #${editing.tripId ?? editing.maChuyen}` : 'Thêm chuyến xe mới'}
        onClose={() => setModalOpen(false)}
        footer={modalFooter}
      >
        {formError && <div className="form-error-banner">{formError}</div>}
        <form
          className="form-grid"
          onSubmit={(event) => {
            event.preventDefault();
            void handleSubmit();
          }}
        >
          <FormField
            label="Xe vận hành"
            name="vehicleId"
            type="select"
            required
            value={form.vehicleId}
            onChange={(val) => setField('vehicleId', val)}
            options={vehicleOptions}
            placeholder="Chọn xe"
          />
          <FormField
            label="Tuyến đường"
            name="routeId"
            type="select"
            required
            value={form.routeId}
            onChange={(val) => setField('routeId', val)}
            options={routeOptions}
            placeholder="Chọn tuyến"
          />
          <FormField
            label="Tài xế phụ trách"
            name="driverId"
            type="select"
            required
            value={form.driverId}
            onChange={(val) => setField('driverId', val)}
            options={driverOptions}
            placeholder="Chọn tài xế"
          />
          <FormField
            label="Thời điểm khởi hành"
            name="departureTime"
            type="datetime-local"
            required
            value={form.departureTime}
            onChange={(val) => setField('departureTime', val)}
            help="Giờ đến dự kiến và giá vé được tự động tính dựa trên bảng giá và thời gian tuyến."
          />
        </form>
      </Modal>

      <Modal
        open={Boolean(deleting)}
        title="Xác nhận xóa chuyến xe"
        onClose={() => setDeleting(null)}
        footer={deleteFooter}
      >
        <p>
          Bạn có chắc muốn xóa chuyến #{deleting?.tripId ?? deleting?.maChuyen} (
          {deleting?.departure ?? deleting?.diemDi} → {deleting?.destination ?? deleting?.diemDen} lúc{' '}
          {formatDateTime(deleting?.departureTime ?? deleting?.ngayGioKhoiHanh)})?
        </p>
        <p className="text-muted">Chuyến xe đã có vé đặt sẽ không thể xóa.</p>
      </Modal>
    </div>
  );
}
