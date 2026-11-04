import { useCallback, useEffect, useMemo, useState, type ReactNode } from 'react';
import { api, getErrorMessage } from '../api/client';
import type { Quyen, Row } from '../api/types';
import { useAuth } from '../auth/AuthContext';
import { DataTable, type DataTableColumn } from './DataTable';
import { FormField, type FormFieldType, type SelectOption } from './FormField';
import { Modal } from './Modal';
import { useToast } from './Toast';

export type CrudRefs = Record<string, Row[]>;

export interface CrudRefDef {
  /** Khóa dùng trong `refs` (ví dụ 'nha-xe'). */
  key: string;
  /** Endpoint tương ứng (ví dụ 'nha-xe' -> GET /api/nha-xe). */
  endpoint: string;
}

export interface CrudFieldDef {
  name: string;
  label: string;
  type?: FormFieldType;
  required?: boolean;
  /** Bắt buộc khi tạo mới (ví dụ mật khẩu tài khoản). */
  requiredOnCreate?: boolean;
  placeholder?: string;
  help?: string;
  options?: SelectOption[] | ((refs: CrudRefs) => SelectOption[]);
  min?: number;
  max?: number;
  /** Ép kiểu số khi gửi (dùng cho ô select khóa ngoại dạng mã số). */
  asNumber?: boolean;
  /** Ép kiểu boolean khi gửi (dùng cho ô select Nam/Nữ). */
  asBoolean?: boolean;
  hideOnCreate?: boolean;
  hideOnEdit?: boolean;
  span?: 1 | 2;
}

export interface CrudColumnDef {
  key: string;
  label: string;
  width?: string;
  align?: 'left' | 'right' | 'center';
  render?: (row: Row, refs: CrudRefs) => ReactNode;
}

export interface CrudConfig {
  title: string;
  subtitle?: string;
  endpoint: string;
  idKey: string;
  columns: CrudColumnDef[];
  fields: CrudFieldDef[];
  refs?: CrudRefDef[];
  searchKeys?: string[];
  searchPlaceholder?: string;
  writeRoles?: Quyen[];
  deleteRoles?: Quyen[];
  createLabel?: string;
  emptyText?: string;
  defaults?: Record<string, string>;
  /** Chỉnh payload trước khi gửi (ví dụ bỏ mật khẩu rỗng khi sửa tài khoản). */
  transform?: (payload: Record<string, unknown>, isEdit: boolean) => Record<string, unknown>;
  /** Tùy biến dữ liệu đưa vào form khi sửa. */
  toForm?: (row: Row) => Record<string, string>;
}

/** Lấy tên hiển thị từ danh sách tham chiếu đã tải. */
export function refName(
  refs: CrudRefs,
  refKey: string,
  idKey: string,
  id: unknown,
  labelKey: string,
): string {
  const list = refs[refKey] ?? [];
  const found = list.find((item) => item[idKey] === id);
  if (!found) return '—';
  const value = found[labelKey];
  return value === null || value === undefined || value === '' ? '—' : String(value);
}

function isNeedWrite(quyen: Quyen | undefined, roles: Quyen[] | undefined): boolean {
  if (!quyen) return false;
  if (roles && roles.length > 0) return roles.includes(quyen);
  return quyen === 'Admin';
}

export function CrudPage({ config }: { config: CrudConfig }) {
  const { nguoiDung } = useAuth();
  const { showToast } = useToast();

  const [rows, setRows] = useState<Row[]>([]);
  const [refs, setRefs] = useState<CrudRefs>({});
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');

  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<Row | null>(null);
  const [form, setForm] = useState<Record<string, string>>({});
  const [formError, setFormError] = useState('');
  const [saving, setSaving] = useState(false);
  const [deleting, setDeleting] = useState<Row | null>(null);
  const [deleteBusy, setDeleteBusy] = useState(false);

  const canWrite = isNeedWrite(nguoiDung?.quyen, config.writeRoles);
  const canDelete = isNeedWrite(nguoiDung?.quyen, config.deleteRoles);

  const loadRows = useCallback(async () => {
    setLoading(true);
    try {
      const data = await api.get<Row[]>(`/${config.endpoint}`);
      setRows(Array.isArray(data) ? data : []);
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setLoading(false);
    }
  }, [config.endpoint, showToast]);

  const loadRefs = useCallback(async () => {
    if (!config.refs || config.refs.length === 0) return;
    const pairs = await Promise.all(
      config.refs.map(async (def) => {
        try {
          const data = await api.get<Row[]>(`/${def.endpoint}`);
          return [def.key, Array.isArray(data) ? data : []] as const;
        } catch {
          return [def.key, [] as Row[]] as const;
        }
      }),
    );
    setRefs(Object.fromEntries(pairs));
  }, [config.refs]);

  useEffect(() => {
    void loadRows();
  }, [loadRows]);

  useEffect(() => {
    void loadRefs();
  }, [loadRefs]);

  const buildDefaults = useCallback((): Record<string, string> => {
    const next: Record<string, string> = { ...(config.defaults ?? {}) };
    config.fields.forEach((field) => {
      if (next[field.name] === undefined) {
        next[field.name] = field.type === 'checkbox' ? 'true' : '';
      }
    });
    return next;
  }, [config.defaults, config.fields]);

  const rowToForm = useCallback(
    (row: Row): Record<string, string> => {
      if (config.toForm) return config.toForm(row);
      const next: Record<string, string> = {};
      config.fields.forEach((field) => {
        const raw = row[field.name];
        if (raw === null || raw === undefined) next[field.name] = '';
        else if (typeof raw === 'boolean') next[field.name] = raw ? 'true' : 'false';
        else next[field.name] = String(raw);
      });
      return next;
    },
    [config],
  );

  const openCreate = () => {
    setEditing(null);
    setForm(buildDefaults());
    setFormError('');
    setModalOpen(true);
  };

  const openEdit = (row: Row) => {
    setEditing(row);
    setForm(rowToForm(row));
    setFormError('');
    setModalOpen(true);
  };

  const setValue = (name: string, value: string) => {
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const visibleFields = config.fields.filter((field) =>
    editing ? !field.hideOnEdit : !field.hideOnCreate,
  );

  const handleSubmit = async () => {
    for (const field of visibleFields) {
      const value = (form[field.name] ?? '').trim();
      const required = field.required || (!editing && field.requiredOnCreate);
      if (required && !value) {
        setFormError(`Vui lòng nhập ${field.label.toLowerCase()}.`);
        return;
      }
    }

    const payload: Record<string, unknown> = {};
    visibleFields.forEach((field) => {
      const raw = form[field.name] ?? '';
      if (field.type === 'checkbox' || field.asBoolean) {
        payload[field.name] = raw === 'true';
      } else if (field.type === 'number' || field.asNumber) {
        payload[field.name] = raw === '' ? null : Number(raw);
      } else {
        payload[field.name] = raw === '' ? null : raw;
      }
    });

    const body = config.transform ? config.transform(payload, Boolean(editing)) : payload;

    setSaving(true);
    setFormError('');
    try {
      if (editing) {
        await api.put(`/${config.endpoint}/${editing[config.idKey]}`, body);
        showToast('Đã cập nhật dữ liệu.');
      } else {
        await api.post(`/${config.endpoint}`, body);
        showToast('Đã thêm mới dữ liệu.');
      }
      setModalOpen(false);
      await loadRows();
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
      await api.del(`/${config.endpoint}/${deleting[config.idKey]}`);
      showToast('Đã xóa dữ liệu.');
      setDeleting(null);
      await loadRows();
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setDeleteBusy(false);
    }
  };

  const filtered = useMemo(() => {
    const term = search.trim().toLowerCase();
    if (!term) return rows;
    const keys = config.searchKeys ?? config.columns.map((col) => col.key);
    return rows.filter((row) =>
      keys.some((key) => String(row[key] ?? '').toLowerCase().includes(term)),
    );
  }, [rows, search, config.searchKeys, config.columns]);

  const tableColumns: DataTableColumn<Row>[] = config.columns.map((col) => {
    const render = col.render;
    return {
      key: col.key,
      label: col.label,
      width: col.width,
      align: col.align,
      render: render ? (row: Row) => render(row, refs) : undefined,
    };
  });

  const showActions = canWrite || canDelete;

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>{config.title}</h1>
          {config.subtitle ? <p className="page-subtitle">{config.subtitle}</p> : null}
        </div>
        <div className="page-actions">
          {canWrite ? (
            <button type="button" className="btn btn-primary" onClick={openCreate}>
              + {config.createLabel ?? 'Thêm mới'}
            </button>
          ) : null}
        </div>
      </div>

      <div className="card">
        <div className="card-toolbar">
          <input
            type="search"
            className="search-input"
            placeholder={config.searchPlaceholder ?? 'Tìm kiếm…'}
            value={search}
            onChange={(event) => setSearch(event.target.value)}
          />
          <div className="toolbar-meta">
            <span className="text-muted">
              {filtered.length} / {rows.length} bản ghi
            </span>
            <button type="button" className="btn btn-ghost btn-sm" onClick={() => void loadRows()}>
              Làm mới
            </button>
          </div>
        </div>

        <DataTable
          columns={tableColumns}
          rows={filtered}
          rowKey={(row) => String(row[config.idKey])}
          loading={loading}
          emptyText={config.emptyText ?? 'Chưa có dữ liệu.'}
          actions={
            showActions
              ? (row) => (
                  <>
                    {canWrite ? (
                      <button type="button" className="btn btn-ghost btn-sm" onClick={() => openEdit(row)}>
                        Sửa
                      </button>
                    ) : null}
                    {canDelete ? (
                      <button type="button" className="btn btn-danger-ghost btn-sm" onClick={() => setDeleting(row)}>
                        Xóa
                      </button>
                    ) : null}
                  </>
                )
              : undefined
          }
        />
      </div>

      <Modal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        title={editing ? `Sửa ${config.title.toLowerCase()}` : config.createLabel ?? 'Thêm mới'}
        subtitle={editing ? `Mã: ${String(editing[config.idKey])}` : undefined}
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
          {visibleFields.map((field) => (
            <FormField
              key={field.name}
              label={field.label}
              value={form[field.name] ?? ''}
              onChange={(value) => setValue(field.name, value)}
              type={field.type}
              options={
                typeof field.options === 'function' ? field.options(refs) : field.options
              }
              placeholder={field.placeholder}
              required={field.required || (!editing && field.requiredOnCreate)}
              help={field.help}
              span={field.span}
              min={field.min}
              max={field.max}
            />
          ))}
        </form>
      </Modal>

      <Modal
        open={Boolean(deleting)}
        onClose={() => setDeleting(null)}
        title="Xác nhận xóa"
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
          Bạn có chắc muốn xóa bản ghi <strong>{deleting ? String(deleting[config.idKey]) : ''}</strong> không? Hành
          động này không thể hoàn tác.
        </p>
      </Modal>
    </div>
  );
}
