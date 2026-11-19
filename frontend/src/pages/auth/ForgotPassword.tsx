import { ClipboardEvent, KeyboardEvent, useEffect, useRef, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { apiRequest, getErrorMessage } from '../../api/client';
import { isAdminPortal } from '../../auth/portal';
import { AuthShell } from '../../components/layout/AuthShell';

type Step = 'email' | 'otp' | 'password';

const OTP_LENGTH = 6;
const PASSWORD_HINT = 'Tối thiểu 8 ký tự, gồm chữ hoa, chữ thường, chữ số và ký tự đặc biệt.';

function isStrongPassword(value: string): boolean {
  return (
    value.length >= 8 &&
    /[A-Z]/.test(value) &&
    /[a-z]/.test(value) &&
    /[0-9]/.test(value) &&
    /[^A-Za-z0-9]/.test(value)
  );
}

function passwordStrength(value: string): { score: number; label: string } {
  if (!value) return { score: 0, label: 'Chưa nhập' };
  let score = 0;
  if (value.length >= 8) score += 1;
  if (/[A-Z]/.test(value) && /[a-z]/.test(value)) score += 1;
  if (/[0-9]/.test(value)) score += 1;
  if (/[^A-Za-z0-9]/.test(value)) score += 1;

  if (score <= 1) return { score: 1, label: 'Yếu' };
  if (score === 2) return { score: 2, label: 'Trung bình' };
  if (score === 3) return { score: 3, label: 'Mạnh' };
  return { score: 4, label: 'Rất mạnh' };
}

function EyeToggleIcon({ visible }: { visible: boolean }) {
  return visible ? (
    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
      <path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24"></path>
      <line x1="1" y1="1" x2="23" y2="23"></line>
    </svg>
  ) : (
    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
      <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"></path>
      <circle cx="12" cy="12" r="3"></circle>
    </svg>
  );
}

export function ForgotPassword() {
  const navigate = useNavigate();

  const [step, setStep] = useState<Step>('email');
  const [email, setEmail] = useState('');
  const [otpDigits, setOtpDigits] = useState<string[]>(Array(OTP_LENGTH).fill(''));
  const [resetToken, setResetToken] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [showNewPassword, setShowNewPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [cooldown, setCooldown] = useState(0);
  const [error, setError] = useState('');
  const [info, setInfo] = useState('');
  const [busy, setBusy] = useState(false);

  const otpRefs = useRef<Array<HTMLInputElement | null>>([]);

  useEffect(() => {
    if (cooldown <= 0) return;
    const timer = window.setInterval(() => {
      setCooldown((prev) => (prev > 0 ? prev - 1 : 0));
    }, 1000);
    return () => window.clearInterval(timer);
  }, [cooldown]);

  useEffect(() => {
    if (step === 'otp') {
      otpRefs.current[0]?.focus();
    }
  }, [step]);

  const handleRequestOtp = async () => {
    const trimmedEmail = email.trim();
    if (!trimmedEmail || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(trimmedEmail)) {
      setError('Vui lòng nhập địa chỉ email hợp lệ.');
      return;
    }

    setBusy(true);
    setError('');
    setInfo('');
    try {
      const data = await apiRequest<{ message?: string }>(
        '/auth/forgot-password',
        {
          method: 'POST',
          body: JSON.stringify({ email: trimmedEmail }),
        },
        { redirectOn401: false },
      );
      setInfo(
        data?.message ??
          `Mã OTP 6 chữ số đã được gửi tới ${trimmedEmail}. Vui lòng kiểm tra hộp thư.`,
      );
      setOtpDigits(Array(OTP_LENGTH).fill(''));
      setResetToken('');
      setStep('otp');
      setCooldown(60);
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setBusy(false);
    }
  };

  const handleOtpChange = (index: number, rawValue: string) => {
    const digit = rawValue.replace(/\D/g, '').slice(-1);
    const next = [...otpDigits];
    next[index] = digit;
    setOtpDigits(next);
    if (digit && index < OTP_LENGTH - 1) {
      otpRefs.current[index + 1]?.focus();
    }
  };

  const handleOtpKeyDown = (index: number, event: KeyboardEvent<HTMLInputElement>) => {
    if (event.key === 'Backspace') {
      if (otpDigits[index]) {
        const next = [...otpDigits];
        next[index] = '';
        setOtpDigits(next);
      } else if (index > 0) {
        const next = [...otpDigits];
        next[index - 1] = '';
        setOtpDigits(next);
        otpRefs.current[index - 1]?.focus();
      }
      event.preventDefault();
    } else if (event.key === 'ArrowLeft' && index > 0) {
      otpRefs.current[index - 1]?.focus();
    } else if (event.key === 'ArrowRight' && index < OTP_LENGTH - 1) {
      otpRefs.current[index + 1]?.focus();
    }
  };

  const handleOtpPaste = (event: ClipboardEvent<HTMLInputElement>) => {
    const pasted = event.clipboardData.getData('text').replace(/\D/g, '').slice(0, OTP_LENGTH);
    if (!pasted) return;
    event.preventDefault();
    const next = Array(OTP_LENGTH).fill('');
    pasted.split('').forEach((char, idx) => {
      next[idx] = char;
    });
    setOtpDigits(next);
    const focusIndex = Math.min(pasted.length, OTP_LENGTH - 1);
    otpRefs.current[focusIndex]?.focus();
  };

  const handleVerifyOtp = async () => {
    const otp = otpDigits.join('');
    if (!/^\d{6}$/.test(otp)) {
      setError('Vui lòng nhập đầy đủ 6 chữ số OTP.');
      return;
    }

    setBusy(true);
    setError('');
    setInfo('');
    try {
      const data = await apiRequest<{ message?: string; resetToken?: string }>(
        '/auth/verify-reset-otp',
        {
          method: 'POST',
          body: JSON.stringify({
            email: email.trim(),
            otp,
          }),
        },
        { redirectOn401: false },
      );
      if (!data?.resetToken) {
        setError('Không nhận được mã phiên xác thực. Vui lòng thử lại.');
        return;
      }
      setResetToken(data.resetToken);
      setInfo(`Xác thực OTP thành công cho ${email.trim()}. Vui lòng đặt mật khẩu mới.`);
      setStep('password');
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setBusy(false);
    }
  };

  const handleResetPassword = async () => {
    if (!newPassword || !confirmPassword) {
      setError('Vui lòng nhập mật khẩu mới và xác nhận mật khẩu.');
      return;
    }
    if (!isStrongPassword(newPassword)) {
      setError(PASSWORD_HINT);
      return;
    }
    if (newPassword !== confirmPassword) {
      setError('Mật khẩu xác nhận không khớp.');
      return;
    }

    setBusy(true);
    setError('');
    try {
      const data = await apiRequest<{ message?: string }>(
        '/auth/reset-password',
        {
          method: 'POST',
          body: JSON.stringify({
            email: email.trim(),
            resetToken,
            newPassword,
          }),
        },
        { redirectOn401: false },
      );
      navigate('/login', {
        replace: true,
        state: {
          notice:
            data?.message ??
            'Đặt lại mật khẩu thành công. Bạn có thể đăng nhập bằng mật khẩu mới.',
        },
      });
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setBusy(false);
    }
  };

  const strength = passwordStrength(newPassword);
  const stepIndex = step === 'email' ? 1 : step === 'otp' ? 2 : 3;
  const admin = isAdminPortal();

  return (
    <AuthShell
      wide
      showBrandTitle={!admin}
      showBackHome={!admin}
      title={
        step === 'email'
          ? 'Quên mật khẩu'
          : step === 'otp'
            ? 'Xác thực mã OTP'
            : 'Đặt lại mật khẩu mới'
      }
      subtitle={
        step === 'email'
          ? 'Nhập email đã đăng ký để nhận mã xác thực 6 chữ số'
          : step === 'otp'
            ? `Nhập mã 6 chữ số vừa gửi tới ${email.trim()}`
            : 'Tạo mật khẩu mới để bảo vệ tài khoản của bạn'
      }
      topSlot={
        <div className="auth-steps" aria-label="Tiến trình đặt lại mật khẩu">
          <div className={`auth-step ${stepIndex === 1 ? 'is-active' : 'is-done'}`}>
            {stepIndex > 1 ? '✓ ' : ''}1. Nhập Email
          </div>
          <div className={`auth-step ${stepIndex === 2 ? 'is-active' : stepIndex > 2 ? 'is-done' : ''}`}>
            {stepIndex > 2 ? '✓ ' : ''}2. Xác thực OTP
          </div>
          <div className={`auth-step ${stepIndex === 3 ? 'is-active' : ''}`}>
            3. Mật khẩu mới
          </div>
        </div>
      }
      footer={<Link to="/login">← Quay lại đăng nhập</Link>}
    >
      {error ? <div className="alert alert-error">{error}</div> : null}
      {info ? <div className="alert alert-success">{info}</div> : null}

        {step === 'email' ? (
          <form
            className="auth-form"
            onSubmit={(event) => {
              event.preventDefault();
              void handleRequestOtp();
            }}
          >
            <div className="auth-input-group">
              <label className="auth-input-label" htmlFor="forgot-email">
                Email <span className="required-star">*</span>
              </label>
              <div className="auth-input-wrapper">
                <span className="auth-input-icon">
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                    <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"></path>
                    <polyline points="22,6 12,13 2,6"></polyline>
                  </svg>
                </span>
                <input
                  id="forgot-email"
                  type="email"
                  className="auth-input"
                  placeholder="example@gmail.com"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  autoComplete="email"
                  required
                />
              </div>
            </div>

            <div className="auth-hint-box">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                <circle cx="12" cy="12" r="10"></circle>
                <line x1="12" y1="16" x2="12" y2="12"></line>
                <line x1="12" y1="8" x2="12.01" y2="8"></line>
              </svg>
              <span>
                Mã OTP có hiệu lực trong <strong>15 phút</strong>. Vui lòng kiểm tra cả hộp thư
                chính và thư rác (Spam).
              </span>
            </div>

            <button type="submit" className="btn-auth-submit" disabled={busy}>
              <span>{busy ? 'Đang gửi OTP…' : 'Gửi mã OTP qua Email'}</span>
              {!busy && (
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                  <line x1="5" y1="12" x2="19" y2="12"></line>
                  <polyline points="12 5 19 12 12 19"></polyline>
                </svg>
              )}
            </button>
          </form>
        ) : null}

        {step === 'otp' ? (
          <form
            className="auth-form"
            onSubmit={(event) => {
              event.preventDefault();
              void handleVerifyOtp();
            }}
          >
            <div className="otp-boxes" role="group" aria-label="Nhập mã OTP 6 chữ số">
              {otpDigits.map((digit, index) => (
                <input
                  key={index}
                  ref={(el) => {
                    otpRefs.current[index] = el;
                  }}
                  type="text"
                  inputMode="numeric"
                  autoComplete={index === 0 ? 'one-time-code' : 'off'}
                  maxLength={1}
                  aria-label={`Chữ số OTP thứ ${index + 1}`}
                  className={`otp-box mono ${digit ? 'is-filled' : ''}`}
                  value={digit}
                  onChange={(e) => handleOtpChange(index, e.target.value)}
                  onKeyDown={(e) => handleOtpKeyDown(index, e)}
                  onPaste={handleOtpPaste}
                />
              ))}
            </div>

            <div className="otp-expiry-note">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                <circle cx="12" cy="12" r="10"></circle>
                <polyline points="12 6 12 12 16 14"></polyline>
              </svg>
              <span>
                Mã OTP có hiệu lực trong <strong>15 phút</strong>
              </span>
            </div>

            <button type="submit" className="btn-auth-submit" disabled={busy}>
              <span>{busy ? 'Đang xác nhận…' : 'Xác nhận mã OTP'}</span>
              {!busy && (
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                  <line x1="5" y1="12" x2="19" y2="12"></line>
                  <polyline points="12 5 19 12 12 19"></polyline>
                </svg>
              )}
            </button>

            <div className="auth-options-row">
              <button
                type="button"
                className="auth-link-btn"
                disabled={busy || cooldown > 0}
                onClick={() => void handleRequestOtp()}
              >
                {cooldown > 0 ? `Gửi lại mã OTP (${cooldown}s)` : 'Gửi lại mã OTP'}
              </button>
              <button
                type="button"
                className="auth-link-btn"
                disabled={busy}
                onClick={() => {
                  setStep('email');
                  setError('');
                  setInfo('');
                  setOtpDigits(Array(OTP_LENGTH).fill(''));
                }}
              >
                Đổi email khác
              </button>
            </div>
          </form>
        ) : null}

        {step === 'password' ? (
          <form
            className="auth-form"
            onSubmit={(event) => {
              event.preventDefault();
              void handleResetPassword();
            }}
          >
            <div className="auth-input-group">
              <label className="auth-input-label" htmlFor="reset-new-password">
                Mật khẩu mới <span className="required-star">*</span>
              </label>
              <div className="auth-input-wrapper">
                <span className="auth-input-icon">
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                    <rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect>
                    <path d="M7 11V7a5 5 0 0 1 10 0v4"></path>
                  </svg>
                </span>
                <input
                  id="reset-new-password"
                  type={showNewPassword ? 'text' : 'password'}
                  className="auth-input has-toggle"
                  placeholder="Nhập mật khẩu mới"
                  value={newPassword}
                  onChange={(e) => setNewPassword(e.target.value)}
                  autoComplete="new-password"
                  required
                />
                <button
                  type="button"
                  className="auth-input-toggle"
                  onClick={() => setShowNewPassword((prev) => !prev)}
                  title={showNewPassword ? 'Ẩn mật khẩu' : 'Hiện mật khẩu'}
                >
                  <EyeToggleIcon visible={showNewPassword} />
                </button>
              </div>
              {newPassword.length > 0 && !isStrongPassword(newPassword) && (
                <span className="auth-field-error">
                  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                    <circle cx="12" cy="12" r="10"></circle>
                    <line x1="12" y1="8" x2="12" y2="12"></line>
                    <line x1="12" y1="16" x2="12.01" y2="16"></line>
                  </svg>
                  {PASSWORD_HINT}
                </span>
              )}
              <div className="password-strength">
                <div className="password-strength-header">
                  <span>Độ mạnh mật khẩu:</span>
                  <strong>{strength.label}</strong>
                </div>
                <div className="password-strength-bars">
                  {[1, 2, 3, 4].map((bar) => (
                    <div
                      key={bar}
                      className={`password-strength-bar ${
                        strength.score >= bar ? `level-${strength.score}` : ''
                      }`}
                    />
                  ))}
                </div>
              </div>
            </div>

            <div className="auth-input-group">
              <label className="auth-input-label" htmlFor="reset-confirm-password">
                Xác nhận mật khẩu mới <span className="required-star">*</span>
              </label>
              <div className="auth-input-wrapper">
                <span className="auth-input-icon">
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                    <rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect>
                    <path d="M7 11V7a5 5 0 0 1 10 0v4"></path>
                  </svg>
                </span>
                <input
                  id="reset-confirm-password"
                  type={showConfirmPassword ? 'text' : 'password'}
                  className="auth-input has-toggle"
                  placeholder="Nhập lại mật khẩu mới"
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                  autoComplete="new-password"
                  required
                />
                <button
                  type="button"
                  className="auth-input-toggle"
                  onClick={() => setShowConfirmPassword((prev) => !prev)}
                  title={showConfirmPassword ? 'Ẩn mật khẩu' : 'Hiện mật khẩu'}
                >
                  <EyeToggleIcon visible={showConfirmPassword} />
                </button>
              </div>
            </div>

            <button type="submit" className="btn-auth-submit" disabled={busy}>
              <span>{busy ? 'Đang cập nhật…' : 'Xác nhận đặt lại mật khẩu'}</span>
              {!busy && (
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                  <line x1="5" y1="12" x2="19" y2="12"></line>
                  <polyline points="12 5 19 12 12 19"></polyline>
                </svg>
              )}
            </button>
          </form>
        ) : null}
    </AuthShell>
  );
}
