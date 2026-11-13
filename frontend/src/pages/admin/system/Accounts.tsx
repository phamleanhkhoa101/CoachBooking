import { useState } from 'react';
import { Badge, StatusBadge } from '../../../components/common/Badge';
import { Button } from '../../../components/common/Button';
import { FormField } from '../../../components/common/FormField';
import { Modal } from '../../../components/common/Modal';
import { CrudPage, refName, type CrudConfig } from '../../../components/table/CrudPage';
import { api, getErrorMessage } from '../../../api/client';
import type { Row } from '../../../api/types';
import { useToast } from '../../../components/common/Toast';

/** Chuyển tên quyền hệ thống thành nhãn tiếng Việt ngắn gọn. */
function roleLabel(roleName: string): string {
  const map: Record<string, string> = {
    Admin: 'Admin',
    NhanVien: 'Nhân viên',
    KhachHang: 'Khách hàng',
  };
  return map[roleName] ?? roleName;
}

/** Modal đổi mật khẩu riêng biệt. */
function ChangePasswordModal({
  account,
  onClose,
}: {
  account: Row | null;
  onClose: () => void;
}) {
  const { showToast } = useToast();
  const [newPwd, setNewPwd] = useState('');
  const [confirmPwd, setConfirmPwd] = useState('');
  const [error, setError] = useState('');
  const [saving, setSaving] = useState(false);

  const handleClose = () => {
    setNewPwd('');
    setConfirmPwd('');
    setError('');
    onClose();
  };

  const handleSubmit = async () => {
    if (!account) return;
    if (newPwd.length < 6) {
      setError('Mật khẩu phải có ít nhất 6 ký tự.');
      return;
    }
    if (newPwd !== confirmPwd) {
      setError('Xác nhận mật khẩu không khớp.');
      return;
    }
    setSaving(true);
    setError('');
    try {
      // UpdateRequest yêu cầu email + roleId + isActive; password là optional
      await api.put(`/accounts/${account.accountId ?? account.maTk}`, {
        email: account.email,
        roleId: account.roleId ?? account.maQuyen,
        isActive: account.isActive ?? account.trangThai ?? true,
        password: newPwd,
      });
      showToast('Đã đổi mật khẩu thành công.');
      handleClose();
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setSaving(false);
    }
  };

  return (
    <Modal
      open={Boolean(account)}
      onClose={handleClose}
      title="Đổi mật khẩu"
      subtitle={
        account
          ? `Tài khoản: ${String(account.username ?? account.tenDangNhap ?? account.accountId)}`
          : undefined
      }
      footer={
        <>
          <Button variant="secondary" disabled={saving} onClick={handleClose}>
            Hủy
          </Button>
          <Button
            variant="primary"
            loading={saving}
            disabled={saving}
            onClick={() => void handleSubmit()}
          >
            Lưu mật khẩu
          </Button>
        </>
      }
    >
      {error ? <div className="alert alert-error">{error}</div> : null}
      <div className="form-grid">
        <FormField
          label="Mật khẩu mới"
          type="password"
          value={newPwd}
          onChange={setNewPwd}
          placeholder="Ít nhất 6 ký tự"
          required
        />
        <FormField
          label="Xác nhận mật khẩu"
          type="password"
          value={confirmPwd}
          onChange={setConfirmPwd}
          placeholder="Nhập lại mật khẩu mới"
          required
        />
      </div>
    </Modal>
  );
}

export function Accounts() {
  const [changePwdAccount, setChangePwdAccount] = useState<Row | null>(null);

  const config: CrudConfig = {
    title: 'Tài khoản',
    subtitle: 'Tài khoản đăng nhập hệ thống và quyền tương ứng.',
    endpoint: 'accounts',
    idKey: 'accountId',
    searchKeys: ['username', 'email', 'tenDangNhap'],
    createLabel: 'Thêm tài khoản',
    searchPlaceholder: 'Tìm theo tên đăng nhập, email…',
    refs: [{ key: 'roles', endpoint: 'roles' }],
    transform: (payload, isEdit) => {
      if (isEdit) {
        delete payload.username;
        delete payload.password; // password được đổi qua modal riêng
      }
      return payload;
    },
    extraActions: [
      {
        label: '🔑 Đổi mật khẩu',
        variant: 'ghost',
        modes: ['view', 'edit'],
        onClick: (row) => setChangePwdAccount(row),
      },
    ],
    columns: [
      {
        key: 'accountId',
        label: 'Mã',
        width: '70px',
        render: (row) => row.accountId ?? row.maTk,
      },
      {
        key: 'username',
        label: 'Tên đăng nhập',
        render: (row) => row.username ?? row.tenDangNhap,
      },
      { key: 'email', label: 'Email' },
      {
        key: 'roleId',
        label: 'Quyền',
        width: '150px',
        render: (row, refs) => {
          const id = row.roleId ?? row.maQuyen;
          const rawName =
            refName(refs, 'roles', 'roleId', id, 'roleName') !== '—'
              ? refName(refs, 'roles', 'roleId', id, 'roleName')
              : refName(refs, 'roles', 'maQuyen', id, 'tenQuyen');
          return (
            <Badge tone={id === 1 ? 'info' : id === 2 ? 'success' : 'neutral'}>
              {roleLabel(rawName)}
            </Badge>
          );
        },
      },
      {
        key: 'isActive',
        label: 'Trạng thái',
        width: '130px',
        render: (row) => (
          <StatusBadge
            active={Boolean(row.isActive ?? row.trangThai)}
            trueLabel="Đang mở"
            falseLabel="Đã khóa"
          />
        ),
      },
    ],
    fields: [
      {
        name: 'username',
        label: 'Tên đăng nhập',
        required: true,
        placeholder: 'Ít nhất 6 ký tự',
        hideOnEdit: true,
      },
      {
        // Chỉ hiển thị khi tạo mới; khi sửa dùng modal Đổi mật khẩu riêng
        name: 'password',
        label: 'Mật khẩu',
        type: 'password',
        requiredOnCreate: true,
        hideOnEdit: true,
        placeholder: 'Ít nhất 6 ký tự',
      },
      {
        name: 'email',
        label: 'Email',
        type: 'email',
        required: true,
        placeholder: 'nguoidung@coachbooking.com',
      },
      {
        name: 'roleId',
        label: 'Quyền',
        type: 'select',
        required: true,
        asNumber: true,
        options: (refs) =>
          (refs['roles'] ?? []).map((item) => ({
            value: String(item.roleId ?? item.maQuyen),
            label: roleLabel(item.roleName ?? item.tenQuyen ?? ''),
          })),
      },
      { name: 'isActive', label: 'Đang hoạt động', type: 'checkbox' },
    ],
  };

  return (
    <>
      <CrudPage config={config} />
      <ChangePasswordModal
        account={changePwdAccount}
        onClose={() => setChangePwdAccount(null)}
      />
    </>
  );
}
