import { StatusBadge } from '../../../components/common/Badge';
import { CrudPage, type CrudConfig } from '../../../components/table/CrudPage';
import { formatHour } from '../../../utils/format';

const config: CrudConfig = {
  title: 'Tuyến đường',
  subtitle: 'Điểm đi, điểm đến, quãng đường và thời gian dự kiến của mỗi tuyến.',
  endpoint: 'tuyen-duong',
  idKey: 'maTuyen',
  searchKeys: ['diemDi', 'diemDen'],
  createLabel: 'Thêm tuyến',
  searchPlaceholder: 'Tìm theo điểm đi, điểm đến…',
  toForm: (row) => ({
    diemDi: row.diemDi ? String(row.diemDi) : '',
    diemDen: row.diemDen ? String(row.diemDen) : '',
    quangDuong: row.quangDuong === null || row.quangDuong === undefined ? '' : String(row.quangDuong),
    thoiGianDuKien: row.thoiGianDuKien ? String(row.thoiGianDuKien).slice(0, 5) : '',
    trangThai: row.trangThai ? 'true' : 'false',
  }),
  transform: (payload) => {
    const thoiGian = payload.thoiGianDuKien;
    if (typeof thoiGian === 'string' && thoiGian.length === 5) {
      payload.thoiGianDuKien = `${thoiGian}:00`;
    }
    return payload;
  },
  columns: [
    { key: 'maTuyen', label: 'Mã', width: '70px' },
    {
      key: 'tuyen',
      label: 'Tuyến',
      render: (row) => (
        <strong>
          {row.diemDi} → {row.diemDen}
        </strong>
      ),
    },
    {
      key: 'quangDuong',
      label: 'Quãng đường',
      align: 'right',
      width: '130px',
      render: (row) => (row.quangDuong ? `${row.quangDuong} km` : '—'),
    },
    {
      key: 'thoiGianDuKien',
      label: 'Thời gian dự kiến',
      align: 'center',
      width: '150px',
      render: (row) => formatHour(row.thoiGianDuKien),
    },
    {
      key: 'trangThai',
      label: 'Trạng thái',
      width: '130px',
      render: (row) => <StatusBadge active={Boolean(row.trangThai)} />,
    },
  ],
  fields: [
    { name: 'diemDi', label: 'Điểm đi', required: true, placeholder: 'TP HCM' },
    { name: 'diemDen', label: 'Điểm đến', required: true, placeholder: 'Vũng Tàu' },
    { name: 'quangDuong', label: 'Quãng đường (km)', type: 'number', min: 0 },
    { name: 'thoiGianDuKien', label: 'Thời gian dự kiến', type: 'time', help: 'Ví dụ 02:30 cho hành trình 2 giờ 30 phút.' },
    { name: 'trangThai', label: 'Đang khai thác', type: 'checkbox' },
  ],
};

export function RoutesPage() {
  return <CrudPage config={config} />;
}
