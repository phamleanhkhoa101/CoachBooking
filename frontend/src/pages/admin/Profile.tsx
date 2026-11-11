import { useCallback, useEffect, useState } from 'react';
import { api, getErrorMessage } from '../../api/client';
import type { Profile as ProfileType } from '../../api/types';
import { FormField } from '../../components/common/FormField';
import { useToast } from '../../components/common/Toast';
import { toDateInput } from '../../utils/format';

function isStrongPassword(value: string): boolean {
  return (
    value.length >= 8 &&
    /[A-Z]/.test(value) &&
    /[a-z]/.test(value) &&
    /[0-9]/.test(value) &&
    /[^A-Za-z0-9]/.test(value)
  );
}

export function Profile() {
  const { showToast } = useToast();
  const [profile, setProfile] = useState<ProfileType | null>(null);
  const [loading, setLoading] = useState(true);

  const [fullName, setFullName] = useState('');
  const [dateOfBirth, setDateOfBirth] = useState('');
  const [phone, setPhone] = useState('');
  const [gender, setGender] = useState('true');
  const [address, setAddress] = useState('');
  const [saving, setSaving] = useState(false);

  const [oldPassword, setOldPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [pwError, setPwError] = useState('');
  const [pwBusy, setPwBusy] = useState(false);

  const applyData = useCallback((data: ProfileType) => {
    setProfile(data);
    setFullName(data.fullName ?? data.hoTen ?? '');
    setDateOfBirth(toDateInput(data.dateOfBirth ?? data.namSinh));
    setPhone(data.phone ?? data.sdt ?? '');
    setGender((data.gender ?? data.gioiTinh) === false ? 'false' : 'true');
    setAddress(data.address ?? data.diaChi ?? '');
  }, []);

  useEffect(() => {
    void (async () => {
      try {
        applyData(await api.get<ProfileType>('/profile'));
      } catch (error) {
        showToast(getErrorMessage(error), 'error');
      } finally {
        setLoading(false);
      }
    })();
  }, [applyData, showToast]);

  const profileType = profile?.type ?? profile?.loai;
  const isEmployee = profileType === 'EMPLOYEE' || profileType === 'NHANVIEN';
  const isCustomer = profileType === 'CUSTOMER' || profileType === 'KHACHHANG';
  const isUnlinked = profileType === 'UNLINKED' || profileType === 'CHUA_GAN';

  const handleSave = async () => {
    if (isUnlinked) {
      showToast('Tài khoản chưa được gắn với hồ sơ nhân viên hoặc khách hàng.', 'error');
      return;
    }
    if (!fullName.trim()) {
      showToast('Họ tên không được để trống.', 'error');
      return;
    }
    if (!dateOfBirth) {
      showToast('Vui lòng nhập ngày sinh.', 'error');
      return;
    }
    if (!/^\d{10,11}$/.test(phone.trim())) {
      showToast('Số điện thoại phải gồm 10 đến 11 chữ số.', 'error');
      return;
    }
    setSaving(true);
    try {
      const body: Record<string, unknown> = {
        fullName: fullName.trim(),
        dateOfBirth,
        phone: phone.trim(),
        gender: gender === 'true',
      };
      if (isEmployee) {
        body.address = address.trim() || null;
      }
      const data = await api.put<ProfileType>('/profile', body);
      applyData(data);
      showToast('Đã cập nhật hồ sơ.');
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setSaving(false);
    }
  };

  const handleChangePassword = async () => {
    if (!oldPassword) {
      setPwError('Vui lòng nhập mật khẩu hiện tại.');
      return;
    }
    if (!isStrongPassword(newPassword)) {
      setPwError('Mật khẩu mới tối thiểu 8 ký tự, gồm chữ hoa, chữ thường, chữ số và ký tự đặc biệt.');
      return;
    }
    if (newPassword !== confirmPassword) {
      setPwError('Mật khẩu xác nhận không khớp.');
      return;
    }
    setPwBusy(true);
    setPwError('');
    try {
      await api.post('/auth/change-password', {
        oldPassword,
        newPassword,
      });
      showToast('Đã đổi mật khẩu thành công.');
      setOldPassword('');
      setNewPassword('');
      setConfirmPassword('');
    } catch (error) {
      setPwError(getErrorMessage(error));
    } finally {
      setPwBusy(false);
    }
  };

  if (loading) {
    return (
      <div className="page-loading">
        <span className="spinner" /> Đang tải hồ sơ…
      </div>
    );
  }

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>Hồ sơ cá nhân</h1>
          <p className="page-subtitle">Thông tin tài khoản đăng nhập và hồ sơ liên kết.</p>
        </div>
      </div>

      <div className="profile-layout">
        <section className="card">
          <div className="card-header">
            <h2>Thông tin cá nhân</h2>
            <span className="text-muted">
              {isEmployee ? 'Nhân viên bến xe' : isCustomer ? 'Khách hàng' : 'Chưa gắn hồ sơ'}
            </span>
          </div>

          <div className="read-grid">
            <div>
              <span className="text-muted">Tên đăng nhập</span>
              <strong>{profile?.username}</strong>
            </div>
            <div>
              <span className="text-muted">Email</span>
              <strong>{profile?.email}</strong>
            </div>
          </div>

          <form
            className="form-grid"
            onSubmit={(event) => {
              event.preventDefault();
              void handleSave();
            }}
          >
            <FormField
              label="Họ và tên"
              name="fullName"
              required
              value={fullName}
              onChange={setFullName}
            />
            <FormField
              label="Ngày sinh"
              name="dateOfBirth"
              type="date"
              required
              value={dateOfBirth}
              onChange={setDateOfBirth}
            />
            <FormField
              label="Số điện thoại"
              name="phone"
              required
              value={phone}
              onChange={setPhone}
              placeholder="09xxxxxxxx"
            />
            <FormField
              label="Giới tính"
              name="gender"
              type="select"
              value={gender}
              onChange={setGender}
              options={[
                { value: 'true', label: 'Nam' },
                { value: 'false', label: 'Nữ' },
              ]}
            />
            {isEmployee && (
              <div className="form-full">
                <FormField
                  label="Địa chỉ thường trú"
                  name="address"
                  required
                  value={address}
                  onChange={setAddress}
                />
              </div>
            )}
            <div className="form-actions form-full">
              <button
                type="submit"
                className="btn btn-primary"
                disabled={saving || isUnlinked}
              >
                {saving ? 'Đang lưu…' : 'Lưu thay đổi'}
              </button>
            </div>
          </form>
        </section>

        <section className="card">
          <div className="card-header">
            <h2>Đổi mật khẩu</h2>
            <span className="text-muted">Bảo mật tài khoản</span>
          </div>

          {pwError && <div className="form-error-banner">{pwError}</div>}

          <form
            className="form-vertical"
            onSubmit={(event) => {
              event.preventDefault();
              void handleChangePassword();
            }}
          >
            <FormField
              label="Mật khẩu hiện tại"
              name="oldPassword"
              type="password"
              required
              value={oldPassword}
              onChange={setOldPassword}
            />
            <FormField
              label="Mật khẩu mới"
              name="newPassword"
              type="password"
              required
              value={newPassword}
              onChange={setNewPassword}
              help="Tối thiểu 8 ký tự, gồm chữ hoa, chữ thường, chữ số và ký tự đặc biệt."
            />
            <FormField
              label="Xác nhận mật khẩu mới"
              name="confirmPassword"
              type="password"
              required
              value={confirmPassword}
              onChange={setConfirmPassword}
            />
            <div className="form-actions">
              <button type="submit" className="btn btn-secondary" disabled={pwBusy}>
                {pwBusy ? 'Đang cập nhật…' : 'Cập nhật mật khẩu'}
              </button>
            </div>
          </form>
        </section>
      </div>
    </div>
  );
}
