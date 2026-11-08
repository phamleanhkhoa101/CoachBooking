import { StatusBadge } from '../../../components/common/Badge';
import { CrudPage, type CrudConfig } from '../../../components/table/CrudPage';

const config: CrudConfig = {
  title: 'Loại xe',
  subtitle: 'Quy định số chỗ và kiểu bố trí ghế của từng loại xe.',
  endpoint: 'vehicle-types',
  idKey: 'typeId',
  searchKeys: ['typeName', 'tenLx'],
  createLabel: 'Thêm loại xe',
  searchPlaceholder: 'Tìm theo tên loại xe…',
  columns: [
    {
      key: 'typeId',
      label: 'Mã',
      width: '70px',
      render: (row) => row.typeId ?? row.maLx,
    },
    {
      key: 'typeName',
      label: 'Tên loại xe',
      render: (row) => row.typeName ?? row.tenLx,
    },
    {
      key: 'seatCapacity',
      label: 'Số chỗ',
      align: 'right',
      width: '90px',
      render: (row) => row.seatCapacity ?? row.soCho,
    },
    {
      key: 'isActive',
      label: 'Trạng thái',
      width: '130px',
      render: (row) => <StatusBadge active={Boolean(row.isActive ?? row.trangThai)} />,
    },
  ],
  fields: [
    { name: 'typeName', label: 'Tên loại xe', required: true, placeholder: 'Giường nằm / Ghế ngồi / Limousine' },
    { name: 'seatCapacity', label: 'Số chỗ', type: 'number', required: true, min: 1, max: 100 },
    { name: 'isActive', label: 'Đang sử dụng', type: 'checkbox' },
  ],
};

export function VehicleTypes() {
  return <CrudPage config={config} />;
}
