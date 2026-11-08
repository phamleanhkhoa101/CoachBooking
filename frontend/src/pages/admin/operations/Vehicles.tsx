import { StatusBadge } from '../../../components/common/Badge';
import { CrudPage, refName, type CrudConfig } from '../../../components/table/CrudPage';

const config: CrudConfig = {
  title: 'Xe',
  subtitle: 'Danh sách xe của các nhà xe, gắn với loại xe để xác định sơ đồ ghế.',
  endpoint: 'vehicles',
  idKey: 'vehicleId',
  searchKeys: ['licensePlate', 'bienSoXe'],
  createLabel: 'Thêm xe',
  searchPlaceholder: 'Tìm theo biển số xe…',
  refs: [
    { key: 'bus-companies', endpoint: 'bus-companies' },
    { key: 'vehicle-types', endpoint: 'vehicle-types' },
  ],
  columns: [
    {
      key: 'vehicleId',
      label: 'Mã',
      width: '70px',
      render: (row) => row.vehicleId ?? row.maXe,
    },
    {
      key: 'licensePlate',
      label: 'Biển số xe',
      width: '150px',
      render: (row) => row.licensePlate ?? row.bienSoXe,
    },
    {
      key: 'companyId',
      label: 'Nhà xe',
      render: (row, refs) => {
        const id = row.companyId ?? row.maNx;
        const name = refName(refs, 'bus-companies', 'companyId', id, 'companyName');
        return name !== '—' ? name : refName(refs, 'bus-companies', 'maNx', id, 'tenNx');
      },
    },
    {
      key: 'typeId',
      label: 'Loại xe',
      render: (row, refs) => {
        const id = row.typeId ?? row.maLx;
        const name = refName(refs, 'vehicle-types', 'typeId', id, 'typeName');
        return name !== '—' ? name : refName(refs, 'vehicle-types', 'maLx', id, 'tenLx');
      },
    },
    {
      key: 'isActive',
      label: 'Trạng thái',
      width: '130px',
      render: (row) => <StatusBadge active={Boolean(row.isActive ?? row.trangThai)} />,
    },
  ],
  fields: [
    { name: 'licensePlate', label: 'Biển số xe', required: true, placeholder: '29A-12345', span: 2 },
    {
      name: 'companyId',
      label: 'Nhà xe',
      type: 'select',
      required: true,
      asNumber: true,
      options: (refs) =>
        (refs['bus-companies'] ?? []).map((item) => ({
          value: String(item.companyId ?? item.maNx),
          label: String(item.companyName ?? item.tenNx),
        })),
    },
    {
      name: 'typeId',
      label: 'Loại xe',
      type: 'select',
      required: true,
      asNumber: true,
      options: (refs) =>
        (refs['vehicle-types'] ?? []).map((item) => ({
          value: String(item.typeId ?? item.maLx),
          label: `${item.typeName ?? item.tenLx} (${item.seatCapacity ?? item.soCho} chỗ)`,
        })),
    },
    { name: 'isActive', label: 'Đang hoạt động', type: 'checkbox' },
  ],
};

export function Vehicles() {
  return <CrudPage config={config} />;
}
