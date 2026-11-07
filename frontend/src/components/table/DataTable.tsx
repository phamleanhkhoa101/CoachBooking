import type { ReactNode } from 'react';

export interface DataTableColumn<T> {
  key: string;
  label: string;
  width?: string;
  align?: 'left' | 'right' | 'center';
  render?: (row: T, index: number) => ReactNode;
}

interface DataTableProps<T> {
  columns: DataTableColumn<T>[];
  rows: T[];
  rowKey: (row: T, index: number) => string | number;
  loading?: boolean;
  emptyText?: string;
  actions?: (row: T) => ReactNode;
  actionsLabel?: string;
  onRowClick?: (row: T) => void;
  selectedKey?: string | number | null;
}

export function DataTable<T>({
  columns,
  rows,
  rowKey,
  loading = false,
  emptyText = 'Không có dữ liệu.',
  actions,
  actionsLabel = 'Thao tác',
  onRowClick,
  selectedKey = null,
}: DataTableProps<T>) {
  const columnCount = columns.length + (actions ? 1 : 0);

  return (
    <div className="table-wrap">
      <table className="table">
        <thead>
          <tr>
            {columns.map((col) => (
              <th key={col.key} style={col.width ? { width: col.width } : undefined} className={col.align ? `align-${col.align}` : undefined}>
                {col.label}
              </th>
            ))}
            {actions ? <th className="align-right">{actionsLabel}</th> : null}
          </tr>
        </thead>
        <tbody>
          {loading ? (
            <tr>
              <td colSpan={columnCount} className="table-state">
                <span className="spinner" /> Đang tải dữ liệu…
              </td>
            </tr>
          ) : rows.length === 0 ? (
            <tr>
              <td colSpan={columnCount} className="table-state">
                {emptyText}
              </td>
            </tr>
          ) : (
            rows.map((row, index) => {
              const key = rowKey(row, index);
              const isSelected = selectedKey !== null && selectedKey === key;
              return (
                <tr
                  key={key}
                  className={[onRowClick ? 'row-clickable' : '', isSelected ? 'row-selected' : '']
                    .filter(Boolean)
                    .join(' ')}
                  onClick={onRowClick ? () => onRowClick(row) : undefined}
                >
                  {columns.map((col) => (
                    <td key={col.key} className={col.align ? `align-${col.align}` : undefined}>
                      {col.render ? col.render(row, index) : renderCell(row, col.key)}
                    </td>
                  ))}
                  {actions ? (
                    <td className="align-right" onClick={(event) => event.stopPropagation()}>
                      <div className="row-actions">{actions(row)}</div>
                    </td>
                  ) : null}
                </tr>
              );
            })
          )}
        </tbody>
      </table>
    </div>
  );
}

function renderCell<T>(row: T, key: string): ReactNode {
  const value = (row as unknown as Record<string, unknown>)[key];
  if (value === null || value === undefined || value === '') return <span className="text-muted">—</span>;
  if (typeof value === 'boolean') return value ? 'Có' : 'Không';
  return String(value);
}
