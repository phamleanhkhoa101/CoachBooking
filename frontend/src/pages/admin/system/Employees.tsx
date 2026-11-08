import { StatusBadge } from '../../../components/common/Badge';
import { CrudPage, refName, type CrudConfig } from '../../../components/table/CrudPage';
import { formatDate, gioiTinhLabel } from '../../../utils/format';

const config: CrudConfig = {
  title: 'Nhân viên',
  subtitle: 'Hồ sơ nhân viên bán vé và tài khoản đăng nhập tương ứng.',
  endpoint: 'employees',
  idKey: 'employeeId',
  searchKeys: ['fullName', 'phone', 'address', 'tenNv', 'sdt', 'diaChi'],
  createLabel: 'Thêm nhân viên',
  searchPlaceholder: 'Tìm theo tên, số điện thoại, địa chỉ…',
  refs: [{ key: 'accounts', endpoint: 'accounts' }],
  columns: [
    {
      key: 'employeeId',
      label: 'Mã',
      width: '70px',
      render: (row) => row.employeeId ?? row.maNv,
    },
    {
      key: 'fullName',
      label: 'Họ tên',
      render: (row) => row.fullName ?? row.tenNv,
    },
    {
      key: 'dateOfBirth',
      label: 'Ngày sinh',
      width: '120px',
      render: (row) => formatDate(row.dateOfBirth ?? row.namSinh),
    },
    {
      key: 'gender',
      label: 'Giới tính',
      width: '100px',
      render: (row) => gioiTinhLabel(row.gender ?? row.gioiTinh),
    },
    {
      key: 'phone',
      label: 'Số điện thoại',
      width: '140px',
      render: (row) => row.phone ?? row.sdt ?? '—',
    },
    {
      key: 'accountId',
      label: 'Tài khoản',
      width: '150px',
      render: (row, refs) => {
        const id = row.accountId ?? row.maTk;
        if (!id) return <span className="text-muted">Chưa có</span>;
        const name = refName(refs, 'accounts', 'accountId', id, 'username');
        return name !== '—' ? name : refName(refs, 'accounts', 'maTk', id, 'tenDangNhap');
      },
    },
    {
      key: 'isActive',
      label: 'Trạng thái',
      width: '130px',
      render: (row) => (
        <StatusBadge
          active={Boolean(row.isActive ?? row.trangThai)}
          trueLabel="Đang làm"
          falseLabel="Nghỉ"
        />
      ),
    },
  ],
  fields: [
    { name: 'fullName', label: 'Họ tên nhân viên', required: true, span: 2 },
    { name: 'dateOfBirth', label: 'Ngày sinh', type: 'date', required: true },
    {
      name: 'gender',
      label: 'Giới tính',
      type: 'select',
      asBoolean: true,
      options: [
        { value: 'true', label: 'Nam' },
        { value: 'false', label: 'Nữ' },
      ],
    },
    { name: 'phone', label: 'Số điện thoại', placeholder: '09xxxxxxxx', required: true },
    { name: 'address', label: 'Địa chỉ', span: 2, required: true },
    {
      name: 'accountId',
      label: 'Tài khoản liên kết',
      type: 'select',
      asNumber: true,
      placeholder: 'Không liên kết',
      options: (refs) =>
        (refs['accounts'] ?? []).map((item) => ({
          value: String(item.accountId ?? item.maTk),
          label: `${item.username ?? item.tenDangNhap} · ${item.email ?? ''}`,
        })),
    },
    { name: 'isActive', label: 'Đang làm việc', type: 'checkbox' },
  ],
};

export function Employees() {
  return <CrudPage config={config} />;
}
