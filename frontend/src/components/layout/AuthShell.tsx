import type { ReactNode } from 'react';
import { Link } from 'react-router-dom';
import { isAdminPortal } from '../../auth/portal';
import { BrandLogo } from '../common/BrandLogo';

export interface AuthShellProps {
  title: string;
  subtitle?: string;
  wide?: boolean;
  showBrandTitle?: boolean;
  showBackHome?: boolean;
  topSlot?: ReactNode;
  footer?: ReactNode;
  children: ReactNode;
}

/**
 * Shared minimal Twilight shell for Login, Register, ForgotPassword and OAuth2Callback.
 */
export function AuthShell({
  title,
  subtitle,
  wide = false,
  showBrandTitle = true,
  showBackHome = !isAdminPortal(),
  topSlot,
  footer,
  children,
}: AuthShellProps) {
  return (
    <div className="auth-page auth-page-single">
      <div className={`auth-shell-wrap ${wide ? 'auth-shell-wrap--wide' : ''}`}>
        {showBackHome ? (
          <Link to="/" className="auth-back-link">
            <svg
              width="16"
              height="16"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="2"
              strokeLinecap="round"
              strokeLinejoin="round"
              aria-hidden="true"
            >
              <line x1="19" y1="12" x2="5" y2="12" />
              <polyline points="12 19 5 12 12 5" />
            </svg>
            <span>Về trang chủ</span>
          </Link>
        ) : null}

        <div className={`auth-card ${wide ? 'auth-card-wide' : ''}`}>
          <div className="auth-brand-center">
            <BrandLogo size={54} className="auth-logo-badge" />
            {showBrandTitle ? <h2 className="auth-brand-title">CoachBooking</h2> : null}
          </div>

          <div className="auth-card-divider" />

          {topSlot}

          <div className="auth-heading-center">
            <h1>{title}</h1>
            {subtitle ? <p>{subtitle}</p> : null}
          </div>

          {children}

          {footer ? <div className="auth-card-footer">{footer}</div> : null}
        </div>
      </div>
    </div>
  );
}

