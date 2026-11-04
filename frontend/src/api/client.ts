import type { ApiErrorBody } from './types';

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

export function getToken(): string | null {
  return localStorage.getItem(TOKEN_KEY);
}

export function setToken(token: string | null): void {
  if (token) {
    localStorage.setItem(TOKEN_KEY, token);
  } else {
    localStorage.removeItem(TOKEN_KEY);
  }
}

function handleUnauthorized(): void {
  setToken(null);
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
    response = await fetch(`/api${path}`, { ...options, headers });
  } catch {
    throw new ApiError(0, 'Không kết nối được tới máy chủ. Vui lòng kiểm tra backend đã chạy chưa.');
  }

  if (response.status === 401 && redirectOn401) {
    handleUnauthorized();
    throw new ApiError(401, 'Phiên đăng nhập đã hết hạn, vui lòng đăng nhập lại.');
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
