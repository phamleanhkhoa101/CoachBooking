import { StatusBadge } from '../../../components/common/Badge';
import { CrudPage, type CrudConfig } from '../../../components/table/CrudPage';
import { formatDate, gioiTinhLabel } from '../../../utils/format';

const config: CrudConfig = {
  title: 'Tài xế',
  subtitle: 'Hồ sơ tài xế và thông tin bằng lái.',
  endpoint: 'drivers',
  idKey: 'driverId',
  searchKeys: ['fullName', 'phone', 'citizenId', 'licenseNumber', 'tenTx', 'sdt', 'cccd', 'bangLai'],
  createLabel: 'Thêm tài xế',
  searchPlaceholder: 'Tìm theo tên, số điện thoại, CCCD, bằng lái…',
  columns: [
    {
      key: 'driverId',
      label: 'Mã',
      width: '70px',
      render: (row) => row.driverId ?? row.maTx,
    },
    {
      key: 'fullName',
      label: 'Họ tên',
      render: (row) => row.fullName ?? row.tenTx,
    },
    {
      key: 'dateOfBirth',
      label: 'Ngày sinh',
      width: '120px',
      render: (row) => formatDate(row.dateOfBirth ?? row.namSinh),
    },
    {
      key: 'gender',
      label: 'Giới tính',
      width: '100px',
      render: (row) => gioiTinhLabel(row.gender ?? row.gioiTinh),
    },
    {
      key: 'phone',
      label: 'Số điện thoại',
      width: '140px',
      render: (row) => row.phone ?? row.sdt ?? '—',
    },
    {
      key: 'licenseNumber',
      label: 'Bằng lái',
      width: '110px',
      render: (row) => row.licenseNumber ?? row.bangLai ?? '—',
    },
    {
      key: 'isActive',
      label: 'Trạng thái',
      width: '130px',
      render: (row) => <StatusBadge active={Boolean(row.isActive ?? row.trangThai)} />,
    },
  ],
  fields: [
    { name: 'fullName', label: 'Họ tên tài xế', required: true, span: 2 },
    { name: 'dateOfBirth', label: 'Ngày sinh', type: 'date', required: true },
    {
      name: 'gender',
      label: 'Giới tính',
      type: 'select',
      asBoolean: true,
      options: [
        { value: 'true', label: 'Nam' },
        { value: 'false', label: 'Nữ' },
      ],
    },
    { name: 'citizenId', label: 'Số CCCD', required: true },
    { name: 'phone', label: 'Số điện thoại', required: true },
    { name: 'licenseNumber', label: 'Hạng bằng lái', required: true, placeholder: 'B2 / C / D' },
    { name: 'issueDate', label: 'Ngày cấp bằng', type: 'date', required: true },
    { name: 'address', label: 'Địa chỉ', required: true, span: 2 },
    { name: 'isActive', label: 'Đang làm việc', type: 'checkbox' },
  ],
};

export function Drivers() {
  return <CrudPage config={config} />;
}
