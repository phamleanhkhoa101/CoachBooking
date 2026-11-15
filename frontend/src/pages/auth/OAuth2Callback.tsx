import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { getErrorMessage } from '../../api/client';
import { homePathFor, useAuth } from '../../auth/AuthContext';

export function OAuth2Callback() {
  const { loginWithToken } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    let active = true;
    const hash = window.location.hash.startsWith('#')
      ? window.location.hash.slice(1)
      : window.location.hash;
    const hashParams = new URLSearchParams(hash);
    const searchParams = new URLSearchParams(window.location.search);

    const token = hashParams.get('token');
    const errorParam = searchParams.get('error') || hashParams.get('error');

    if (errorParam) {
      navigate(`/login?error=${encodeURIComponent(errorParam)}`, { replace: true });
      return;
    }

    if (!token) {
      navigate(
        `/login?error=${encodeURIComponent('Không nhận được mã xác thực từ Google.')}`,
        { replace: true },
      );
      return;
    }

    // Xóa token khỏi URL fragment trước khi xử lý tiếp
    window.history.replaceState(null, '', window.location.pathname);

    loginWithToken(token)
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
  }, [loginWithToken, navigate]);

  return (
    <div className="page-loading">
      <span className="spinner" /> Đang hoàn tất đăng nhập Google…
    </div>
  );
}
