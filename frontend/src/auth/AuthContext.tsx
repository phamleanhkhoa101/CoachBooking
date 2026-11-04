import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from 'react';
import { Navigate } from 'react-router-dom';
import { api, apiRequest, getToken, setToken } from '../api/client';
import type { LoginResponse, NguoiDung, Quyen } from '../api/types';

interface AuthContextValue {
  nguoiDung: NguoiDung | null;
  loading: boolean;
  login: (tenDangNhap: string, matKhau: string) => Promise<NguoiDung>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | null>(null);

/** Trang mặc định theo quyền sau khi đăng nhập. */
export function homePathFor(quyen: Quyen): string {
  return quyen === 'KhachHang' ? '/app/dat-ve' : '/admin';
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [nguoiDung, setNguoiDung] = useState<NguoiDung | null>(null);
  const [loading, setLoading] = useState<boolean>(Boolean(getToken()));

  useEffect(() => {
    let cancelled = false;
    if (!getToken()) {
      setLoading(false);
      return;
    }
    api
      .get<NguoiDung>('/auth/me', { redirectOn401: false })
      .then((data) => {
        if (!cancelled) setNguoiDung(data);
      })
      .catch(() => {
        if (!cancelled) {
          setToken(null);
          setNguoiDung(null);
        }
      })
      .finally(() => {
        if (!cancelled) setLoading(false);
      });
    return () => {
      cancelled = true;
    };
  }, []);

  const login = useCallback(async (tenDangNhap: string, matKhau: string) => {
    const data = await apiRequest<LoginResponse>(
      '/auth/login',
      {
        method: 'POST',
        body: JSON.stringify({ tenDangNhap, matKhau }),
      },
      { redirectOn401: false },
    );
    setToken(data.token);
    setNguoiDung(data.nguoiDung);
    return data.nguoiDung;
  }, []);

  const logout = useCallback(() => {
    setToken(null);
    setNguoiDung(null);
  }, []);

  const value = useMemo<AuthContextValue>(
    () => ({ nguoiDung, loading, login, logout }),
    [nguoiDung, loading, login, logout],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth(): AuthContextValue {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth phải được dùng bên trong <AuthProvider>.');
  return ctx;
}

/** Chặn route theo quyền: chưa đăng nhập -> /login, sai quyền -> trang mặc định của quyền. */
export function RequireRole({ roles, children }: { roles: Quyen[]; children: ReactNode }) {
  const { nguoiDung, loading } = useAuth();

  if (loading) {
    return (
      <div className="page-loading">
        <span className="spinner" /> Đang tải…
      </div>
    );
  }
  if (!nguoiDung) {
    return <Navigate to="/login" replace />;
  }
  if (!roles.includes(nguoiDung.quyen)) {
    return <Navigate to={homePathFor(nguoiDung.quyen)} replace />;
  }
  return <>{children}</>;
}
