import { StatusBadge } from '../../../components/common/Badge';
import { CrudPage, type CrudConfig } from '../../../components/table/CrudPage';
import { formatHour } from '../../../utils/format';

const config: CrudConfig = {
  title: 'Tuyến đường',
  subtitle: 'Điểm đi, điểm đến, quãng đường và thời gian dự kiến của mỗi tuyến.',
  endpoint: 'routes',
  idKey: 'routeId',
  searchKeys: ['departure', 'destination', 'diemDi', 'diemDen'],
  createLabel: 'Thêm tuyến',
  searchPlaceholder: 'Tìm theo điểm đi, điểm đến…',
  toForm: (row) => ({
    departure: String(row.departure ?? row.diemDi ?? ''),
    destination: String(row.destination ?? row.diemDen ?? ''),
    distance: row.distance != null ? String(row.distance) : row.quangDuong != null ? String(row.quangDuong) : '',
    estimatedDuration: String(row.estimatedDuration ?? row.thoiGianDuKien ?? '').slice(0, 5),
    isActive: (row.isActive ?? row.trangThai) ? 'true' : 'false',
  }),
  transform: (payload) => {
    const duration = payload.estimatedDuration;
    if (typeof duration === 'string' && duration.length === 5) {
      payload.estimatedDuration = `${duration}:00`;
    }
    return payload;
  },
  columns: [
    {
      key: 'routeId',
      label: 'Mã',
      width: '70px',
      render: (row) => row.routeId ?? row.maTuyen,
    },
    {
      key: 'route',
      label: 'Tuyến',
      render: (row) => (
        <strong>
          {row.departure ?? row.diemDi} → {row.destination ?? row.diemDen}
        </strong>
      ),
    },
    {
      key: 'distance',
      label: 'Quãng đường',
      align: 'right',
      width: '130px',
      render: (row) => {
        const dist = row.distance ?? row.quangDuong;
        return dist ? `${dist} km` : '—';
      },
    },
    {
      key: 'estimatedDuration',
      label: 'Thời gian dự kiến',
      align: 'center',
      width: '150px',
      render: (row) => formatHour(row.estimatedDuration ?? row.thoiGianDuKien),
    },
    {
      key: 'isActive',
      label: 'Trạng thái',
      width: '130px',
      render: (row) => <StatusBadge active={Boolean(row.isActive ?? row.trangThai)} />,
    },
  ],
  fields: [
    { name: 'departure', label: 'Điểm đi', required: true, placeholder: 'TP HCM' },
    { name: 'destination', label: 'Điểm đến', required: true, placeholder: 'Vũng Tàu' },
    { name: 'distance', label: 'Quãng đường (km)', type: 'number', min: 0 },
    { name: 'estimatedDuration', label: 'Thời gian dự kiến', type: 'time', help: 'Ví dụ 02:30 cho hành trình 2 giờ 30 phút.' },
    { name: 'isActive', label: 'Đang khai thác', type: 'checkbox' },
  ],
};

export function RoutesPage() {
  return <CrudPage config={config} />;
}
