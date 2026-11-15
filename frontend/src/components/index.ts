// Re-export common UI components
export { Badge, StatusBadge, PaidBadge, RoleBadge, type BadgeProps, type BadgeTone } from './common/Badge';
export { BrandLogo, type BrandLogoProps } from './common/BrandLogo';
export { Button, type ButtonProps, type ButtonVariant, type ButtonSize } from './common/Button';
export { FormField, type FormFieldProps, type FormFieldType, type SelectOption } from './common/FormField';
export { Modal } from './common/Modal';
export { ToastProvider, useToast, type ToastKind, type ToastItem } from './common/Toast';
export { Spinner, Skeleton, LoadingState, type SpinnerProps, type SkeletonProps, type LoadingStateProps } from './common/LoadingState';
export { EmptyState, type EmptyStateProps } from './common/EmptyState';
export { ErrorBanner, type ErrorBannerProps } from './common/ErrorBanner';

// Re-export layout & table
export { AppLayout } from './layout/AppLayout';
export { CustomerLayout } from './layout/CustomerLayout';
export { CustomerNavbar } from './layout/CustomerNavbar';
export { DataTable, type DataTableColumn } from './table/DataTable';
export { CrudPage } from './table/CrudPage';

// Re-export SeatMap for backward compatibility
export { SeatMap } from '../pages/customer/components/SeatMap';
