import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { getErrorMessage } from '../../api/client';
import { homePathFor, useAuth } from '../../auth/AuthContext';

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
    <div className="page-loading">
      <span className="spinner" /> Đang hoàn tất đăng nhập Google…
    </div>
  );
}
