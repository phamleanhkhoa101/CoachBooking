import type { ReactNode } from 'react';
import type { Role } from '../../api/types';

export type BadgeTone = 'success' | 'danger' | 'warning' | 'info' | 'neutral' | 'accent';

export interface BadgeProps {
  tone?: BadgeTone;
  children: ReactNode;
  className?: string;
  icon?: ReactNode;
}

export function Badge({ tone = 'neutral', children, className = '', icon }: BadgeProps) {
  return (
    <span className={`badge badge-${tone} ${className}`.trim()}>
      {icon ? <span className="badge-icon" aria-hidden="true">{icon}</span> : null}
      <span>{children}</span>
    </span>
  );
}

export function StatusBadge({
  active,
  trueLabel = 'Hoạt động',
  falseLabel = 'Ngừng',
}: {
  active: boolean;
  trueLabel?: string;
  falseLabel?: string;
}) {
  return <Badge tone={active ? 'success' : 'danger'}>{active ? trueLabel : falseLabel}</Badge>;
}

export function PaidBadge({ paid }: { paid: boolean }) {
  return <Badge tone={paid ? 'success' : 'warning'}>{paid ? 'Đã thanh toán' : 'Chưa thanh toán'}</Badge>;
}

export function RoleBadge({ role }: { role: Role | string }) {
  if (role === 'Admin') {
    return <Badge tone="accent">Quản trị viên</Badge>;
  }
  if (role === 'NhanVien') {
    return <Badge tone="info">Nhân viên</Badge>;
  }
  return <Badge tone="neutral">Khách hàng</Badge>;
}
