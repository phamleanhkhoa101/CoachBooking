import { useCallback, useEffect, useMemo, useState, type ReactNode } from 'react';
import { api, getErrorMessage } from '../../api/client';
import type { Role, Row } from '../../api/types';
import { useAuth } from '../../auth/AuthContext';
import { Button } from '../common/Button';
import { StatusBadge } from '../common/Badge';
import { DataTable, type DataTableColumn } from './DataTable';
import { FormField, type FormFieldType, type SelectOption } from '../common/FormField';
import { Modal } from '../common/Modal';
import { useToast } from '../common/Toast';

export type CrudRefs = Record<string, Row[]>;

export interface CrudRefDef {
  /** Khóa dùng trong `refs` (ví dụ 'bus-companies'). */
  key: string;
  /** Endpoint tương ứng (ví dụ 'bus-companies' -> GET /api/bus-companies). */
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
  sortable?: boolean;
  render?: (row: Row, refs: CrudRefs) => ReactNode;
}

export interface CrudExtraAction {
  label: string;
  variant?: 'primary' | 'secondary' | 'ghost' | 'danger';
  /** Chế độ modal nào hiển thị nút này (mặc định: ['view', 'edit']). */
  modes?: Array<'view' | 'edit'>;
  onClick: (row: Row) => void;
}

export interface CrudConfig {
  title: string;
  subtitle?: string;
  endpoint: string;
  idKey: string;
  columns: CrudColumnDef[];
  fields: CrudFieldDef[];
  refs?: CrudRefDef[];
  /** Nút hành động bổ sung trong footer của modal (ngoài Lưu / Đóng). */
  extraActions?: CrudExtraAction[];
  searchKeys?: string[];
  searchPlaceholder?: string;
  writeRoles?: Role[];
  deleteRoles?: Role[];
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

function isNeedWrite(role: Role | undefined, roles: Role[] | undefined): boolean {
  if (!role) return false;
  if (roles && roles.length > 0) return roles.includes(role);
  return role === 'Admin';
}

export function CrudPage({ config }: { config: CrudConfig }) {
  const { user } = useAuth();
  const { showToast } = useToast();

  const [rows, setRows] = useState<Row[]>([]);
  const [refs, setRefs] = useState<CrudRefs>({});
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');

  // Modal and mode state: 'create' | 'view' | 'edit'
  const [modalOpen, setModalOpen] = useState(false);
  const [modalMode, setModalMode] = useState<'create' | 'view' | 'edit'>('create');
  const [selectedRow, setSelectedRow] = useState<Row | null>(null);
  const [editing, setEditing] = useState<Row | null>(null);

  const [form, setForm] = useState<Record<string, string>>({});
  const [formError, setFormError] = useState('');
  const [saving, setSaving] = useState(false);
  const [deleting, setDeleting] = useState<Row | null>(null);
  const [deleteBusy, setDeleteBusy] = useState(false);

  const canWrite = isNeedWrite(user?.role, config.writeRoles);
  const canDelete = isNeedWrite(user?.role, config.deleteRoles);

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
    setSelectedRow(null);
    setEditing(null);
    setForm(buildDefaults());
    setFormError('');
    setModalMode('create');
    setModalOpen(true);
  };

  const openView = (row: Row) => {
    setSelectedRow(row);
    setEditing(row);
    setForm(rowToForm(row));
    setFormError('');
    setModalMode('view');
    setModalOpen(true);
  };

  const openEdit = (row: Row) => {
    setSelectedRow(row);
    setEditing(row);
    setForm(rowToForm(row));
    setFormError('');
    setModalMode('edit');
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
        showToast('Đã cập nhật dữ liệu thành công.');
      } else {
        await api.post(`/${config.endpoint}`, body);
        showToast('Đã thêm mới dữ liệu thành công.');
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
      showToast('Đã xóa dữ liệu thành công.');
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
      sortable: col.sortable,
      render: render ? (row: Row) => render(row, refs) : undefined,
    };
  });

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>{config.title}</h1>
          {config.subtitle ? <p className="page-subtitle">{config.subtitle}</p> : null}
        </div>
        <div className="page-actions">
          {canWrite ? (
            <Button variant="primary" onClick={openCreate}>
              + {config.createLabel ?? 'Thêm mới'}
            </Button>
          ) : null}
        </div>
      </div>

      <div className="card">
        <div className="card-toolbar">
          <input
            type="search"
            className="search-input"
            placeholder={config.searchPlaceholder ?? 'Tìm kiếm theo từ khóa…'}
            value={search}
            onChange={(event) => setSearch(event.target.value)}
          />
          <div className="toolbar-meta">
            <span className="text-muted">
              {filtered.length} / {rows.length} bản ghi
            </span>
            <Button variant="ghost" size="sm" onClick={() => void loadRows()}>
              Làm mới
            </Button>
          </div>
        </div>

        <DataTable
          columns={tableColumns}
          rows={filtered}
          rowKey={(row) => String(row[config.idKey])}
          loading={loading}
          emptyText={config.emptyText ?? 'Chưa có dữ liệu.'}
          onRowClick={(row) => openView(row)}
          actions={(row) => (
            <>
              <button
                type="button"
                className="btn btn-ghost btn-sm"
                onClick={() => openView(row)}
                title="Xem chi tiết"
              >
                Xem
              </button>
              {canWrite ? (
                <button
                  type="button"
                  className="btn btn-ghost btn-sm"
                  onClick={() => openEdit(row)}
                  title="Chỉnh sửa"
                >
                  Sửa
                </button>
              ) : null}
              {canDelete ? (
                <button
                  type="button"
                  className="btn btn-danger-ghost btn-sm"
                  onClick={() => setDeleting(row)}
                  title="Xóa bản ghi"
                >
                  Xóa
                </button>
              ) : null}
            </>
          )}
        />
      </div>

      {/* VIEW / EDIT / CREATE MODAL */}
      <Modal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        title={
          modalMode === 'view'
            ? `Chi tiết ${config.title.toLowerCase()}`
            : modalMode === 'edit'
            ? `Sửa ${config.title.toLowerCase()}`
            : config.createLabel ?? 'Thêm mới'
        }
        subtitle={
          editing ? `Mã định danh: ${String(editing[config.idKey])}` : undefined
        }
        footer={
          modalMode === 'view' ? (
            <>
              <Button variant="secondary" onClick={() => setModalOpen(false)}>
                Đóng
              </Button>
              {/* Nút hành động bổ sung (ví dụ: Đổi mật khẩu) */}
              {config.extraActions
                ?.filter((a) => !a.modes || a.modes.includes('view'))
                .map((a) => (
                  <Button
                    key={a.label}
                    variant={a.variant ?? 'ghost'}
                    onClick={() => selectedRow && a.onClick(selectedRow)}
                  >
                    {a.label}
                  </Button>
                ))}
              {canWrite ? (
                <Button variant="primary" onClick={() => setModalMode('edit')}>
                  Chỉnh sửa
                </Button>
              ) : null}
            </>
          ) : modalMode === 'edit' ? (
            <>
              <Button
                variant="secondary"
                disabled={saving}
                onClick={() => {
                  if (selectedRow) {
                    setForm(rowToForm(selectedRow));
                  }
                  setModalMode('view');
                }}
              >
                Quay lại
              </Button>
              {/* Nút hành động bổ sung trong chế độ sửa */}
              {config.extraActions
                ?.filter((a) => !a.modes || a.modes.includes('edit'))
                .map((a) => (
                  <Button
                    key={a.label}
                    variant={a.variant ?? 'ghost'}
                    disabled={saving}
                    onClick={() => selectedRow && a.onClick(selectedRow)}
                  >
                    {a.label}
                  </Button>
                ))}
              <Button
                variant="primary"
                loading={saving}
                disabled={saving}
                onClick={() => void handleSubmit()}
              >
                Lưu thay đổi
              </Button>
            </>
          ) : (
            <>
              <Button
                variant="secondary"
                disabled={saving}
                onClick={() => setModalOpen(false)}
              >
                Hủy
              </Button>
              <Button
                variant="primary"
                loading={saving}
                disabled={saving}
                onClick={() => void handleSubmit()}
              >
                Tạo mới
              </Button>
            </>
          )
        }
      >
        {formError ? <div className="alert alert-error">{formError}</div> : null}

        {modalMode === 'view' && selectedRow ? (
          <div className="detail-grid">
            {config.fields.map((field) => {
              const rawVal = selectedRow[field.name];
              let displayNode: ReactNode = '—';

              // Password → luôn ẩn trong View Mode
              if (field.type === 'password' || field.name === 'password' || field.name === 'matKhau') {
                displayNode = <span style={{ letterSpacing: '0.15em', color: 'var(--text-muted)' }}>••••••••</span>;
              // Field có options (select) → ưu tiên tra cứu nhãn trước
              } else if (field.options) {
                const opts =
                  typeof field.options === 'function'
                    ? field.options(refs)
                    : field.options;
                const matched = opts.find((o) => String(o.value) === String(rawVal));
                displayNode = matched
                  ? matched.label
                  : rawVal !== null && rawVal !== undefined
                  ? String(rawVal)
                  : '—';
              // Checkbox thuần (không có options) → StatusBadge
              } else if (field.type === 'checkbox') {
                displayNode = <StatusBadge active={Boolean(rawVal)} />;
              // asBoolean nhưng không có options → Nam/Nữ fallback
              } else if (field.asBoolean) {
                displayNode = rawVal === true || rawVal === 'true' ? 'Nam' : 'Nữ';
              } else if (rawVal !== null && rawVal !== undefined && rawVal !== '') {
                displayNode = String(rawVal);
              }

              return (
                <div
                  key={field.name}
                  className={`detail-item ${field.span === 2 ? 'full-width' : ''}`}
                >
                  <span className="detail-label">{field.label}</span>
                  <div className="detail-value">{displayNode}</div>
                </div>
              );
            })}
          </div>
        ) : (
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
                  typeof field.options === 'function'
                    ? field.options(refs)
                    : field.options
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
        )}
      </Modal>

      {/* DELETE CONFIRMATION MODAL */}
      <Modal
        open={Boolean(deleting)}
        onClose={() => setDeleting(null)}
        title="Xác nhận xóa bản ghi"
        footer={
          <>
            <Button
              variant="secondary"
              onClick={() => setDeleting(null)}
              disabled={deleteBusy}
            >
              Hủy
            </Button>
            <Button
              variant="danger"
              loading={deleteBusy}
              disabled={deleteBusy}
              onClick={() => void handleDelete()}
            >
              Xóa bản ghi
            </Button>
          </>
        }
      >
        <p>
          Bạn có chắc chắn muốn xóa bản ghi{' '}
          <strong>{deleting ? String(deleting[config.idKey]) : ''}</strong> không?
          Hành động này không thể hoàn tác và có thể ảnh hưởng đến các dữ liệu liên quan.
        </p>
      </Modal>
    </div>
  );
}
