import type { CSSProperties } from 'react';

export interface SpinnerProps {
  size?: 'sm' | 'md' | 'lg';
  className?: string;
  label?: string;
}

export function Spinner({ size = 'md', className = '', label = 'Đang tải…' }: SpinnerProps) {
  const pixelSize = size === 'sm' ? 16 : size === 'lg' ? 32 : 22;
  return (
    <span className={`inline-spinner-wrap ${className}`.trim()} role="status" aria-label={label}>
      <svg
        className={`app-spinner spinner-${size}`}
        viewBox="0 0 24 24"
        width={pixelSize}
        height={pixelSize}
        fill="none"
        stroke="currentColor"
        strokeWidth="2.5"
        aria-hidden="true"
      >
        <circle cx="12" cy="12" r="10" stroke="currentColor" strokeOpacity="0.2" />
        <path d="M12 2a10 10 0 0 1 10 10" stroke="currentColor" strokeLinecap="round" />
      </svg>
      {label ? <span className="sr-only">{label}</span> : null}
    </span>
  );
}

export interface SkeletonProps {
  width?: string | number;
  height?: string | number;
  variant?: 'text' | 'rect' | 'circle';
  count?: number;
  className?: string;
  style?: CSSProperties;
}

export function Skeleton({
  width = '100%',
  height,
  variant = 'text',
  count = 1,
  className = '',
  style,
}: SkeletonProps) {
  const items = Array.from({ length: count });

  const getDefaultHeight = () => {
    if (height) return height;
    if (variant === 'circle') return width;
    if (variant === 'text') return '1rem';
    return '2rem';
  };

  const itemStyle: CSSProperties = {
    width,
    height: getDefaultHeight(),
    ...style,
  };

  return (
    <div className={`skeleton-group ${className}`.trim()} aria-hidden="true">
      {items.map((_, i) => (
        <div
          key={i}
          className={`skeleton skeleton-${variant}`}
          style={itemStyle}
        />
      ))}
    </div>
  );
}

export interface LoadingStateProps {
  message?: string;
  size?: 'sm' | 'md' | 'lg';
  className?: string;
}

export function LoadingState({
  message = 'Đang tải dữ liệu…',
  size = 'md',
  className = '',
}: LoadingStateProps) {
  return (
    <div className={`loading-state-box ${className}`.trim()} role="status" aria-live="polite">
      <Spinner size={size} label={message} />
      <span className="loading-state-text">{message}</span>
    </div>
  );
}
