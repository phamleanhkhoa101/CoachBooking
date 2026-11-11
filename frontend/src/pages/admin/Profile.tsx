import { useCallback, useEffect, useState } from 'react';
import { api, getErrorMessage } from '../../api/client';
import type { Profile as ProfileType } from '../../api/types';
import { Badge } from '../../components/common/Badge';
import { FormField } from '../../components/common/FormField';
import { useToast } from '../../components/common/Toast';
import { formatDate, toDateInput } from '../../utils/format';

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

  // Chế độ chỉnh sửa thông tin cá nhân
  const [isEditing, setIsEditing] = useState(false);

  // Chế độ đổi mật khẩu
  const [isChangingPassword, setIsChangingPassword] = useState(false);

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

  const handleCancelEdit = () => {
    if (profile) {
      applyData(profile);
    }
    setIsEditing(false);
  };

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
      setIsEditing(false);
      showToast('Đã cập nhật hồ sơ thành công.');
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setSaving(false);
    }
  };

  const handleCancelPassword = () => {
    setOldPassword('');
    setNewPassword('');
    setConfirmPassword('');
    setPwError('');
    setIsChangingPassword(false);
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
      setPwError('');
      setIsChangingPassword(false);
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
            <div>
              <h2>{isEditing ? 'Chỉnh sửa thông tin cá nhân' : 'Thông tin cá nhân'}</h2>
              <span className="text-muted">
                {isEmployee ? 'Nhân viên bến xe' : isCustomer ? 'Khách hàng' : 'Chưa gắn hồ sơ'}
              </span>
            </div>

            <div className="profile-card-header-actions">
              {!isEditing ? (
                <button
                  type="button"
                  className="btn btn-primary btn-sm"
                  onClick={() => setIsEditing(true)}
                  disabled={isUnlinked}
                >
                  <svg
                    width="15"
                    height="15"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    strokeWidth="2.2"
                  >
                    <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7" />
                    <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z" />
                  </svg>
                  <span>Thay đổi thông tin</span>
                </button>
              ) : (
                <button
                  type="button"
                  className="btn btn-ghost btn-sm"
                  onClick={handleCancelEdit}
                  disabled={saving}
                >
                  Hủy bỏ
                </button>
              )}
            </div>
          </div>

          {!isEditing ? (
            <div className="profile-view-box">
              <div className="profile-section-title">
                <svg
                  width="15"
                  height="15"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  strokeWidth="2"
                >
                  <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
                  <circle cx="12" cy="7" r="4" />
                </svg>
                <span>Thông tin cá nhân</span>
              </div>
              <div className="profile-info-grid">
                <div className="info-item">
                  <span className="info-label">Họ và tên</span>
                  <strong className="info-value text-strong">{fullName || 'Chưa cập nhật'}</strong>
                </div>
                <div className="info-item">
                  <span className="info-label">Giới tính</span>
                  <span className="info-value">{gender === 'true' ? 'Nam' : 'Nữ'}</span>
                </div>
                <div className="info-item">
                  <span className="info-label">Ngày sinh</span>
                  <span className="info-value">
                    {dateOfBirth ? formatDate(dateOfBirth) : 'Chưa cập nhật'}
                  </span>
                </div>
                <div className="info-item">
                  <span className="info-label">Số điện thoại</span>
                  <span className="info-value">{phone || 'Chưa cập nhật'}</span>
                </div>
                {isEmployee && (
                  <div className="info-item info-item-full">
                    <span className="info-label">Địa chỉ thường trú</span>
                    <span className="info-value">{address || 'Chưa cập nhật'}</span>
                  </div>
                )}
              </div>

              <div className="profile-divider" />

              <div className="profile-section-title">
                <svg
                  width="15"
                  height="15"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  strokeWidth="2"
                >
                  <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                  <path d="M7 11V7a5 5 0 0 1 10 0v4" />
                </svg>
                <span>Tài khoản đăng nhập</span>
              </div>
              <div className="profile-info-grid">
                <div className="info-item">
                  <span className="info-label">Tên đăng nhập</span>
                  <span className="info-value">
                    <span className="mono">@{profile?.username}</span>
                  </span>
                </div>
                <div className="info-item">
                  <span className="info-label">Địa chỉ Email</span>
                  <span className="info-value">{profile?.email || 'Chưa cập nhật'}</span>
                </div>
              </div>
            </div>
          ) : (
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
              <div className="form-actions form-full" style={{ display: 'flex', gap: '10px' }}>
                <button
                  type="button"
                  className="btn btn-secondary"
                  onClick={handleCancelEdit}
                  disabled={saving}
                >
                  Hủy bỏ
                </button>
                <button
                  type="submit"
                  className="btn btn-primary"
                  disabled={saving || isUnlinked}
                >
                  {saving ? 'Đang lưu…' : 'Lưu thay đổi'}
                </button>
              </div>
            </form>
          )}
        </section>

        <section className="card">
          <div className="card-header">
            <div>
              <h2>{isChangingPassword ? 'Cập nhật mật khẩu' : 'Đổi mật khẩu'}</h2>
              <span className="text-muted">Bảo mật tài khoản</span>
            </div>

            <div className="profile-card-header-actions">
              {!isChangingPassword ? (
                <button
                  type="button"
                  className="btn btn-primary btn-sm"
                  onClick={() => {
                    setPwError('');
                    setIsChangingPassword(true);
                  }}
                >
                  <svg
                    width="15"
                    height="15"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    strokeWidth="2.2"
                  >
                    <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                    <path d="M7 11V7a5 5 0 0 1 10 0v4" />
                  </svg>
                  <span>Thay đổi mật khẩu</span>
                </button>
              ) : (
                <button
                  type="button"
                  className="btn btn-ghost btn-sm"
                  onClick={handleCancelPassword}
                  disabled={pwBusy}
                >
                  Hủy bỏ
                </button>
              )}
            </div>
          </div>

          {!isChangingPassword ? (
            <div className="profile-view-box">
              <div className="profile-info-grid">
                <div className="info-item">
                  <span className="info-label">Mật khẩu tài khoản</span>
                  <span
                    className="info-value"
                    style={{ letterSpacing: '3px', fontSize: '1.15rem' }}
                  >
                    ••••••••••••
                  </span>
                </div>
                <div className="info-item">
                  <span className="info-label">Tình trạng bảo mật</span>
                  <div
                    className="info-value"
                    style={{ display: 'flex', alignItems: 'center', marginTop: '2px' }}
                  >
                    <Badge tone="success">
                      <svg
                        width="12"
                        height="12"
                        viewBox="0 0 24 24"
                        fill="none"
                        stroke="currentColor"
                        strokeWidth="3"
                      >
                        <polyline points="20 6 9 17 4 12" />
                      </svg>
                      Đã thiết lập & bảo vệ
                    </Badge>
                  </div>
                </div>
              </div>
            </div>
          ) : (
            <>
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
                <div className="form-actions" style={{ display: 'flex', gap: '10px' }}>
                  <button
                    type="button"
                    className="btn btn-secondary"
                    onClick={handleCancelPassword}
                    disabled={pwBusy}
                  >
                    Hủy bỏ
                  </button>
                  <button type="submit" className="btn btn-primary" disabled={pwBusy}>
                    {pwBusy ? 'Đang cập nhật…' : 'Cập nhật mật khẩu'}
                  </button>
                </div>
              </form>
            </>
          )}
        </section>
      </div>
    </div>
  );
}
