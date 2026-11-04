import { CrudPage, refName, type CrudConfig } from '../components/CrudPage';
import { formatDate, gioiTinhLabel } from '../utils/format';

const config: CrudConfig = {
  title: 'Khách hàng',
  subtitle: 'Hồ sơ khách hàng đi xe, gắn tài khoản nếu khách tự đặt vé trực tuyến.',
  endpoint: 'khach-hang',
  idKey: 'maKh',
  writeRoles: ['Admin', 'NhanVien'],
  deleteRoles: ['Admin'],
  searchKeys: ['tenKh', 'sdt'],
  createLabel: 'Thêm khách hàng',
  searchPlaceholder: 'Tìm theo tên khách hàng, số điện thoại…',
  refs: [{ key: 'tai-khoan', endpoint: 'tai-khoan' }],
  columns: [
    { key: 'maKh', label: 'Mã', width: '70px' },
    { key: 'tenKh', label: 'Họ tên' },
    { key: 'namSinh', label: 'Ngày sinh', width: '120px', render: (row) => formatDate(row.namSinh) },
    { key: 'gioiTinh', label: 'Giới tính', width: '100px', render: (row) => gioiTinhLabel(row.gioiTinh) },
    { key: 'sdt', label: 'Số điện thoại', width: '140px' },
    {
      key: 'maTk',
      label: 'Tài khoản',
      width: '150px',
      render: (row, refs) =>
        row.maTk ? refName(refs, 'tai-khoan', 'maTk', row.maTk, 'tenDangNhap') : <span className="text-muted">Chưa có</span>,
    },
  ],
  fields: [
    { name: 'tenKh', label: 'Họ tên khách hàng', required: true, span: 2 },
    { name: 'namSinh', label: 'Ngày sinh', type: 'date' },
    {
      name: 'gioiTinh',
      label: 'Giới tính',
      type: 'select',
      asBoolean: true,
      options: [
        { value: 'true', label: 'Nam' },
        { value: 'false', label: 'Nữ' },
      ],
    },
    { name: 'sdt', label: 'Số điện thoại', placeholder: '09xxxxxxxx' },
    {
      name: 'maTk',
      label: 'Tài khoản liên kết',
      type: 'select',
      asNumber: true,
      placeholder: 'Không liên kết',
      help: 'Chỉ chọn khi khách hàng có tài khoản đăng nhập.',
      options: (refs) =>
        (refs['tai-khoan'] ?? []).map((item) => ({
          value: String(item.maTk),
          label: `${item.tenDangNhap} · ${item.email ?? ''}`,
        })),
    },
  ],
};

export function Customers() {
  return <CrudPage config={config} />;
}
