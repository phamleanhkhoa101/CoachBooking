import type { ReactNode } from 'react';

type BadgeTone = 'success' | 'danger' | 'warning' | 'info' | 'neutral';

export function Badge({ tone = 'neutral', children }: { tone?: BadgeTone; children: ReactNode }) {
  return <span className={`badge badge-${tone}`}>{children}</span>;
}

export function StatusBadge({ active, trueLabel = 'Hoạt động', falseLabel = 'Ngừng' }: { active: boolean; trueLabel?: string; falseLabel?: string }) {
  return <Badge tone={active ? 'success' : 'danger'}>{active ? trueLabel : falseLabel}</Badge>;
}

export function PaidBadge({ paid }: { paid: boolean }) {
  return <Badge tone={paid ? 'success' : 'warning'}>{paid ? 'Đã thanh toán' : 'Chưa thanh toán'}</Badge>;
}
