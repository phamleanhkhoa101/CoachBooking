import { useCallback, useEffect, useState } from 'react';
import { api, getErrorMessage } from '../api/client';
import type { HoSo } from '../api/types';
import { FormField } from '../components/FormField';
import { useToast } from '../components/Toast';
import { toDateInput } from '../utils/format';

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
  const [hoSo, setHoSo] = useState<HoSo | null>(null);
  const [loading, setLoading] = useState(true);

  const [hoTen, setHoTen] = useState('');
  const [namSinh, setNamSinh] = useState('');
  const [sdt, setSdt] = useState('');
  const [gioiTinh, setGioiTinh] = useState('true');
  const [diaChi, setDiaChi] = useState('');
  const [saving, setSaving] = useState(false);

  const [matKhauCu, setMatKhauCu] = useState('');
  const [matKhauMoi, setMatKhauMoi] = useState('');
  const [xacNhan, setXacNhan] = useState('');
  const [pwError, setPwError] = useState('');
  const [pwBusy, setPwBusy] = useState(false);

  const applyData = useCallback((data: HoSo) => {
    setHoSo(data);
    setHoTen(data.hoTen ?? '');
    setNamSinh(toDateInput(data.namSinh));
    setSdt(data.sdt ?? '');
    setGioiTinh(data.gioiTinh === false ? 'false' : 'true');
    setDiaChi(data.diaChi ?? '');
  }, []);

  useEffect(() => {
    void (async () => {
      try {
        applyData(await api.get<HoSo>('/ho-so'));
      } catch (error) {
        showToast(getErrorMessage(error), 'error');
      } finally {
        setLoading(false);
      }
    })();
  }, [applyData, showToast]);

  const isNhanVien = hoSo?.loai === 'NHANVIEN';
  const chuaGanHoSo = hoSo?.loai === 'CHUA_GAN';

  const handleSave = async () => {
    if (chuaGanHoSo) {
      showToast('Tài khoản chưa được gắn với hồ sơ nhân viên hoặc khách hàng.', 'error');
      return;
    }
    if (!hoTen.trim()) {
      showToast('Họ tên không được để trống.', 'error');
      return;
    }
    if (!namSinh) {
      showToast('Vui lòng nhập ngày sinh.', 'error');
      return;
    }
    if (!/^\d{10,11}$/.test(sdt.trim())) {
      showToast('Số điện thoại phải gồm 10 đến 11 chữ số.', 'error');
      return;
    }
    setSaving(true);
    try {
      const body: Record<string, unknown> = {
        hoTen: hoTen.trim(),
        namSinh,
        sdt: sdt.trim(),
        gioiTinh: gioiTinh === 'true',
      };
      if (isNhanVien) body.diaChi = diaChi.trim() || null;
      const data = await api.put<HoSo>('/ho-so', body);
      applyData(data);
      showToast('Đã cập nhật hồ sơ.');
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setSaving(false);
    }
  };

  const handleChangePassword = async () => {
    if (!matKhauCu) {
      setPwError('Vui lòng nhập mật khẩu hiện tại.');
      return;
    }
    if (!isStrongPassword(matKhauMoi)) {
      setPwError('Mật khẩu mới tối thiểu 8 ký tự, gồm chữ hoa, chữ thường, chữ số và ký tự đặc biệt.');
      return;
    }
    if (matKhauMoi !== xacNhan) {
      setPwError('Mật khẩu xác nhận không khớp.');
      return;
    }
    setPwBusy(true);
    setPwError('');
    try {
      await api.post('/auth/doi-mat-khau', { matKhauCu, matKhauMoi });
      showToast('Đã đổi mật khẩu thành công.');
      setMatKhauCu('');
      setMatKhauMoi('');
      setXacNhan('');
    } catch (error) {
      setPwError(getErrorMessage(error));
    } finally {
      setPwBusy(false);
    }
  };

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>Hồ sơ cá nhân</h1>
          <p className="page-subtitle">Cập nhật thông tin liên hệ và đổi mật khẩu đăng nhập.</p>
        </div>
      </div>

      {loading ? (
        <div className="card">
          <div className="empty-state">
            <span className="spinner" /> Đang tải hồ sơ…
          </div>
        </div>
      ) : (
        <div className="grid-2">
          <section className="card">
            <div className="card-header">
              <h2>Thông tin cá nhân</h2>
              <span className="badge badge-info">
                {chuaGanHoSo ? 'Chưa gắn hồ sơ' : isNhanVien ? 'Nhân viên' : 'Khách hàng'}
              </span>
            </div>

            <div className="info-grid info-grid-tight">
              <div>
                <span className="text-muted">Tên đăng nhập</span>
                <strong>{hoSo?.tenDangNhap ?? '—'}</strong>
              </div>
              <div>
                <span className="text-muted">Email</span>
                <strong>{hoSo?.email ?? '—'}</strong>
              </div>
            </div>

            <form
              className="form-grid"
              onSubmit={(event) => {
                event.preventDefault();
                void handleSave();
              }}
            >
              <FormField label="Họ và tên" value={hoTen} onChange={setHoTen} required />
              <FormField label="Ngày sinh" type="date" value={namSinh} onChange={setNamSinh} />
              <FormField label="Số điện thoại" value={sdt} onChange={setSdt} placeholder="09xxxxxxxx" />
              <FormField
                label="Giới tính"
                type="select"
                value={gioiTinh}
                onChange={setGioiTinh}
                options={[
                  { value: 'true', label: 'Nam' },
                  { value: 'false', label: 'Nữ' },
                ]}
              />
              {isNhanVien ? (
                <FormField label="Địa chỉ" value={diaChi} onChange={setDiaChi} span={2} />
              ) : null}
              <div className="field field-span-2 form-actions">
                <button type="submit" className="btn btn-primary" disabled={saving}>
                  {saving ? 'Đang lưu…' : 'Lưu thay đổi'}
                </button>
              </div>
            </form>
          </section>

          <section className="card">
            <div className="card-header">
              <h2>Đổi mật khẩu</h2>
            </div>
            {pwError ? <div className="alert alert-error">{pwError}</div> : null}
            <form
              className="form-grid"
              onSubmit={(event) => {
                event.preventDefault();
                void handleChangePassword();
              }}
            >
              <FormField
                label="Mật khẩu hiện tại"
                type="password"
                value={matKhauCu}
                onChange={setMatKhauCu}
                autoComplete="current-password"
                required
              />
              <FormField
                label="Mật khẩu mới"
                type="password"
                value={matKhauMoi}
                onChange={setMatKhauMoi}
                autoComplete="new-password"
                required
                help="Tối thiểu 8 ký tự, gồm chữ hoa, chữ thường, chữ số và ký tự đặc biệt."
              />
              <FormField
                label="Xác nhận mật khẩu mới"
                type="password"
                value={xacNhan}
                onChange={setXacNhan}
                autoComplete="new-password"
                required
              />
              <div className="field field-span-2 form-actions">
                <button type="submit" className="btn btn-primary" disabled={pwBusy}>
                  {pwBusy ? 'Đang cập nhật…' : 'Đổi mật khẩu'}
                </button>
              </div>
            </form>
          </section>
        </div>
      )}
    </div>
  );
}
