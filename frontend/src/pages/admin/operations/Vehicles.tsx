import { StatusBadge } from '../../../components/common/Badge';
import { CrudPage, refName, type CrudConfig } from '../../../components/table/CrudPage';

const config: CrudConfig = {
  title: 'Xe',
  subtitle: 'Danh sách xe của các nhà xe, gắn với loại xe để xác định sơ đồ ghế.',
  endpoint: 'xe',
  idKey: 'maXe',
  searchKeys: ['bienSoXe'],
  createLabel: 'Thêm xe',
  searchPlaceholder: 'Tìm theo biển số xe…',
  refs: [
    { key: 'nha-xe', endpoint: 'nha-xe' },
    { key: 'loai-xe', endpoint: 'loai-xe' },
  ],
  columns: [
    { key: 'maXe', label: 'Mã', width: '70px' },
    { key: 'bienSoXe', label: 'Biển số xe', width: '150px' },
    {
      key: 'maNx',
      label: 'Nhà xe',
      render: (row, refs) => refName(refs, 'nha-xe', 'maNx', row.maNx, 'tenNx'),
    },
    {
      key: 'maLx',
      label: 'Loại xe',
      render: (row, refs) => refName(refs, 'loai-xe', 'maLx', row.maLx, 'tenLx'),
    },
    {
      key: 'trangThai',
      label: 'Trạng thái',
      width: '130px',
      render: (row) => <StatusBadge active={Boolean(row.trangThai)} />,
    },
  ],
  fields: [
    { name: 'bienSoXe', label: 'Biển số xe', required: true, placeholder: '29A-12345', span: 2 },
    {
      name: 'maNx',
      label: 'Nhà xe',
      type: 'select',
      required: true,
      asNumber: true,
      options: (refs) => (refs['nha-xe'] ?? []).map((item) => ({ value: String(item.maNx), label: String(item.tenNx) })),
    },
    {
      name: 'maLx',
      label: 'Loại xe',
      type: 'select',
      required: true,
      asNumber: true,
      options: (refs) => (refs['loai-xe'] ?? []).map((item) => ({ value: String(item.maLx), label: `${item.tenLx} (${item.soCho} chỗ)` })),
    },
    { name: 'trangThai', label: 'Đang hoạt động', type: 'checkbox' },
  ],
};

export function Vehicles() {
  return <CrudPage config={config} />;
}
