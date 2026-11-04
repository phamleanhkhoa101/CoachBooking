import { useCallback, useEffect, useMemo, useState } from 'react';
import { api, getErrorMessage } from '../api/client';
import type { ChuyenXe, TaiXe, TuyenDuong, Xe } from '../api/types';
import { useAuth } from '../auth/AuthContext';
import { Badge } from '../components/Badge';
import { DataTable, type DataTableColumn } from '../components/DataTable';
import { FormField } from '../components/FormField';
import { Modal } from '../components/Modal';
import { useToast } from '../components/Toast';
import { formatDateTime, formatVnd, toDateTimeInput } from '../utils/format';

interface TripForm {
  maXe: string;
  maTuyen: string;
  maTx: string;
  ngayGioKhoiHanh: string;
}

const EMPTY_FORM: TripForm = { maXe: '', maTuyen: '', maTx: '', ngayGioKhoiHanh: '' };

function isUpcoming(value: string): boolean {
  return new Date(value).getTime() > Date.now();
}

export function Trips() {
  const { nguoiDung } = useAuth();
  const { showToast } = useToast();
  const isAdmin = nguoiDung?.quyen === 'Admin';

  const [rows, setRows] = useState<ChuyenXe[]>([]);
  const [xes, setXes] = useState<Xe[]>([]);
  const [tuyenList, setTuyenList] = useState<TuyenDuong[]>([]);
  const [taiXes, setTaiXes] = useState<TaiXe[]>([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');

  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<ChuyenXe | null>(null);
  const [form, setForm] = useState<TripForm>(EMPTY_FORM);
  const [formError, setFormError] = useState('');
  const [saving, setSaving] = useState(false);
  const [deleting, setDeleting] = useState<ChuyenXe | null>(null);
  const [deleteBusy, setDeleteBusy] = useState(false);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const [chuyenData, xeData, tuyenData, txData] = await Promise.all([
        api.get<ChuyenXe[]>('/chuyen-xe'),
        api.get<Xe[]>('/xe'),
        api.get<TuyenDuong[]>('/tuyen-duong'),
        api.get<TaiXe[]>('/tai-xe'),
      ]);
      setRows(Array.isArray(chuyenData) ? chuyenData : []);
      setXes(Array.isArray(xeData) ? xeData : []);
      setTuyenList(Array.isArray(tuyenData) ? tuyenData : []);
      setTaiXes(Array.isArray(txData) ? txData : []);
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setLoading(false);
    }
  }, [showToast]);

  useEffect(() => {
    void load();
  }, [load]);

  const xeOptions = useMemo(
    () =>
      xes
        .filter((xe) => xe.trangThai !== false)
        .map((xe) => ({ value: String(xe.maXe), label: xe.bienSoXe })),
    [xes],
  );

  const tuyenOptions = useMemo(
    () =>
      tuyenList.map((tuyen) => ({
        value: String(tuyen.maTuyen),
        label: `${tuyen.diemDi} → ${tuyen.diemDen}`,
      })),
    [tuyenList],
  );

  const taiXeOptions = useMemo(
    () =>
      taiXes
        .filter((tx) => tx.trangThai !== false)
        .map((tx) => ({ value: String(tx.maTx), label: tx.tenTx })),
    [taiXes],
  );

  const openCreate = () => {
    setEditing(null);
    setForm(EMPTY_FORM);
    setFormError('');
    setModalOpen(true);
  };

  const openEdit = (row: ChuyenXe) => {
    setEditing(row);
    setForm({
      maXe: String(row.maXe ?? ''),
      maTuyen: String(row.maTuyen ?? ''),
      maTx: String(row.maTx ?? ''),
      ngayGioKhoiHanh: toDateTimeInput(row.ngayGioKhoiHanh),
    });
    setFormError('');
    setModalOpen(true);
  };

  const setField = (name: keyof TripForm, value: string) => {
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async () => {
    if (!form.maXe || !form.maTuyen || !form.maTx || !form.ngayGioKhoiHanh) {
      setFormError('Vui lòng chọn xe, tuyến đường, tài xế và thời điểm khởi hành.');
      return;
    }
    const payload = {
      maXe: Number(form.maXe),
      maTuyen: Number(form.maTuyen),
      maTx: Number(form.maTx),
      ngayGioKhoiHanh: form.ngayGioKhoiHanh.length === 16 ? `${form.ngayGioKhoiHanh}:00` : form.ngayGioKhoiHanh,
    };

    setSaving(true);
    setFormError('');
    try {
      if (editing) {
        await api.put(`/chuyen-xe/${editing.maChuyen}`, payload);
        showToast('Đã cập nhật chuyến xe.');
      } else {
        await api.post('/chuyen-xe', payload);
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
    setDeleteBusy(true);
    try {
      await api.del(`/chuyen-xe/${deleting.maChuyen}`);
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
    return rows.filter((row) =>
      [row.diemDi, row.diemDen, row.bienSoXe, row.tenNhaXe, row.tenTx]
        .filter(Boolean)
        .some((value) => String(value).toLowerCase().includes(term)),
    );
  }, [rows, search]);

  const columns: DataTableColumn<ChuyenXe>[] = [
    {
      key: 'ngayGioKhoiHanh',
      label: 'Khởi hành',
      width: '150px',
      render: (row) => (
        <div className="cell-stack">
          <strong>{formatDateTime(row.ngayGioKhoiHanh)}</strong>
          <span className="text-muted">Đến {formatDateTime(row.ngayGioDenDuKien)}</span>
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
          <span className="text-muted">{row.tenNhaXe}</span>
        </div>
      ),
    },
    {
      key: 'xe',
      label: 'Xe / Loại xe',
      render: (row) => (
        <div className="cell-stack">
          <strong>{row.bienSoXe}</strong>
          <span className="text-muted">
            {row.tenLoaiXe} · {row.soCho} chỗ
          </span>
        </div>
      ),
    },
    { key: 'tenTx', label: 'Tài xế' },
    {
      key: 'giaVeThucTe',
      label: 'Giá vé',
      align: 'right',
      render: (row) => <strong className="text-primary">{formatVnd(row.giaVeThucTe)}</strong>,
    },
    {
      key: 'soVeDaBan',
      label: 'Đã bán',
      align: 'center',
      width: '90px',
      render: (row) => `${row.soVeDaBan ?? 0}/${row.soCho}`,
    },
    {
      key: 'trangThai',
      label: 'Trạng thái',
      width: '130px',
      render: (row) =>
        isUpcoming(row.ngayGioKhoiHanh) ? (
          <Badge tone="info">Sắp khởi hành</Badge>
        ) : (
          <Badge tone="neutral">Đã khởi hành</Badge>
        ),
    },
  ];

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>Chuyến xe</h1>
          <p className="page-subtitle">
            Lịch chạy của các xe. Giờ đến và giá vé được hệ thống tự tính từ tuyến đường và bảng giá.
          </p>
        </div>
        {isAdmin ? (
          <div className="page-actions">
            <button type="button" className="btn btn-primary" onClick={openCreate}>
              + Thêm chuyến xe
            </button>
          </div>
        ) : null}
      </div>

      <div className="card">
        <div className="card-toolbar">
          <input
            type="search"
            className="search-input"
            placeholder="Tìm theo tuyến, biển số xe, nhà xe, tài xế…"
            value={search}
            onChange={(event) => setSearch(event.target.value)}
          />
          <div className="toolbar-meta">
            <span className="text-muted">
              {filtered.length} / {rows.length} chuyến
            </span>
            <button type="button" className="btn btn-ghost btn-sm" onClick={() => void load()}>
              Làm mới
            </button>
          </div>
        </div>

        <DataTable
          columns={columns}
          rows={filtered}
          rowKey={(row) => row.maChuyen}
          loading={loading}
          emptyText="Chưa có chuyến xe nào."
          actions={
            isAdmin
              ? (row) => (
                  <>
                    <button type="button" className="btn btn-ghost btn-sm" onClick={() => openEdit(row)}>
                      Sửa
                    </button>
                    <button type="button" className="btn btn-danger-ghost btn-sm" onClick={() => setDeleting(row)}>
                      Xóa
                    </button>
                  </>
                )
              : undefined
          }
        />
      </div>

      <Modal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        title={editing ? `Sửa chuyến xe #${editing.maChuyen}` : 'Thêm chuyến xe'}
        subtitle={editing ? `${editing.diemDi} → ${editing.diemDen}` : 'Hệ thống tự tính giờ đến và giá vé.'}
        footer={
          <>
            <button type="button" className="btn btn-secondary" onClick={() => setModalOpen(false)}>
              Hủy
            </button>
            <button type="button" className="btn btn-primary" onClick={() => void handleSubmit()} disabled={saving}>
              {saving ? 'Đang lưu…' : 'Lưu'}
            </button>
          </>
        }
      >
        {formError ? <div className="alert alert-error">{formError}</div> : null}
        <form
          className="form-grid"
          onSubmit={(event) => {
            event.preventDefault();
            void handleSubmit();
          }}
        >
          <FormField
            label="Xe"
            type="select"
            value={form.maXe}
            onChange={(value) => setField('maXe', value)}
            options={xeOptions}
            required
          />
          <FormField
            label="Tuyến đường"
            type="select"
            value={form.maTuyen}
            onChange={(value) => setField('maTuyen', value)}
            options={tuyenOptions}
            required
          />
          <FormField
            label="Tài xế"
            type="select"
            value={form.maTx}
            onChange={(value) => setField('maTx', value)}
            options={taiXeOptions}
            required
          />
          <FormField
            label="Ngày giờ khởi hành"
            type="datetime-local"
            value={form.ngayGioKhoiHanh}
            onChange={(value) => setField('ngayGioKhoiHanh', value)}
            required
            help="Không được chọn thời điểm trong quá khứ."
          />
        </form>
      </Modal>

      <Modal
        open={Boolean(deleting)}
        onClose={() => setDeleting(null)}
        title="Xác nhận xóa chuyến xe"
        footer={
          <>
            <button type="button" className="btn btn-secondary" onClick={() => setDeleting(null)}>
              Hủy
            </button>
            <button type="button" className="btn btn-danger" onClick={() => void handleDelete()} disabled={deleteBusy}>
              {deleteBusy ? 'Đang xóa…' : 'Xóa'}
            </button>
          </>
        }
      >
        <p>
          Xóa chuyến xe <strong>#{deleting?.maChuyen}</strong> ({deleting?.diemDi} → {deleting?.diemDen})? Chỉ xóa
          được khi chuyến chưa có vé.
        </p>
      </Modal>
    </div>
  );
}
