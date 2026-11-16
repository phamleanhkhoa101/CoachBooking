import { useCallback, useEffect, useState } from 'react';
import { api, getErrorMessage } from '../../../api/client';
import type { GenerationResult, TripSchedule } from '../../../api/operations';
import type { Driver, Route, Vehicle } from '../../../api/types';
import { DataTable } from '../../../components/table/DataTable';
import { FormField } from '../../../components/common/FormField';
import { Modal } from '../../../components/common/Modal';
import { useToast } from '../../../components/common/Toast';
import { formatDateTime } from '../../../utils/format';

const today = () => new Intl.DateTimeFormat('en-CA', { timeZone: 'Asia/Ho_Chi_Minh', year: 'numeric', month: '2-digit', day: '2-digit' }).format(new Date());
const emptyForm = () => ({ name: '', vehicleId: '', routeId: '', driverId: '', departureTime: '08:00', weekdays: [1, 2, 3, 4, 5, 6, 7], validFrom: today(), validTo: today(), isActive: true, excludedDates: '' });
const STATUS: Record<string, string> = { READY: 'Sẵn sàng', CREATED: 'Đã tạo', EXISTS: 'Đã tồn tại', EXCLUDED: 'Ngày nghỉ', DELETED: 'Đã xóa chuyến', SKIPPED: 'Ngoài lịch', PAST: 'Đã qua', ERROR: 'Lỗi / xung đột' };

export function TripSchedules() {
  const [rows, setRows] = useState<TripSchedule[]>([]);
  const [vehicles, setVehicles] = useState<Vehicle[]>([]);
  const [drivers, setDrivers] = useState<Driver[]>([]);
  const [routes, setRoutes] = useState<Route[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [busy, setBusy] = useState(false);
  const [open, setOpen] = useState(false);
  const [id, setId] = useState<number | null>(null);
  const [form, setForm] = useState(emptyForm);
  const [target, setTarget] = useState<TripSchedule | null>(null);
  const [from, setFrom] = useState(today);
  const [to, setTo] = useState(today);
  const [results, setResults] = useState<GenerationResult[]>([]);
  const { showToast } = useToast();
  const load = useCallback(async () => {
    setLoading(true); setError('');
    try {
      const [s, v, d, r] = await Promise.all([
        api.get<TripSchedule[]>('/trip-schedules'), api.get<Vehicle[]>('/vehicles'), api.get<Driver[]>('/drivers'),
        api.get<Route[]>('/routes'),
      ]);
      setRows(s); setVehicles(v); setDrivers(d); setRoutes(r);
    } catch (e) { setError(getErrorMessage(e)); } finally { setLoading(false); }
  }, []);
  useEffect(() => { void load(); }, [load]);
  async function run(preview: boolean) {
    if (!target) return;
    setBusy(true); setError(''); setResults([]);
    try {
      const data = await api.post<GenerationResult[]>(`/trip-schedules/${target.scheduleId}/${preview ? 'preview' : 'generate'}`, { from, to });
      setResults(data);
      if (!preview) { showToast('Đã xử lý lịch. Xem kết quả từng ngày.'); await load(); }
    } catch (e) { setError(getErrorMessage(e)); } finally { setBusy(false); }
  }
  return <div className="page">
    <div className="page-header"><div><h1>Lịch chạy định kỳ</h1><p className="page-subtitle">Giờ Việt Nam. Tự bổ sung 14 ngày khi khởi động và lúc 00:05 mỗi ngày. Sửa lịch không đổi chuyến đã sinh.</p></div>
      <button className="btn btn-primary" onClick={() => { setId(null); setForm(emptyForm()); setError(''); setOpen(true); }}>+ Thêm lịch</button>
    </div>
    {error && <p role="alert" className="form-error-banner">{error}</p>}
    <button className="btn btn-secondary" disabled={loading} onClick={() => void load()}>Làm mới</button>
    <DataTable rows={rows} rowKey={s => String(s.scheduleId)} loading={loading} emptyText="Chưa có lịch chạy. Chọn xe, tuyến và tài xế để tạo lịch."
      columns={[
        { key: 'name', label: 'Lịch', render: s => s.name },
        { key: 'time', label: 'Giờ / Thứ', render: s => `${s.departureTime} · ${s.weekdays.split(',').map(d => Number(d) === 7 ? 'CN' : 'T' + (Number(d) + 1)).join(', ')}` },
        { key: 'range', label: 'Hiệu lực', render: s => `${s.validFrom} → ${s.validTo}` },
        { key: 'active', label: 'Trạng thái', render: s => s.isActive ? 'Hoạt động' : 'Tắt' },
        { key: 'last', label: 'Lần xử lý gần nhất', render: s => s.lastRunAt ? formatDateTime(s.lastRunAt) : 'Chưa chạy' },
      ]} actions={s => <div className="table-actions">
        <button className="btn btn-secondary btn-sm" onClick={() => {
          setId(s.scheduleId); setError(''); setForm({ ...s, vehicleId: String(s.vehicleId), routeId: String(s.routeId), driverId: String(s.driverId), weekdays: s.weekdays.split(',').map(Number), excludedDates: s.excludedDates }); setOpen(true);
        }}>Sửa</button>
        <button className="btn btn-primary btn-sm" onClick={() => { setTarget(s); setError(''); setFrom(today()); setTo(today()); try { setResults(s.lastResult ? JSON.parse(s.lastResult) : []); } catch { setResults([]); } }}>Xem / Sinh chuyến</button>
      </div>} />
    <Modal open={open} title={id ? 'Sửa lịch chạy' : 'Thêm lịch chạy'} onClose={() => { if (!busy) setOpen(false); }}>
      <form onSubmit={async e => {
        e.preventDefault(); setBusy(true); setError('');
        const payload = { ...form, vehicleId: Number(form.vehicleId), routeId: Number(form.routeId), driverId: Number(form.driverId), excludedDates: form.excludedDates.split(/[\s,]+/).filter(Boolean) };
        try {
          if (id) await api.put(`/trip-schedules/${id}`, payload); else await api.post('/trip-schedules', payload);
          showToast('Đã lưu lịch chạy.'); setOpen(false); await load();
        } catch (e) { setError(getErrorMessage(e)); } finally { setBusy(false); }
      }}>
        {error && <p role="alert" className="form-error-banner">{error}</p>}
        <div className="form-grid">
          <FormField label="Tên lịch" required value={form.name} onChange={name => setForm({ ...form, name })} />
          <FormField label="Xe" type="select" required value={form.vehicleId} onChange={vehicleId => setForm({ ...form, vehicleId })} options={vehicles.filter(v => v.isActive).map(v => ({ value: String(v.vehicleId), label: v.licensePlate }))} />
          <FormField label="Tuyến / chiều" type="select" required value={form.routeId} onChange={routeId => setForm({ ...form, routeId })} options={routes.filter(r => r.isActive).map(r => ({ value: String(r.routeId), label: `${r.departure} → ${r.destination}` }))} />
          <FormField label="Tài xế" type="select" required value={form.driverId} onChange={driverId => setForm({ ...form, driverId })} options={drivers.filter(d => d.isActive).map(d => ({ value: String(d.driverId), label: d.fullName }))} />
          <FormField label="Giờ khởi hành" type="time" required value={form.departureTime} onChange={departureTime => setForm({ ...form, departureTime })} />
          <FormField label="Từ ngày" type="date" required value={form.validFrom} onChange={validFrom => setForm({ ...form, validFrom })} />
          <FormField label="Đến ngày" type="date" required value={form.validTo} onChange={validTo => setForm({ ...form, validTo })} />
        </div>
        <fieldset><legend>Ngày chạy trong tuần</legend>{[1, 2, 3, 4, 5, 6, 7].map(day => <label key={day} style={{ marginRight: 12 }}><input type="checkbox" checked={form.weekdays.includes(day)} onChange={e => setForm({ ...form, weekdays: e.target.checked ? [...form.weekdays, day] : form.weekdays.filter(d => d !== day) })} />{day === 7 ? 'CN' : 'Thứ ' + (day + 1)}</label>)}</fieldset>
        <FormField label="Ngày nghỉ (YYYY-MM-DD, cách nhau bằng dấu phẩy)" value={form.excludedDates} onChange={excludedDates => setForm({ ...form, excludedDates })} />
        <label><input type="checkbox" checked={form.isActive} onChange={e => setForm({ ...form, isActive: e.target.checked })} /> Bật lịch</label>
        <button className="btn btn-primary" disabled={busy || !form.weekdays.length}>{busy ? 'Đang lưu…' : 'Lưu lịch'}</button>
      </form>
    </Modal>
    <Modal open={Boolean(target)} title={`Xử lý lịch: ${target?.name ?? ''}`} onClose={() => { if (!busy) setTarget(null); }}>
      {error && <p role="alert" className="form-error-banner">{error}</p>}
      <p className="text-muted">Chọn tối đa 30 ngày. Xem trước là dự kiến; hệ thống kiểm tra lại khi tạo. Ngày đã loại trừ hoặc chuyến đã xóa không tự sinh lại.</p>
      <div className="form-grid">
        <FormField label="Từ ngày" type="date" value={from} onChange={setFrom} />
        <FormField label="Đến ngày" type="date" value={to} onChange={setTo} />
      </div>
      <div className="table-actions"><button className="btn btn-secondary" disabled={busy} onClick={() => void run(true)}>Xem trước</button>
        <button className="btn btn-primary" disabled={busy || !target?.isActive} onClick={() => void run(false)}>{busy ? 'Đang xử lý…' : 'Sinh chuyến'}</button></div>
      <DataTable rows={results} rowKey={r => r.serviceDate} loading={busy} emptyText="Chưa có kết quả. Chọn Xem trước để kiểm tra."
        columns={[
          { key: 'date', label: 'Ngày', render: r => r.serviceDate },
          { key: 'status', label: 'Kết quả', render: r => STATUS[r.status] ?? r.status },
          { key: 'trip', label: 'Chuyến', render: r => r.tripId ? '#' + r.tripId : '—' },
          { key: 'message', label: 'Chi tiết', render: r => r.message },
        ]} />
    </Modal>
  </div>;
}
