/** Các hàm định dạng dùng chung (không phụ thuộc thư viện ngoài). */

const EMPTY = '—';

export function formatVnd(value: number | null | undefined): string {
  if (value === null || value === undefined || Number.isNaN(value)) return EMPTY;
  return `${new Intl.NumberFormat('vi-VN').format(value)} đ`;
}

export function formatNumber(value: number | null | undefined): string {
  if (value === null || value === undefined || Number.isNaN(value)) return '0';
  return new Intl.NumberFormat('vi-VN').format(value);
}

function parts(value: string): { d: string; m: string; y: string; time: string } {
  const [datePart, rest] = value.split('T');
  const [y, m, d] = (datePart ?? '').split('-');
  const time = (rest ?? '').split('.')[0] ?? '';
  return { d: d ?? '', m: m ?? '', y: y ?? '', time };
}

/** "2026-11-05" hoặc "2026-11-05T07:00:00" -> "05/11/2026" */
export function formatDate(value: string | null | undefined): string {
  if (!value) return EMPTY;
  const { d, m, y } = parts(value);
  if (!d || !m || !y) return value;
  return `${d}/${m}/${y}`;
}

/** "2026-11-05T07:00:00" -> "07:00" */
export function formatTime(value: string | null | undefined): string {
  if (!value) return EMPTY;
  const { time } = parts(value);
  if (!time) return EMPTY;
  const [h, min] = time.split(':');
  return `${h}:${min}`;
}

/** "2026-11-05T07:00:00" -> "05/11/2026 07:00" */
export function formatDateTime(value: string | null | undefined): string {
  if (!value) return EMPTY;
  const date = formatDate(value);
  const time = formatTime(value);
  if (time === EMPTY) return date;
  return `${date} ${time}`;
}

/** "07:00:00" -> "07:00" (dùng cho thời gian dự kiến của tuyến) */
export function formatHour(value: string | null | undefined): string {
  if (!value) return EMPTY;
  const [h, m] = value.split(':');
  return m ? `${h}:${m}` : value;
}

function pad(n: number): string {
  return String(n).padStart(2, '0');
}

/** Ngày hôm nay dạng YYYY-MM-DD (theo giờ máy người dùng). */
export function todayISO(): string {
  const now = new Date();
  return `${now.getFullYear()}-${pad(now.getMonth() + 1)}-${pad(now.getDate())}`;
}

export function daysAgoISO(days: number): string {
  const now = new Date();
  now.setDate(now.getDate() - days);
  return `${now.getFullYear()}-${pad(now.getMonth() + 1)}-${pad(now.getDate())}`;
}

/** Cắt chuỗi ISO về dạng phù hợp với <input type="date">. */
export function toDateInput(value: string | null | undefined): string {
  if (!value) return '';
  return value.slice(0, 10);
}

/** Cắt chuỗi ISO về dạng phù hợp với <input type="datetime-local">. */
export function toDateTimeInput(value: string | null | undefined): string {
  if (!value) return '';
  return value.slice(0, 16);
}

/** Boolean trong CSDL (trangThai, gioiTinh) hiển thị dạng nhãn. */
export function boolLabel(value: boolean | null | undefined, trueLabel = 'Hoạt động', falseLabel = 'Ngừng'): string {
  if (value === null || value === undefined) return EMPTY;
  return value ? trueLabel : falseLabel;
}
export const booleanLabel = boolLabel;

export function gioiTinhLabel(value: boolean | null | undefined): string {
  if (value === null || value === undefined) return EMPTY;
  return value ? 'Nam' : 'Nữ';
}
export const genderLabel = gioiTinhLabel;
