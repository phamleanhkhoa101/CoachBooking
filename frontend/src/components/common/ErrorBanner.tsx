export interface ErrorBannerProps {
  title?: string;
  message: string;
  onRetry?: () => void;
  onDismiss?: () => void;
  className?: string;
}

export function ErrorBanner({
  title = 'Có lỗi xảy ra',
  message,
  onRetry,
  onDismiss,
  className = '',
}: ErrorBannerProps) {
  return (
    <div className={`error-banner ${className}`.trim()} role="alert" aria-live="assertive">
      <div className="error-banner-icon" aria-hidden="true">
        <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
          <circle cx="12" cy="12" r="10" />
          <line x1="12" y1="8" x2="12" y2="12" />
          <line x1="12" y1="16" x2="12.01" y2="16" />
        </svg>
      </div>
      <div className="error-banner-body">
        {title ? <strong className="error-banner-title">{title}</strong> : null}
        <p className="error-banner-message">{message}</p>
      </div>
      <div className="error-banner-actions">
        {onRetry ? (
          <button type="button" className="btn btn-sm btn-danger" onClick={onRetry}>
            Thử lại
          </button>
        ) : null}
        {onDismiss ? (
          <button
            type="button"
            className="icon-btn error-banner-close"
            onClick={onDismiss}
            aria-label="Đóng thông báo"
          >
            ×
          </button>
        ) : null}
      </div>
    </div>
  );
}
