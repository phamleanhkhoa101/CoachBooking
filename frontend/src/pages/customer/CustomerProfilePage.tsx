import { useCallback, useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { api, getErrorMessage } from '../../api/client';
import type { Profile as ProfileType } from '../../api/types';
import { useAuth } from '../../auth/AuthContext';
import { useToast } from '../../components/common/Toast';
import { formatDate, toDateInput } from '../../utils/format';
import './CustomerProfilePage.css';

function isStrongPassword(value: string): boolean {
  return (
    value.length >= 8 &&
    /[A-Z]/.test(value) &&
    /[a-z]/.test(value) &&
    /[0-9]/.test(value) &&
    /[^A-Za-z0-9]/.test(value)
  );
}

export function CustomerProfilePage() {
  const { user } = useAuth();
  const { showToast } = useToast();

  const [profile, setProfile] = useState<ProfileType | null>(null);
  const [loading, setLoading] = useState(true);

  // Profile Edit mode toggle
  const [isEditing, setIsEditing] = useState(false);

  // Password Modal toggle
  const [showPasswordModal, setShowPasswordModal] = useState(false);

  // Form profile fields
  const [fullName, setFullName] = useState('');
  const [dateOfBirth, setDateOfBirth] = useState('');
  const [phone, setPhone] = useState('');
  const [gender, setGender] = useState('true');
  const [saving, setSaving] = useState(false);

  // Change password fields (inside modal)
  const [oldPassword, setOldPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [pwError, setPwError] = useState('');
  const [pwBusy, setPwBusy] = useState(false);

  const applyData = useCallback((data: ProfileType) => {
    setProfile(data);
    setFullName(data.fullName ?? '');
    setDateOfBirth(toDateInput(data.dateOfBirth));
    setPhone(data.phone ?? '');
    setGender(data.gender === false ? 'false' : 'true');
  }, []);

  const fetchProfile = useCallback(async () => {
    setLoading(true);
    try {
      const data = await api.get<ProfileType>('/profile');
      applyData(data);
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setLoading(false);
    }
  }, [applyData, showToast]);

  useEffect(() => {
    void fetchProfile();
  }, [fetchProfile]);

  // Handle ESC key to close modal
  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape' && showPasswordModal && !pwBusy) {
        handleClosePasswordModal();
      }
    };
    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [showPasswordModal, pwBusy]);

  const handleCancelEdit = () => {
    if (profile) {
      applyData(profile);
    }
    setIsEditing(false);
  };

  const handleOpenPasswordModal = () => {
    setOldPassword('');
    setNewPassword('');
    setConfirmPassword('');
    setPwError('');
    setShowPasswordModal(true);
  };

  const handleClosePasswordModal = () => {
    if (pwBusy) return;
    setOldPassword('');
    setNewPassword('');
    setConfirmPassword('');
    setPwError('');
    setShowPasswordModal(false);
  };

  const handleSaveProfile = async (e: React.FormEvent) => {
    e.preventDefault();
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
      const body = {
        fullName: fullName.trim(),
        dateOfBirth,
        phone: phone.trim(),
        gender: gender === 'true',
      };
      const data = await api.put<ProfileType>('/profile', body);
      applyData(data);
      setIsEditing(false);
      showToast('Đã cập nhật thông tin cá nhân thành công.');
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setSaving(false);
    }
  };

  const handleChangePassword = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!oldPassword) {
      setPwError('Vui lòng nhập mật khẩu hiện tại.');
      return;
    }
    if (!isStrongPassword(newPassword)) {
      setPwError(
        'Mật khẩu mới tối thiểu 8 ký tự, gồm chữ hoa, chữ thường, chữ số và ký tự đặc biệt.',
      );
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
      handleClosePasswordModal();
    } catch (error) {
      setPwError(getErrorMessage(error));
    } finally {
      setPwBusy(false);
    }
  };

  const displayName = profile?.fullName ?? user?.fullName ?? user?.username ?? 'Khách hàng';
  const displayEmail = profile?.email ?? user?.email ?? 'Chưa cập nhật email';
  const displayUsername = profile?.username ?? user?.username ?? '';

  if (loading) {
    return (
      <div className="customer-profile-page">
        <div className="profile-loading-state">
          <span className="spinner" /> Đang tải thông tin hồ sơ…
        </div>
      </div>
    );
  }

  return (
    <div className="customer-profile-page">
      {/* 1. HERO HEADER */}
      <section className="profile-page-header">
        <div className="profile-header-container">
          <div className="profile-hero">
            <div className="profile-avatar-large">
              {displayName.charAt(0).toUpperCase()}
            </div>
            <div className="profile-hero-info">
              <h1>{displayName}</h1>
              <div className="profile-meta-row">
                <span className="profile-role-badge">
                  <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3">
                    <polyline points="20 6 9 17 4 12" />
                  </svg>
                  Khách hàng thân thiết
                </span>
                <span className="profile-meta-item">
                  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
                    <circle cx="12" cy="7" r="4" />
                  </svg>
                  @{displayUsername}
                </span>
                <span className="profile-meta-item">
                  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z" />
                    <polyline points="22,6 12,13 2,6" />
                  </svg>
                  {displayEmail}
                </span>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* 2. MAIN CONTENT */}
      <main className="profile-main-container">
        <div className="profile-center-layout">
          {/* THẺ HỒ SƠ CHÍNH */}
          <section className={`profile-card ${isEditing ? 'editing' : ''}`}>
            <div className="profile-card-header">
              <div className="profile-card-header-title">
                <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                  <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
                  <circle cx="12" cy="7" r="4" />
                </svg>
                <div>
                  <h2>Hồ sơ cá nhân</h2>
                  <p>
                    {isEditing
                      ? 'Cập nhật lại họ tên, ngày sinh và số điện thoại liên hệ'
                      : 'Thông tin tài khoản và thông tin liên hệ của bạn'}
                  </p>
                </div>
              </div>

              {/* HÀNG NÚT THAO TÁC Ở GÓC PHẢI HEADER */}
              <div className="profile-header-actions">
                {!isEditing ? (
                  <>
                    <button
                      type="button"
                      className="btn-password-trigger"
                      onClick={handleOpenPasswordModal}
                      title="Nhấn để đổi mật khẩu tài khoản"
                    >
                      <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.2">
                        <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                        <path d="M7 11V7a5 5 0 0 1 10 0v4" />
                      </svg>
                      <span>Đổi mật khẩu</span>
                    </button>

                    <button
                      type="button"
                      className="btn-edit-trigger"
                      onClick={() => setIsEditing(true)}
                      title="Nhấn để chỉnh sửa thông tin"
                    >
                      <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.2">
                        <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7" />
                        <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z" />
                      </svg>
                      <span>Chỉnh sửa thông tin</span>
                    </button>
                  </>
                ) : (
                  <button
                    type="button"
                    className="btn-cancel-top-trigger"
                    onClick={handleCancelEdit}
                    disabled={saving}
                  >
                    Hủy bỏ
                  </button>
                )}
              </div>
            </div>

            {/* CHẾ ĐỘ XEM (VIEW MODE) */}
            {!isEditing ? (
              <div className="profile-view-box">
                {/* NHÓM 1: THÔNG TIN CÁ NHÂN */}
                <div className="view-section-title">
                  <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
                    <circle cx="12" cy="7" r="4" />
                  </svg>
                  <span>Thông tin liên hệ</span>
                </div>

                <div className="view-grid-2">
                  <div className="view-field-item">
                    <span className="view-label">Họ và tên</span>
                    <span className="view-value strong-text">{fullName || 'Chưa cập nhật'}</span>
                  </div>

                  <div className="view-field-item">
                    <span className="view-label">Giới tính</span>
                    <span className="view-value">
                      {gender === 'true' ? 'Nam' : 'Nữ'}
                    </span>
                  </div>
                </div>

                <div className="view-grid-2">
                  <div className="view-field-item">
                    <span className="view-label">Ngày sinh</span>
                    <span className="view-value">{formatDate(dateOfBirth) || 'Chưa cập nhật'}</span>
                  </div>

                  <div className="view-field-item">
                    <span className="view-label">Số điện thoại</span>
                    <span className="view-value">{phone || 'Chưa cập nhật'}</span>
                  </div>
                </div>

                <div className="view-divider" />

                {/* NHÓM 2: THÔNG TIN TÀI KHOẢN VÀ BẢO MẬT */}
                <div className="view-section-title">
                  <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                    <path d="M7 11V7a5 5 0 0 1 10 0v4" />
                  </svg>
                  <span>Tài khoản & Bảo mật</span>
                </div>

                <div className="view-grid-2">
                  <div className="view-field-item">
                    <span className="view-label">Tên đăng nhập</span>
                    <span className="view-value text-secondary">@{displayUsername}</span>
                  </div>

                  <div className="view-field-item">
                    <span className="view-label">Địa chỉ Email</span>
                    <span className="view-value text-secondary">{displayEmail}</span>
                  </div>
                </div>

                <div className="security-row-inline">
                  <div className="view-field-item">
                    <span className="view-label">Mật khẩu đăng nhập</span>
                    <span className="view-value password-bullets">••••••••••••</span>
                  </div>
                  <div className="security-badges-wrap">
                    <span className="security-badge-active">
                      <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3">
                        <polyline points="20 6 9 17 4 12" />
                      </svg>
                      Đã bảo vệ
                    </span>
                    <button
                      type="button"
                      className="btn-inline-change-pw"
                      onClick={handleOpenPasswordModal}
                    >
                      Đổi mật khẩu
                    </button>
                  </div>
                </div>
              </div>
            ) : (
              /* CHẾ ĐỘ SỬA (EDIT MODE) */
              <form onSubmit={handleSaveProfile} className="profile-edit-form">
                <div className="form-group">
                  <label htmlFor="fullName">
                    Họ và tên <span className="required">*</span>
                  </label>
                  <input
                    id="fullName"
                    type="text"
                    placeholder="Nhập họ và tên đầy đủ"
                    value={fullName}
                    onChange={(e) => setFullName(e.target.value)}
                    required
                    autoFocus
                  />
                </div>

                <div className="form-row-2">
                  <div className="form-group">
                    <label htmlFor="dateOfBirth">
                      Ngày sinh <span className="required">*</span>
                    </label>
                    <input
                      id="dateOfBirth"
                      type="date"
                      value={dateOfBirth}
                      onChange={(e) => setDateOfBirth(e.target.value)}
                      required
                    />
                  </div>

                  <div className="form-group">
                    <label htmlFor="phone">
                      Số điện thoại <span className="required">*</span>
                    </label>
                    <input
                      id="phone"
                      type="tel"
                      placeholder="VD: 0981111111"
                      value={phone}
                      onChange={(e) => setPhone(e.target.value)}
                      required
                    />
                  </div>
                </div>

                <div className="form-group">
                  <label>Giới tính</label>
                  <div className="gender-options">
                    <label className="gender-option">
                      <input
                        type="radio"
                        name="gender"
                        value="true"
                        checked={gender === 'true'}
                        onChange={(e) => setGender(e.target.value)}
                      />
                      <span>Nam</span>
                    </label>
                    <label className="gender-option">
                      <input
                        type="radio"
                        name="gender"
                        value="false"
                        checked={gender === 'false'}
                        onChange={(e) => setGender(e.target.value)}
                      />
                      <span>Nữ</span>
                    </label>
                  </div>
                </div>

                <div className="form-row-2">
                  <div className="form-group">
                    <label htmlFor="username">Tên đăng nhập</label>
                    <input id="username" type="text" value={displayUsername} disabled />
                    <span className="form-readonly-badge">
                      <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                        <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                        <path d="M7 11V7a5 5 0 0 1 10 0v4" />
                      </svg>
                      Cố định
                    </span>
                  </div>

                  <div className="form-group">
                    <label htmlFor="email">Địa chỉ Email</label>
                    <input id="email" type="email" value={displayEmail} disabled />
                    <span className="form-readonly-badge">
                      <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                        <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                        <path d="M7 11V7a5 5 0 0 1 10 0v4" />
                      </svg>
                      Cố định
                    </span>
                  </div>
                </div>

                <div className="edit-form-actions">
                  <button
                    type="button"
                    className="btn-cancel-profile"
                    onClick={handleCancelEdit}
                    disabled={saving}
                  >
                    Hủy bỏ
                  </button>
                  <button
                    type="submit"
                    className="btn-primary-profile"
                    disabled={saving}
                  >
                    {saving ? (
                      <>
                        <span className="spinner" /> Đang lưu…
                      </>
                    ) : (
                      <>
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
                          <path d="M19 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h11l5 5v11a2 2 0 0 1-2 2z" />
                          <polyline points="17 21 17 13 7 13 7 21" />
                          <polyline points="7 3 7 8 15 8" />
                        </svg>
                        Lưu thông tin
                      </>
                    )}
                  </button>
                </div>
              </form>
            )}
          </section>

          {/* 3. LỐI TẮT HỮU ÍCH */}
          <div className="profile-shortcuts-box">
            <h3 className="shortcuts-title">Thao tác nhanh</h3>
            <div className="shortcuts-list">
              <Link to="/ve-cua-toi" className="shortcut-btn">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                  <rect x="3" y="4" width="18" height="16" rx="2" />
                  <path d="M7 8h10M7 12h10M7 16h6" />
                </svg>
                <span>Xem vé của tôi & Lịch sử</span>
              </Link>
              <Link to="/tim-chuyen" className="shortcut-btn">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                  <circle cx="11" cy="11" r="8" />
                  <line x1="21" y1="21" x2="16.65" y2="16.65" />
                </svg>
                <span>Tìm chuyến xe mới</span>
              </Link>
              <Link to="/tra-cuu-ve" className="shortcut-btn">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                  <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z" />
                  <polyline points="14 2 14 8 20 8" />
                  <line x1="16" y1="13" x2="8" y2="13" />
                  <line x1="16" y1="17" x2="8" y2="17" />
                  <polyline points="10 9 9 9 8 9" />
                </svg>
                <span>Tra cứu vé xe</span>
              </Link>
            </div>
          </div>
        </div>
      </main>

      {/* 4. MODAL ĐỔI MẬT KHẨU (CHỈ XUẤT HIỆN KHI NGƯỜI DÙNG NHẤN ĐỔI MẬT KHẨU) */}
      {showPasswordModal && (
        <div className="modal-backdrop" onClick={handleClosePasswordModal}>
          <div
            className="modal-container"
            onClick={(e) => e.stopPropagation()}
            role="dialog"
            aria-modal="true"
            aria-labelledby="modal-password-title"
          >
            <div className="modal-header">
              <div className="modal-header-title">
                <div className="modal-icon-wrap">
                  <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                    <path d="M7 11V7a5 5 0 0 1 10 0v4" />
                  </svg>
                </div>
                <div>
                  <h3 id="modal-password-title">Đổi mật khẩu tài khoản</h3>
                  <p>Mật khẩu mới cần đáp ứng các tiêu chuẩn bảo mật.</p>
                </div>
              </div>
              <button
                type="button"
                className="modal-close-btn"
                onClick={handleClosePasswordModal}
                disabled={pwBusy}
                aria-label="Đóng"
              >
                <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.2">
                  <line x1="18" y1="6" x2="6" y2="18" />
                  <line x1="6" y1="6" x2="18" y2="18" />
                </svg>
              </button>
            </div>

            <form onSubmit={handleChangePassword} className="modal-form">
              {pwError && (
                <div className="form-error-alert">
                  <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <circle cx="12" cy="12" r="10" />
                    <line x1="12" y1="8" x2="12" y2="12" />
                    <line x1="12" y1="16" x2="12.01" y2="16" />
                  </svg>
                  <span>{pwError}</span>
                </div>
              )}

              <div className="form-group">
                <label htmlFor="oldPw">
                  Mật khẩu hiện tại <span className="required">*</span>
                </label>
                <input
                  id="oldPw"
                  type="password"
                  placeholder="Nhập mật khẩu bạn đang dùng"
                  value={oldPassword}
                  onChange={(e) => setOldPassword(e.target.value)}
                  autoComplete="current-password"
                  required
                  autoFocus
                />
              </div>

              <div className="form-group">
                <label htmlFor="newPw">
                  Mật khẩu mới <span className="required">*</span>
                </label>
                <input
                  id="newPw"
                  type="password"
                  placeholder="Nhập mật khẩu mới"
                  value={newPassword}
                  onChange={(e) => setNewPassword(e.target.value)}
                  autoComplete="new-password"
                  required
                />
                <span className="pw-hint">
                  Tối thiểu 8 ký tự, bao gồm chữ hoa, chữ thường, chữ số và ký tự đặc biệt.
                </span>
              </div>

              <div className="form-group">
                <label htmlFor="confirmPw">
                  Xác nhận mật khẩu mới <span className="required">*</span>
                </label>
                <input
                  id="confirmPw"
                  type="password"
                  placeholder="Nhập lại mật khẩu mới"
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                  autoComplete="new-password"
                  required
                />
              </div>

              <div className="modal-actions">
                <button
                  type="button"
                  className="btn-cancel-profile"
                  onClick={handleClosePasswordModal}
                  disabled={pwBusy}
                >
                  Hủy bỏ
                </button>
                <button
                  type="submit"
                  className="btn-primary-profile"
                  disabled={pwBusy}
                >
                  {pwBusy ? (
                    <>
                      <span className="spinner" /> Đang cập nhật…
                    </>
                  ) : (
                    <>
                      <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
                        <polyline points="20 6 9 17 4 12" />
                      </svg>
                      Xác nhận đổi mật khẩu
                    </>
                  )}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
