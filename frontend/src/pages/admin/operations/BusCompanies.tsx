import { StatusBadge } from '../../../components/common/Badge';
import { CrudPage, type CrudConfig } from '../../../components/table/CrudPage';

const config: CrudConfig = {
  title: 'Nhà xe',
  subtitle: 'Danh sách các nhà xe đang hợp tác khai thác tuyến.',
  endpoint: 'bus-companies',
  idKey: 'companyId',
  searchKeys: ['companyName', 'phone', 'email', 'tenNx', 'sdt'],
  createLabel: 'Thêm nhà xe',
  searchPlaceholder: 'Tìm theo tên nhà xe, số điện thoại, email…',
  columns: [
    {
      key: 'companyId',
      label: 'Mã',
      width: '70px',
      render: (row) => row.companyId ?? row.maNx,
    },
    {
      key: 'companyName',
      label: 'Tên nhà xe',
      render: (row) => row.companyName ?? row.tenNx,
    },
    {
      key: 'phone',
      label: 'Số điện thoại',
      width: '150px',
      render: (row) => row.phone ?? row.sdt ?? '—',
    },
    { key: 'email', label: 'Email', render: (row) => row.email ?? '—' },
    {
      key: 'isActive',
      label: 'Trạng thái',
      width: '130px',
      render: (row) => <StatusBadge active={Boolean(row.isActive ?? row.trangThai)} />,
    },
  ],
  fields: [
    { name: 'companyName', label: 'Tên nhà xe', required: true, span: 2, placeholder: 'Nhà xe Phương Trang' },
    { name: 'phone', label: 'Số điện thoại', required: true, placeholder: '028 3822 xxxx' },
    { name: 'email', label: 'Email', type: 'email', required: true, placeholder: 'lienhe@nhaxe.vn' },
    { name: 'isActive', label: 'Đang hoạt động', type: 'checkbox' },
  ],
};

export function BusCompanies() {
  return <CrudPage config={config} />;
}
