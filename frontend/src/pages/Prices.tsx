import { CrudPage, refName, type CrudConfig } from '../components/CrudPage';
import { formatVnd } from '../utils/format';

const config: CrudConfig = {
  title: 'Giá vé',
  subtitle: 'Bảng giá theo cặp (nhà xe, loại xe, tuyến đường).',
  endpoint: 'gia-ve',
  idKey: 'maGv',
  searchKeys: ['maGv', 'giaVe'],
  createLabel: 'Thêm giá vé',
  searchPlaceholder: 'Tìm theo mã giá vé…',
  refs: [
    { key: 'nha-xe', endpoint: 'nha-xe' },
    { key: 'loai-xe', endpoint: 'loai-xe' },
    { key: 'tuyen-duong', endpoint: 'tuyen-duong' },
  ],
  columns: [
    { key: 'maGv', label: 'Mã', width: '70px' },
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
      key: 'maTuyen',
      label: 'Tuyến',
      render: (row, refs) => {
        const tuyen = (refs['tuyen-duong'] ?? []).find((item) => item.maTuyen === row.maTuyen);
        return tuyen ? `${tuyen.diemDi} → ${tuyen.diemDen}` : '—';
      },
    },
    {
      key: 'giaVe',
      label: 'Giá vé',
      align: 'right',
      render: (row) => <strong className="text-primary">{formatVnd(row.giaVe)}</strong>,
    },
  ],
  fields: [
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
      options: (refs) => (refs['loai-xe'] ?? []).map((item) => ({ value: String(item.maLx), label: String(item.tenLx) })),
    },
    {
      name: 'maTuyen',
      label: 'Tuyến đường',
      type: 'select',
      required: true,
      asNumber: true,
      options: (refs) =>
        (refs['tuyen-duong'] ?? []).map((item) => ({
          value: String(item.maTuyen),
          label: `${item.diemDi} → ${item.diemDen}`,
        })),
    },
    { name: 'giaVe', label: 'Giá vé (VNĐ)', type: 'number', required: true, min: 0 },
  ],
};

export function Prices() {
  return <CrudPage config={config} />;
}
