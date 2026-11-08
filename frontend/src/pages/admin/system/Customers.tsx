import { CrudPage, refName, type CrudConfig } from '../../../components/table/CrudPage';
import { formatDate, gioiTinhLabel } from '../../../utils/format';

const config: CrudConfig = {
  title: 'Khách hàng',
  subtitle: 'Hồ sơ khách hàng đi xe, gắn tài khoản nếu khách tự đặt vé trực tuyến.',
  endpoint: 'customers',
  idKey: 'customerId',
  writeRoles: ['Admin', 'NhanVien'],
  deleteRoles: ['Admin'],
  searchKeys: ['fullName', 'phone', 'tenKh', 'sdt'],
  createLabel: 'Thêm khách hàng',
  searchPlaceholder: 'Tìm theo tên khách hàng, số điện thoại…',
  refs: [{ key: 'accounts', endpoint: 'accounts' }],
  columns: [
    {
      key: 'customerId',
      label: 'Mã',
      width: '70px',
      render: (row) => row.customerId ?? row.maKh,
    },
    {
      key: 'fullName',
      label: 'Họ tên',
      render: (row) => row.fullName ?? row.tenKh,
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
  ],
  fields: [
    { name: 'fullName', label: 'Họ tên khách hàng', required: true, span: 2 },
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
    {
      name: 'accountId',
      label: 'Tài khoản liên kết',
      type: 'select',
      asNumber: true,
      placeholder: 'Không liên kết',
      help: 'Chỉ chọn khi khách hàng có tài khoản đăng nhập.',
      options: (refs) =>
        (refs['accounts'] ?? []).map((item) => ({
          value: String(item.accountId ?? item.maTk),
          label: `${item.username ?? item.tenDangNhap} · ${item.email ?? ''}`,
        })),
    },
  ],
};

export function Customers() {
  return <CrudPage config={config} />;
}
