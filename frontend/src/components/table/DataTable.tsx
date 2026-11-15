import { useEffect, useMemo, useState, type ReactNode } from 'react';
import { EmptyState } from '../common/EmptyState';
import { LoadingState } from '../common/LoadingState';

export interface DataTableColumn<T> {
  key: string;
  label: string;
  width?: string;
  align?: 'left' | 'right' | 'center';
  sortable?: boolean;
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
  sortable?: boolean;
  pagination?: boolean;
  defaultPageSize?: number;
  pageSizeOptions?: number[];
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
  sortable = true,
  pagination = true,
  defaultPageSize = 10,
  pageSizeOptions = [10, 25, 50, 100],
}: DataTableProps<T>) {
  const columnCount = columns.length + (actions ? 1 : 0);

  // Sorting state
  const [sortKey, setSortKey] = useState<string | null>(null);
  const [sortDirection, setSortDirection] = useState<'asc' | 'desc' | null>(null);

  // Pagination state
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(defaultPageSize);

  // Reset page when rows length changes or filter changes
  useEffect(() => {
    setCurrentPage(1);
  }, [rows.length]);

  const handleHeaderClick = (col: DataTableColumn<T>) => {
    if (!sortable || col.sortable === false) return;
    if (sortKey !== col.key) {
      setSortKey(col.key);
      setSortDirection('asc');
    } else if (sortDirection === 'asc') {
      setSortDirection('desc');
    } else {
      setSortKey(null);
      setSortDirection(null);
    }
  };

  // Sort rows
  const sortedRows = useMemo(() => {
    if (!sortKey || !sortDirection) return rows;
    return [...rows].sort((a, b) => {
      const valA = (a as unknown as Record<string, unknown>)[sortKey];
      const valB = (b as unknown as Record<string, unknown>)[sortKey];

      if (valA === valB) return 0;
      if (valA === null || valA === undefined || valA === '') return 1;
      if (valB === null || valB === undefined || valB === '') return -1;

      let comparison = 0;
      if (typeof valA === 'number' && typeof valB === 'number') {
        comparison = valA - valB;
      } else if (typeof valA === 'boolean' && typeof valB === 'boolean') {
        comparison = valA === valB ? 0 : valA ? 1 : -1;
      } else {
        comparison = String(valA).localeCompare(String(valB), 'vi', { numeric: true });
      }

      return sortDirection === 'asc' ? comparison : -comparison;
    });
  }, [rows, sortKey, sortDirection]);

  // Paginate rows
  const totalItems = sortedRows.length;
  const totalPages = Math.max(1, Math.ceil(totalItems / pageSize));

  // Ensure current page does not exceed totalPages
  const activePage = Math.min(currentPage, totalPages);

  const displayedRows = useMemo(() => {
    if (!pagination) return sortedRows;
    const start = (activePage - 1) * pageSize;
    return sortedRows.slice(start, start + pageSize);
  }, [sortedRows, pagination, activePage, pageSize]);

  return (
    <div className="datatable-container">
      <div className="table-wrap">
        <table className="table">
          <thead>
            <tr>
              {columns.map((col) => {
                const isColSortable = sortable && col.sortable !== false;
                const isSorted = sortKey === col.key;
                return (
                  <th
                    key={col.key}
                    style={col.width ? { width: col.width } : undefined}
                    className={[
                      col.align ? `align-${col.align}` : '',
                      isColSortable ? 'th-sortable' : '',
                    ]
                      .filter(Boolean)
                      .join(' ')}
                    onClick={() => handleHeaderClick(col)}
                    title={isColSortable ? `Sắp xếp theo ${col.label}` : undefined}
                  >
                    <div className="th-sort-inner">
                      <span>{col.label}</span>
                      {isColSortable && (
                        <span
                          className={`sort-icon ${isSorted ? 'active' : 'inactive'}`}
                          aria-hidden="true"
                        >
                          {isSorted ? (sortDirection === 'asc' ? '▲' : '▼') : '⇅'}
                        </span>
                      )}
                    </div>
                  </th>
                );
              })}
              {actions ? <th className="align-right">{actionsLabel}</th> : null}
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td colSpan={columnCount} className="table-state">
                  <LoadingState message="Đang tải dữ liệu…" />
                </td>
              </tr>
            ) : displayedRows.length === 0 ? (
              <tr>
                <td colSpan={columnCount} className="table-state">
                  <EmptyState title={emptyText} />
                </td>
              </tr>
            ) : (
              displayedRows.map((row, index) => {
                const globalIndex = (activePage - 1) * pageSize + index;
                const key = rowKey(row, globalIndex);
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
                        {col.render ? col.render(row, globalIndex) : renderCell(row, col.key)}
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

      {/* Pagination Footer Bar */}
      {pagination && !loading && totalItems > 0 && (
        <div className="table-pagination">
          <div className="pagination-info">
            <span>
              Hiển thị{' '}
              <strong>{(activePage - 1) * pageSize + 1}</strong> –{' '}
              <strong>{Math.min(activePage * pageSize, totalItems)}</strong> trong{' '}
              <strong>{totalItems}</strong> bản ghi
            </span>
            <select
              className="pagination-size-select"
              value={pageSize}
              onChange={(e) => {
                setPageSize(Number(e.target.value));
                setCurrentPage(1);
              }}
              aria-label="Số bản ghi trên mỗi trang"
            >
              {pageSizeOptions.map((opt) => (
                <option key={opt} value={opt}>
                  {opt} / trang
                </option>
              ))}
            </select>
          </div>

          <div className="pagination-controls">
            <button
              type="button"
              className="pagination-btn"
              disabled={activePage <= 1}
              onClick={() => setCurrentPage(1)}
              title="Trang đầu"
              aria-label="Trang đầu"
            >
              «
            </button>
            <button
              type="button"
              className="pagination-btn"
              disabled={activePage <= 1}
              onClick={() => setCurrentPage((p) => Math.max(1, p - 1))}
              title="Trang trước"
              aria-label="Trang trước"
            >
              ‹
            </button>

            {/* Page number buttons */}
            {generatePageNumbers(activePage, totalPages).map((p, idx) =>
              p === -1 ? (
                <span key={`ellipsis-${idx}`} className="pagination-ellipsis">
                  …
                </span>
              ) : (
                <button
                  key={p}
                  type="button"
                  className={`pagination-btn ${p === activePage ? 'active' : ''}`}
                  onClick={() => setCurrentPage(p)}
                >
                  {p}
                </button>
              ),
            )}

            <button
              type="button"
              className="pagination-btn"
              disabled={activePage >= totalPages}
              onClick={() => setCurrentPage((p) => Math.min(totalPages, p + 1))}
              title="Trang tiếp"
              aria-label="Trang tiếp"
            >
              ›
            </button>
            <button
              type="button"
              className="pagination-btn"
              disabled={activePage >= totalPages}
              onClick={() => setCurrentPage(totalPages)}
              title="Trang cuối"
              aria-label="Trang cuối"
            >
              »
            </button>
          </div>
        </div>
      )}
    </div>
  );
}

function renderCell<T>(row: T, key: string): ReactNode {
  const value = (row as unknown as Record<string, unknown>)[key];
  if (value === null || value === undefined || value === '') return <span className="text-muted">—</span>;
  if (typeof value === 'boolean') return value ? 'Có' : 'Không';
  return String(value);
}

function generatePageNumbers(current: number, total: number): number[] {
  if (total <= 5) {
    return Array.from({ length: total }, (_, i) => i + 1);
  }
  const pages: number[] = [];
  pages.push(1);
  if (current > 3) pages.push(-1);

  const start = Math.max(2, current - 1);
  const end = Math.min(total - 1, current + 1);
  for (let i = start; i <= end; i++) {
    pages.push(i);
  }

  if (current < total - 2) pages.push(-1);
  pages.push(total);
  return pages;
}
