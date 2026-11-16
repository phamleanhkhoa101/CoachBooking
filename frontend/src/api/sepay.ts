import { api } from './client';
import type { SepayCheckout, SepayStatus } from './types';

export async function openSepay(sessionId: string, canSubmit: () => boolean = () => true): Promise<void> {
  const checkout = await api.post<SepayCheckout>(`/payments/sepay/${encodeURIComponent(sessionId)}/checkout`);
  if (!canSubmit()) return;
  if (checkout.checkoutUrl !== 'https://pay-sandbox.sepay.vn/v1/checkout/init') {
    throw new Error('Địa chỉ thanh toán không hợp lệ.');
  }
  const form = document.createElement('form');
  form.method = 'POST';
  form.action = checkout.checkoutUrl;
  form.hidden = true;
  for (const field of checkout.fields) {
    const input = document.createElement('input');
    input.type = 'hidden';
    input.name = field.name;
    input.value = field.value;
    form.appendChild(input);
  }
  document.body.appendChild(form);
  try {
    HTMLFormElement.prototype.submit.call(form);
  } finally {
    form.remove();
  }
}

export function sepayLabel(session?: SepayStatus | null): string {
  if (!session) return 'Chưa thanh toán';
  if (session.paymentStatus === 'PENDING' && Date.parse(session.expiresAt) <= Date.now()) {
    return 'Đã hết thời gian thanh toán';
  }
  return {
    PENDING: 'Chờ thanh toán SePay',
    PAID: 'Đã thanh toán',
    EXPIRED: 'Đã hết hạn',
    CANCELLED: 'Đã hủy',
    REVIEW_REQUIRED: 'Cần đối soát — vui lòng liên hệ nhà xe',
  }[session.paymentStatus];
}

export function canPaySepay(session?: SepayStatus | null): boolean {
  return session?.paymentStatus === 'PENDING' && Date.parse(session.expiresAt) > Date.now();
}
