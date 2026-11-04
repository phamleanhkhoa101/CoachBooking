import { StatusBadge } from '../components/Badge';
import { CrudPage, type CrudConfig } from '../components/CrudPage';

const config: CrudConfig = {
  title: 'Nhà xe',
  subtitle: 'Danh sách các nhà xe đang hợp tác khai thác tuyến.',
  endpoint: 'nha-xe',
  idKey: 'maNx',
  searchKeys: ['tenNx', 'sdt', 'email'],
  createLabel: 'Thêm nhà xe',
  searchPlaceholder: 'Tìm theo tên nhà xe, số điện thoại, email…',
  columns: [
    { key: 'maNx', label: 'Mã', width: '70px' },
    { key: 'tenNx', label: 'Tên nhà xe' },
    { key: 'sdt', label: 'Số điện thoại', width: '150px' },
    { key: 'email', label: 'Email' },
    {
      key: 'trangThai',
      label: 'Trạng thái',
      width: '130px',
      render: (row) => <StatusBadge active={Boolean(row.trangThai)} />,
    },
  ],
  fields: [
    { name: 'tenNx', label: 'Tên nhà xe', required: true, span: 2, placeholder: 'Nhà xe Phương Trang' },
    { name: 'sdt', label: 'Số điện thoại', placeholder: '028 3822 xxxx' },
    { name: 'email', label: 'Email', type: 'email', placeholder: 'lienhe@nhaxe.vn' },
    { name: 'trangThai', label: 'Đang hoạt động', type: 'checkbox' },
  ],
};

export function BusCompanies() {
  return <CrudPage config={config} />;
}
