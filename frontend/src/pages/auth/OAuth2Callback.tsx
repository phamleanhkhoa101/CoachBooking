import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { getErrorMessage } from '../../api/client';
import { homePathFor, useAuth } from '../../auth/AuthContext';
import { AuthShell } from '../../components/layout/AuthShell';

export function OAuth2Callback() {
  const { restoreSession } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    let active = true;
    const errorParam = new URLSearchParams(window.location.search).get('error');

    if (errorParam) {
      navigate(`/login?error=${encodeURIComponent(errorParam)}`, { replace: true });
      return;
    }

    restoreSession()
      .then((loggedUser) => {
        if (!active) return;
        navigate(homePathFor(loggedUser.role), { replace: true });
      })
      .catch((err) => {
        if (!active) return;
        navigate(`/login?error=${encodeURIComponent(getErrorMessage(err))}`, { replace: true });
      });

    return () => {
      active = false;
    };
  }, [restoreSession, navigate]);

  return (
    <AuthShell
      title="Đang xác thực Google"
      subtitle="Vui lòng chờ trong giây lát khi hệ thống hoàn tất phiên đăng nhập"
      showBackHome={false}
    >
      <div className="auth-callback-state" aria-busy="true">
        <span className="spinner" />
        <span>Đang kết nối tài khoản của bạn…</span>
      </div>
    </AuthShell>
  );
}
