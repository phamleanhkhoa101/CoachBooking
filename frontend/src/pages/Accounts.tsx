import { Badge, StatusBadge } from '../components/Badge';
import { CrudPage, refName, type CrudConfig } from '../components/CrudPage';

const config: CrudConfig = {
  title: 'Tài khoản',
  subtitle: 'Tài khoản đăng nhập hệ thống và quyền tương ứng.',
  endpoint: 'tai-khoan',
  idKey: 'maTk',
  searchKeys: ['tenDangNhap', 'email'],
  createLabel: 'Thêm tài khoản',
  searchPlaceholder: 'Tìm theo tên đăng nhập, email…',
  refs: [{ key: 'quyen', endpoint: 'quyen' }],
  transform: (payload, isEdit) => {
    if (isEdit && !payload.matKhau) {
      delete payload.matKhau;
    }
    return payload;
  },
  columns: [
    { key: 'maTk', label: 'Mã', width: '70px' },
    { key: 'tenDangNhap', label: 'Tên đăng nhập' },
    { key: 'email', label: 'Email' },
    {
      key: 'maQuyen',
      label: 'Quyền',
      width: '150px',
      render: (row, refs) => {
        const ten = refName(refs, 'quyen', 'maQuyen', row.maQuyen, 'tenQuyen');
        return <Badge tone={row.maQuyen === 1 ? 'info' : row.maQuyen === 2 ? 'success' : 'neutral'}>{ten}</Badge>;
      },
    },
    {
      key: 'trangThai',
      label: 'Trạng thái',
      width: '130px',
      render: (row) => <StatusBadge active={Boolean(row.trangThai)} trueLabel="Đang mở" falseLabel="Đã khóa" />,
    },
  ],
  fields: [
    { name: 'tenDangNhap', label: 'Tên đăng nhập', required: true, placeholder: 'Ít nhất 6 ký tự' },
    {
      name: 'matKhau',
      label: 'Mật khẩu',
      type: 'password',
      requiredOnCreate: true,
      help: 'Khi sửa: để trống nếu không đổi mật khẩu.',
    },
    { name: 'email', label: 'Email', type: 'email', required: true, placeholder: 'nguoidung@qlbxmd.vn' },
    {
      name: 'maQuyen',
      label: 'Quyền',
      type: 'select',
      required: true,
      asNumber: true,
      options: (refs) =>
        (refs['quyen'] ?? []).map((item) => ({
          value: String(item.maQuyen),
          label: `${item.tenQuyen}${item.moTa ? ` · ${item.moTa}` : ''}`,
        })),
    },
    { name: 'trangThai', label: 'Đang hoạt động', type: 'checkbox' },
  ],
};

export function Accounts() {
  return <CrudPage config={config} />;
}
