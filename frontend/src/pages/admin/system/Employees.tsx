import { StatusBadge } from '../../../components/common/Badge';
import { CrudPage, refName, type CrudConfig } from '../../../components/table/CrudPage';
import { formatDate, gioiTinhLabel } from '../../../utils/format';

const config: CrudConfig = {
  title: 'Nhân viên',
  subtitle: 'Hồ sơ nhân viên bán vé và tài khoản đăng nhập tương ứng.',
  endpoint: 'nhan-vien',
  idKey: 'maNv',
  searchKeys: ['tenNv', 'sdt', 'diaChi'],
  createLabel: 'Thêm nhân viên',
  searchPlaceholder: 'Tìm theo tên, số điện thoại, địa chỉ…',
  refs: [{ key: 'tai-khoan', endpoint: 'tai-khoan' }],
  columns: [
    { key: 'maNv', label: 'Mã', width: '70px' },
    { key: 'tenNv', label: 'Họ tên' },
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
    {
      key: 'trangThai',
      label: 'Trạng thái',
      width: '130px',
      render: (row) => <StatusBadge active={Boolean(row.trangThai)} trueLabel="Đang làm" falseLabel="Nghỉ" />,
    },
  ],
  fields: [
    { name: 'tenNv', label: 'Họ tên nhân viên', required: true, span: 2 },
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
    { name: 'diaChi', label: 'Địa chỉ', span: 2 },
    {
      name: 'maTk',
      label: 'Tài khoản liên kết',
      type: 'select',
      asNumber: true,
      placeholder: 'Không liên kết',
      options: (refs) =>
        (refs['tai-khoan'] ?? []).map((item) => ({
          value: String(item.maTk),
          label: `${item.tenDangNhap} · ${item.email ?? ''}`,
        })),
    },
    { name: 'trangThai', label: 'Đang làm việc', type: 'checkbox' },
  ],
};

export function Employees() {
  return <CrudPage config={config} />;
}
