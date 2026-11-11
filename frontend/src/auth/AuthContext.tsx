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
import type { LoginResponse, Role, User } from '../api/types';

interface AuthContextValue {
  user: User | null;
  nguoiDung: User | null; // Alias for backward compatibility
  loading: boolean;
  login: (username: string, password: string, portal?: 'user' | 'admin') => Promise<User>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | null>(null);

/** Trang mặc định theo quyền sau khi đăng nhập. */
export function homePathFor(role?: Role | null): string {
  return role === 'KhachHang' ? '/' : '/admin';
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState<boolean>(Boolean(getToken()));

  useEffect(() => {
    let cancelled = false;
    if (!getToken()) {
      setLoading(false);
      return;
    }
    api
      .get<User>('/auth/me', { redirectOn401: false })
      .then((data) => {
        if (!cancelled) setUser(data);
      })
      .catch(() => {
        if (!cancelled) {
          setToken(null);
          setUser(null);
        }
      })
      .finally(() => {
        if (!cancelled) setLoading(false);
      });
    return () => {
      cancelled = true;
    };
  }, []);

  const login = useCallback(
    async (username: string, password: string, portal?: 'user' | 'admin') => {
      const endpoint =
        portal === 'admin'
          ? '/auth/admin/login'
          : portal === 'user'
            ? '/auth/user/login'
            : '/auth/login';
      const data = await apiRequest<LoginResponse>(
        endpoint,
        {
          method: 'POST',
          body: JSON.stringify({
            username,
            password,
          }),
        },
        { redirectOn401: false },
      );
      setToken(data.token);
      const currentUser = data.user ?? null;
      setUser(currentUser);
      return currentUser!;
    },
    [],
  );

  const logout = useCallback(() => {
    setToken(null);
    setUser(null);
  }, []);

  const value = useMemo<AuthContextValue>(
    () => ({ user, nguoiDung: user, loading, login, logout }),
    [user, loading, login, logout],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth(): AuthContextValue {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth phải được dùng bên trong <AuthProvider>.');
  return ctx;
}

/** Chặn route theo quyền: chưa đăng nhập -> /login, sai quyền -> trang mặc định của quyền. */
export function RequireRole({ roles, children }: { roles: Role[]; children: ReactNode }) {
  const { user, loading } = useAuth();

  if (loading) {
    return (
      <div className="page-loading">
        <span className="spinner" /> Đang tải…
      </div>
    );
  }
  if (!user) {
    return <Navigate to="/login" replace />;
  }
  const currentRole = user.role ?? user.quyen;
  if (!currentRole || !roles.includes(currentRole)) {
    return <Navigate to={homePathFor(currentRole)} replace />;
  }
  return <>{children}</>;
}
