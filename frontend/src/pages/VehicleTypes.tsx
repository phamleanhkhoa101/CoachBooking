import { StatusBadge } from '../components/Badge';
import { CrudPage, type CrudConfig } from '../components/CrudPage';

const config: CrudConfig = {
  title: 'Loại xe',
  subtitle: 'Quy định số chỗ và kiểu bố trí ghế của từng loại xe.',
  endpoint: 'loai-xe',
  idKey: 'maLx',
  searchKeys: ['tenLx'],
  createLabel: 'Thêm loại xe',
  searchPlaceholder: 'Tìm theo tên loại xe…',
  columns: [
    { key: 'maLx', label: 'Mã', width: '70px' },
    { key: 'tenLx', label: 'Tên loại xe' },
    { key: 'soCho', label: 'Số chỗ', align: 'right', width: '90px' },
    {
      key: 'trangThai',
      label: 'Trạng thái',
      width: '130px',
      render: (row) => <StatusBadge active={Boolean(row.trangThai)} />,
    },
  ],
  fields: [
    { name: 'tenLx', label: 'Tên loại xe', required: true, placeholder: 'Giường nằm / Ghế ngồi / Limousine' },
    { name: 'soCho', label: 'Số chỗ', type: 'number', required: true, min: 1, max: 100 },
    { name: 'trangThai', label: 'Đang sử dụng', type: 'checkbox' },
  ],
};

export function VehicleTypes() {
  return <CrudPage config={config} />;
}
