import type { ApiErrorBody, LoginResponse } from './types';

export const TOKEN_KEY = 'coachbooking_token';

/** Lỗi gọi API: mang theo mã HTTP và message hiển thị được cho người dùng. */
export class ApiError extends Error {
  status: number;

  constructor(status: number, message: string) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
  }
}

let accessToken: string | null = null;
let currentSession: LoginResponse | null = null;
let refreshPromise: Promise<LoginResponse> | null = null;
let generation = 0;
let logoutPending = false;
const channel = typeof BroadcastChannel !== 'undefined' ? new BroadcastChannel('coachbooking_auth') : null;
const listeners = new Set<(session: LoginResponse | null) => void>();
localStorage.removeItem(TOKEN_KEY);

export function getToken(): string | null { return accessToken; }

function applySession(session: LoginResponse | null, broadcast = true): void {
  currentSession = session;
  accessToken = session?.token ?? null;
  listeners.forEach(listener => listener(session));
  if (broadcast) channel?.postMessage({ type: 'session', session });
}

export function subscribeSession(listener: (session: LoginResponse | null) => void): () => void {
  listeners.add(listener);
  return () => { listeners.delete(listener); };
}

channel?.addEventListener('message', event => {
  if (event.data?.type === 'request') {
    if (currentSession && !logoutPending) channel.postMessage({ type: 'session', session: currentSession });
  } else if (event.data?.type === 'session') {
    if (!event.data.session) { generation++; logoutPending = false; }
    if (!logoutPending) applySession(event.data.session, false);
  }
});

async function withAuthLock<T>(action: () => Promise<T>): Promise<T> {
  if (navigator.locks) return navigator.locks.request('coachbooking_auth', action);
  throw new ApiError(0, 'Trình duyệt cần hỗ trợ Web Locks để quản lý phiên an toàn.');
}

function usableToken(token: string | null): boolean {
  if (!token) return false;
  try { return JSON.parse(atob(token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/'))).exp * 1000 > Date.now() + 5000; }
  catch { return false; }
}

async function authPost(path: string, body?: unknown): Promise<LoginResponse> {
  let response: Response;
  try {
    response = await fetch(`/api/auth${path}`, {
      method: 'POST', credentials: 'same-origin',
      headers: { 'Content-Type': 'application/json', 'X-Auth-Request': '1' },
      body: body === undefined ? undefined : JSON.stringify(body),
    });
  } catch { throw new ApiError(0, 'Không kết nối được tới máy chủ. Vui lòng thử lại.'); }
  if (!response.ok) {
    const data = await response.json().catch(() => null);
    throw new ApiError(response.status, data?.message ?? defaultMessage(response.status));
  }
  return response.status === 204 ? undefined as never : response.json();
}

export function refreshSession(rejectedToken?: string | null): Promise<LoginResponse> {
  if (logoutPending) return Promise.reject(new ApiError(401, 'Đang đăng xuất.'));
  if (refreshPromise) return refreshPromise;
  const expectedGeneration = generation;
  refreshPromise = withAuthLock(async () => {
    channel?.postMessage({ type: 'request' });
    await new Promise(resolve => setTimeout(resolve, 60));
    if (expectedGeneration !== generation || logoutPending) throw new ApiError(401, 'Phiên đã kết thúc.');
    if (currentSession && usableToken(accessToken) && (rejectedToken === undefined || accessToken !== rejectedToken)) return currentSession;
    try {
      const session = await authPost('/refresh');
      if (expectedGeneration !== generation || logoutPending) throw new ApiError(401, 'Phiên đã kết thúc.');
      applySession(session);
      return session;
    } catch (error) {
      if (error instanceof ApiError && error.status === 401 && expectedGeneration === generation) {
        generation++; applySession(null);
      }
      throw error;
    }
  }).finally(() => { refreshPromise = null; });
  return refreshPromise;
}

export async function loginSession(username: string, password: string, portal: 'user' | 'admin' | undefined, rememberMe: boolean): Promise<LoginResponse> {
  return withAuthLock(async () => {
    const session = await authPost(portal ? `/${portal}/login` : '/login', { username, password, rememberMe });
    generation++; logoutPending = false; applySession(session);
    return session;
  });
}

export async function logoutSession(): Promise<void> {
  logoutPending = true; generation++;
  try {
    await withAuthLock(async () => { await authPost('/logout'); applySession(null); });
  } finally { logoutPending = false; }
}

function handleUnauthorized(): void {
  generation++; applySession(null);
  // Không đẩy về /login nếu đang ở chính trang đăng nhập (tránh vòng lặp).
  if (window.location.pathname !== '/login') {
    window.location.replace('/login');
  }
}

interface RequestConfig {
  /** Mặc định true: gặp 401 sẽ xóa token và chuyển về /login. */
  redirectOn401?: boolean;
}

function parseBody(text: string): unknown {
  if (!text) return null;
  try {
    return JSON.parse(text) as unknown;
  } catch {
    return null;
  }
}

function defaultMessage(status: number): string {
  switch (status) {
    case 400:
      return 'Dữ liệu không hợp lệ.';
    case 401:
      return 'Bạn cần đăng nhập để thực hiện thao tác này.';
    case 403:
      return 'Bạn không có quyền thực hiện thao tác này.';
    case 404:
      return 'Không tìm thấy dữ liệu.';
    case 409:
      return 'Dữ liệu đã bị thay đổi, vui lòng thử lại.';
    default:
      return 'Đã có lỗi xảy ra, vui lòng thử lại.';
  }
}

/** Gọi API dùng chung: tự gắn token, tự đọc lỗi `message` từ response. */
export async function apiRequest<T>(
  path: string,
  options: RequestInit = {},
  config: RequestConfig = {},
): Promise<T> {
  const { redirectOn401 = true } = config;
  const headers = new Headers(options.headers);
  if (options.body !== undefined && options.body !== null && !headers.has('Content-Type')) {
    headers.set('Content-Type', 'application/json');
  }
  const token = getToken();
  if (token) {
    headers.set('Authorization', `Bearer ${token}`);
  }

  let response: Response;
  try {
    response = await fetch(`/api${path}`, { ...options, headers, credentials: 'same-origin' });
  } catch {
    throw new ApiError(0, 'Không kết nối được tới máy chủ. Vui lòng kiểm tra backend đã chạy chưa.');
  }

  if (response.status === 401 && redirectOn401 && response.headers.get('X-Auth-Expired') === '1') {
    if (document.visibilityState === 'hidden') throw new ApiError(401, 'Vui lòng mở lại ứng dụng để tiếp tục.');
    try {
      const session = await refreshSession(token);
      headers.set('Authorization', `Bearer ${session.token}`);
    } catch (error) {
      if (error instanceof ApiError && error.status === 401) handleUnauthorized();
      throw error;
    }
    // Retry only authentication-filter rejections, before business handlers execute.
    try { response = await fetch(`/api${path}`, { ...options, headers, credentials: 'same-origin' }); }
    catch { throw new ApiError(0, 'Không kết nối được tới máy chủ. Vui lòng thử lại.'); }
    if (response.status === 401 && response.headers.get('X-Auth-Expired') === '1') {
      handleUnauthorized();
      throw new ApiError(401, 'Phiên đăng nhập đã hết hạn, vui lòng đăng nhập lại.');
    }
  }

  if (response.status === 204) {
    return undefined as T;
  }

  const raw = await response.text();
  const data = parseBody(raw);

  if (!response.ok) {
    const message =
      data && typeof data === 'object' && typeof (data as ApiErrorBody).message === 'string'
        ? (data as ApiErrorBody).message
        : defaultMessage(response.status);
    throw new ApiError(response.status, message);
  }

  if (path === '/auth/change-password') {
    generation++; applySession(null);
    window.location.replace('/login?passwordChanged=1');
  }

  return data as T;
}

type QueryValue = string | number | boolean | null | undefined;

/** Ghép query string, bỏ qua giá trị rỗng/null. */
export function buildQuery(params: Record<string, QueryValue>): string {
  const search = new URLSearchParams();
  Object.entries(params).forEach(([key, value]) => {
    if (value === undefined || value === null || value === '') return;
    search.set(key, String(value));
  });
  const qs = search.toString();
  return qs ? `?${qs}` : '';
}

export function getErrorMessage(error: unknown): string {
  if (error instanceof ApiError) return error.message;
  if (error instanceof Error && error.message) return error.message;
  return 'Đã có lỗi xảy ra, vui lòng thử lại.';
}

export const api = {
  get: <T>(path: string, config?: RequestConfig) => apiRequest<T>(path, { method: 'GET' }, config),
  post: <T>(path: string, body?: unknown, config?: RequestConfig) =>
    apiRequest<T>(
      path,
      { method: 'POST', body: body === undefined ? undefined : JSON.stringify(body) },
      config,
    ),
  put: <T>(path: string, body?: unknown) =>
    apiRequest<T>(path, { method: 'PUT', body: body === undefined ? undefined : JSON.stringify(body) }),
  del: <T>(path: string) => apiRequest<T>(path, { method: 'DELETE' }),
};
