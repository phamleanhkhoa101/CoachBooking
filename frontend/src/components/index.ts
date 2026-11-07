// Re-export common UI components
export { Badge } from './common/Badge';
export { FormField } from './common/FormField';
export { Modal } from './common/Modal';
export { ToastProvider, useToast } from './common/Toast';

// Re-export layout & table
export { AppLayout } from './layout/AppLayout';
export { DataTable, type DataTableColumn } from './table/DataTable';
export { CrudPage } from './table/CrudPage';

// Re-export SeatMap for backward compatibility
export { SeatMap } from '../pages/customer/components/SeatMap';
