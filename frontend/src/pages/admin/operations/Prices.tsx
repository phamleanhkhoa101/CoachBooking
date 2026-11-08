import { CrudPage, refName, type CrudConfig } from '../../../components/table/CrudPage';
import { formatVnd } from '../../../utils/format';

const config: CrudConfig = {
  title: 'Giá vé',
  subtitle: 'Bảng giá theo cặp (nhà xe, loại xe, tuyến đường).',
  endpoint: 'ticket-prices',
  idKey: 'priceId',
  searchKeys: ['priceId', 'price', 'maGv', 'giaVe'],
  createLabel: 'Thêm giá vé',
  searchPlaceholder: 'Tìm theo mã giá vé…',
  refs: [
    { key: 'bus-companies', endpoint: 'bus-companies' },
    { key: 'vehicle-types', endpoint: 'vehicle-types' },
    { key: 'routes', endpoint: 'routes' },
  ],
  columns: [
    {
      key: 'priceId',
      label: 'Mã',
      width: '70px',
      render: (row) => row.priceId ?? row.maGv,
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
      key: 'routeId',
      label: 'Tuyến',
      render: (row, refs) => {
        const id = row.routeId ?? row.maTuyen;
        const routeList = refs['routes'] ?? [];
        const route = routeList.find((item) => (item.routeId ?? item.maTuyen) === id);
        if (!route) return '—';
        const from = route.departure ?? route.diemDi;
        const to = route.destination ?? route.diemDen;
        return `${from} → ${to}`;
      },
    },
    {
      key: 'price',
      label: 'Giá vé',
      align: 'right',
      render: (row) => <strong className="text-primary">{formatVnd(row.price ?? row.giaVe)}</strong>,
    },
  ],
  fields: [
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
          label: String(item.typeName ?? item.tenLx),
        })),
    },
    {
      name: 'routeId',
      label: 'Tuyến đường',
      type: 'select',
      required: true,
      asNumber: true,
      options: (refs) =>
        (refs['routes'] ?? []).map((item) => ({
          value: String(item.routeId ?? item.maTuyen),
          label: `${item.departure ?? item.diemDi} → ${item.destination ?? item.diemDen}`,
        })),
    },
    { name: 'price', label: 'Giá vé (VNĐ)', type: 'number', required: true, min: 0 },
  ],
};

export function Prices() {
  return <CrudPage config={config} />;
}
