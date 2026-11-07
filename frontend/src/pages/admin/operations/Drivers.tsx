import { StatusBadge } from '../../../components/common/Badge';
import { CrudPage, type CrudConfig } from '../../../components/table/CrudPage';
import { formatDate, gioiTinhLabel } from '../../../utils/format';

const config: CrudConfig = {
  title: 'Tài xế',
  subtitle: 'Hồ sơ tài xế và thông tin bằng lái.',
  endpoint: 'tai-xe',
  idKey: 'maTx',
  searchKeys: ['tenTx', 'sdt', 'cccd', 'bangLai'],
  createLabel: 'Thêm tài xế',
  searchPlaceholder: 'Tìm theo tên, số điện thoại, CCCD, bằng lái…',
  columns: [
    { key: 'maTx', label: 'Mã', width: '70px' },
    { key: 'tenTx', label: 'Họ tên' },
    {
      key: 'namSinh',
      label: 'Ngày sinh',
      width: '120px',
      render: (row) => formatDate(row.namSinh),
    },
    {
      key: 'gioiTinh',
      label: 'Giới tính',
      width: '100px',
      render: (row) => gioiTinhLabel(row.gioiTinh),
    },
    { key: 'sdt', label: 'Số điện thoại', width: '140px' },
    { key: 'bangLai', label: 'Bằng lái', width: '110px' },
    {
      key: 'trangThai',
      label: 'Trạng thái',
      width: '130px',
      render: (row) => <StatusBadge active={Boolean(row.trangThai)} />,
    },
  ],
  fields: [
    { name: 'tenTx', label: 'Họ tên tài xế', required: true, span: 2 },
    { name: 'namSinh', label: 'Ngày sinh', type: 'date' },
    {
      name: 'gioiTinh',
      label: 'Giới tính',
      type: 'select',
      asBoolean: true,
      options: [
        { value: 'true', label: 'Nam' },
        { value: 'false', label: 'Nữ' },
      ],
    },
    { name: 'cccd', label: 'Số CCCD' },
    { name: 'sdt', label: 'Số điện thoại' },
    { name: 'bangLai', label: 'Hạng bằng lái', placeholder: 'B2 / C / D' },
    { name: 'ngayCap', label: 'Ngày cấp bằng', type: 'date' },
    { name: 'diaChi', label: 'Địa chỉ', span: 2 },
    { name: 'trangThai', label: 'Đang làm việc', type: 'checkbox' },
  ],
};

export function Drivers() {
  return <CrudPage config={config} />;
}
