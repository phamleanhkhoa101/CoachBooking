import { Badge, StatusBadge } from '../../../components/common/Badge';
import { CrudPage, refName, type CrudConfig } from '../../../components/table/CrudPage';

const config: CrudConfig = {
  title: 'Tài khoản',
  subtitle: 'Tài khoản đăng nhập hệ thống và quyền tương ứng.',
  endpoint: 'accounts',
  idKey: 'accountId',
  searchKeys: ['username', 'email', 'tenDangNhap'],
  createLabel: 'Thêm tài khoản',
  searchPlaceholder: 'Tìm theo tên đăng nhập, email…',
  refs: [{ key: 'roles', endpoint: 'roles' }],
  transform: (payload, isEdit) => {
    if (isEdit) {
      delete payload.username;
      if (!payload.password) {
        delete payload.password;
      }
    }
    return payload;
  },
  columns: [
    {
      key: 'accountId',
      label: 'Mã',
      width: '70px',
      render: (row) => row.accountId ?? row.maTk,
    },
    {
      key: 'username',
      label: 'Tên đăng nhập',
      render: (row) => row.username ?? row.tenDangNhap,
    },
    { key: 'email', label: 'Email' },
    {
      key: 'roleId',
      label: 'Quyền',
      width: '150px',
      render: (row, refs) => {
        const id = row.roleId ?? row.maQuyen;
        const ten =
          refName(refs, 'roles', 'roleId', id, 'roleName') !== '—'
            ? refName(refs, 'roles', 'roleId', id, 'roleName')
            : refName(refs, 'roles', 'maQuyen', id, 'tenQuyen');
        return (
          <Badge tone={id === 1 ? 'info' : id === 2 ? 'success' : 'neutral'}>
            {ten}
          </Badge>
        );
      },
    },
    {
      key: 'isActive',
      label: 'Trạng thái',
      width: '130px',
      render: (row) => (
        <StatusBadge
          active={Boolean(row.isActive ?? row.trangThai)}
          trueLabel="Đang mở"
          falseLabel="Đã khóa"
        />
      ),
    },
  ],
  fields: [
    { name: 'username', label: 'Tên đăng nhập', required: true, placeholder: 'Ít nhất 6 ký tự', hideOnEdit: true },
    {
      name: 'password',
      label: 'Mật khẩu',
      type: 'password',
      requiredOnCreate: true,
      help: 'Khi sửa: để trống nếu không đổi mật khẩu.',
    },
    { name: 'email', label: 'Email', type: 'email', required: true, placeholder: 'nguoidung@coachbooking.com' },
    {
      name: 'roleId',
      label: 'Quyền',
      type: 'select',
      required: true,
      asNumber: true,
      options: (refs) =>
        (refs['roles'] ?? []).map((item) => ({
          value: String(item.roleId ?? item.maQuyen),
          label: `${item.roleName ?? item.tenQuyen}${
            item.description ?? item.moTa ? ` · ${item.description ?? item.moTa}` : ''
          }`,
        })),
    },
    { name: 'isActive', label: 'Đang hoạt động', type: 'checkbox' },
  ],
};

export function Accounts() {
  return <CrudPage config={config} />;
}
