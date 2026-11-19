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
import { ApiError, getErrorMessage, loginSession, logoutSession, refreshSession, subscribeSession } from '../api/client';
import type { Role, User } from '../api/types';
import { isAdminPortal } from './portal';

interface AuthContextValue {
  user: User | null;
  nguoiDung: User | null; // Alias for backward compatibility
  loading: boolean;
  login: (username: string, password: string, portal?: 'user' | 'admin', rememberMe?: boolean) => Promise<User>;
  restoreSession: () => Promise<User>;
  logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextValue | null>(null);

/** Trang mặc định theo quyền sau khi đăng nhập. */
export function homePathFor(role?: Role | null): string {
  return role === 'KhachHang' ? '/' : '/admin';
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);
  const [restoreError, setRestoreError] = useState('');

  const restoreSession = useCallback(async () => {
    const data = await refreshSession();
    setUser(data.user);
    return data.user;
  }, []);

  const restore = useCallback(async () => {
    setLoading(true); setRestoreError('');
    try { await restoreSession(); }
    catch (error) {
      if (error instanceof ApiError && error.status === 401) setUser(null);
      else setRestoreError(getErrorMessage(error));
    } finally { setLoading(false); }
  }, [restoreSession]);

  useEffect(() => {
    const unsubscribe = subscribeSession(session => setUser(session?.user ?? null));
    void restore();
    return unsubscribe;
  }, [restore]);

  const login = useCallback(async (username: string, password: string, portal?: 'user' | 'admin', rememberMe = false) => {
    const data = await loginSession(username, password, portal, rememberMe);
    setUser(data.user);
    return data.user;
  }, []);

  const logout = useCallback(async () => {
    await logoutSession();
    setUser(null);
  }, []);

  const value = useMemo<AuthContextValue>(
    () => ({ user, nguoiDung: user, loading, login, restoreSession, logout }),
    [user, loading, login, restoreSession, logout],
  );

  return <AuthContext.Provider value={value}>
    {restoreError ? <div className="alert alert-error" role="alert">
      {restoreError} <button className="btn btn-secondary" disabled={loading} onClick={() => { void restore(); }}>Thử lại</button>
    </div> : children}
  </AuthContext.Provider>;
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
  const currentRole = user.role;
  if (!currentRole || !roles.includes(currentRole)) {
    if (isAdminPortal()) {
      return <Navigate to="/login" replace />;
    }
    return <Navigate to={homePathFor(currentRole)} replace />;
  }
  return <>{children}</>;
}
