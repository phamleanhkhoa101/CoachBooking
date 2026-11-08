import { useCallback, useEffect, useState } from 'react';
import { api, buildQuery, getErrorMessage } from '../../api/client';
import type { InvoiceDetail, InvoiceItem, TicketItem } from '../../api/types';
import { PaidBadge } from '../../components/common/Badge';
import { DataTable, type DataTableColumn } from '../../components/table/DataTable';
import { FormField } from '../../components/common/FormField';
import { Modal } from '../../components/common/Modal';
import { useToast } from '../../components/common/Toast';
import { formatDateTime, formatVnd } from '../../utils/format';

const PAYMENT_METHODS = ['Tiền mặt', 'Chuyển khoản', 'Thẻ ATM'];

const ticketColumns: DataTableColumn<TicketItem>[] = [
  {
    key: 'ticketId',
    label: 'Mã vé',
    width: '80px',
    render: (row) => row.ticketId ?? row.maVe,
  },
  {
    key: 'route',
    label: 'Tuyến',
    render: (row) => (
      <div className="cell-stack">
        <strong>
          {row.departure ?? row.diemDi} → {row.destination ?? row.diemDen}
        </strong>
        <span className="text-muted">
          Khởi hành {formatDateTime(row.departureTime ?? row.ngayGioKhoiHanh)}
        </span>
      </div>
    ),
  },
  {
    key: 'seatNumber',
    label: 'Ghế',
    align: 'center',
    width: '70px',
    render: (row) => row.seatNumber ?? row.choNgoi,
  },
  {
    key: 'price',
    label: 'Giá vé',
    align: 'right',
    render: (row) => formatVnd(row.price ?? row.giaVe),
  },
];

function detailToItem(detail: InvoiceDetail): InvoiceItem {
  return {
    invoiceId: detail.invoiceId ?? detail.maHd ?? 0,
    customerName: detail.customerName ?? detail.tenKhachHang ?? '',
    employeeName: detail.employeeName ?? detail.tenNhanVien ?? null,
    createdAt: detail.createdAt ?? detail.ngayLap ?? '',
    totalAmount: detail.totalAmount ?? detail.tongTien ?? 0,
    isPaid: Boolean(detail.isPaid ?? detail.trangThai),
    ticketCount: (detail.tickets ?? detail.veXes)?.length ?? 0,
    paymentMethod: detail.payment?.paymentMethod ?? detail.thanhToan?.phuongThuc ?? null,
  };
}

export function Invoices() {
  const { showToast } = useToast();
  const [fromDate, setFromDate] = useState('');
  const [toDate, setToDate] = useState('');
  const [paidStatus, setPaidStatus] = useState('');
  const [rows, setRows] = useState<InvoiceItem[]>([]);
  const [loading, setLoading] = useState(true);

  const [detailOpen, setDetailOpen] = useState(false);
  const [detail, setDetail] = useState<InvoiceDetail | null>(null);
  const [detailLoading, setDetailLoading] = useState(false);
  const [detailId, setDetailId] = useState<number | null>(null);

  const [payTarget, setPayTarget] = useState<InvoiceItem | null>(null);
  const [payMethod, setPayMethod] = useState('Tiền mặt');
  const [busy, setBusy] = useState(false);

  const [cancelTarget, setCancelTarget] = useState<InvoiceItem | null>(null);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const data = await api.get<InvoiceItem[]>(
        `/invoices${buildQuery({ fromDate, toDate, isPaid: paidStatus })}`,
      );
      setRows(Array.isArray(data) ? data : []);
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setLoading(false);
    }
  }, [fromDate, toDate, paidStatus, showToast]);

  useEffect(() => {
    void load();
  }, [load]);

  const loadDetail = useCallback(
    async (id: number) => {
      setDetailLoading(true);
      try {
        const data = await api.get<InvoiceDetail>(`/invoices/${id}`);
        setDetail(data);
      } catch (error) {
        showToast(getErrorMessage(error), 'error');
      } finally {
        setDetailLoading(false);
      }
    },
    [showToast],
  );

  const openDetail = (id: number) => {
    setDetailId(id);
    setDetail(null);
    setDetailOpen(true);
    void loadDetail(id);
  };

  const handlePay = async () => {
    if (!payTarget) return;
    const id = payTarget.invoiceId ?? payTarget.maHd;
    setBusy(true);
    try {
      await api.post(`/invoices/${id}/pay`, { paymentMethod: payMethod });
      showToast(`Đã thanh toán hóa đơn #${id}.`);
      setPayTarget(null);
      await load();
      if (detailOpen && detailId === id) await loadDetail(id);
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setBusy(false);
    }
  };

  const handleCancel = async () => {
    if (!cancelTarget) return;
    const id = cancelTarget.invoiceId ?? cancelTarget.maHd;
    setBusy(true);
    try {
      await api.post(`/invoices/${id}/cancel`);
      showToast(`Đã hủy hóa đơn #${id}.`);
      setCancelTarget(null);
      if (detailOpen && detailId === id) {
        setDetailOpen(false);
        setDetail(null);
      }
      await load();
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setBusy(false);
    }
  };

  const columns: DataTableColumn<InvoiceItem>[] = [
    {
      key: 'invoiceId',
      label: 'Mã HĐ',
      width: '90px',
      render: (row) => <strong>#{row.invoiceId ?? row.maHd}</strong>,
    },
    {
      key: 'customerName',
      label: 'Khách hàng',
      render: (row) => {
        const custName = row.customerName ?? row.tenKhachHang;
        const empName = row.employeeName ?? row.tenNhanVien;
        return (
          <div className="cell-stack">
            <strong>{custName}</strong>
            <span className="text-muted">{empName ? `NV: ${empName}` : 'Khách tự đặt'}</span>
          </div>
        );
      },
    },
    {
      key: 'createdAt',
      label: 'Ngày lập',
      render: (row) => formatDateTime(row.createdAt ?? row.ngayLap),
    },
    {
      key: 'ticketCount',
      label: 'Số vé',
      align: 'right',
      width: '80px',
      render: (row) => row.ticketCount ?? row.soVe ?? 0,
    },
    {
      key: 'totalAmount',
      label: 'Tổng tiền',
      align: 'right',
      render: (row) => <strong>{formatVnd(row.totalAmount ?? row.tongTien)}</strong>,
    },
    {
      key: 'paymentMethod',
      label: 'Thanh toán',
      render: (row) => row.paymentMethod ?? row.phuongThuc ?? '—',
    },
    {
      key: 'isPaid',
      label: 'Trạng thái',
      render: (row) => <PaidBadge paid={Boolean(row.isPaid ?? row.trangThai)} />,
    },
  ];

  const currentIsPaid = Boolean(detail?.isPaid ?? detail?.trangThai);
  const detailFooter = detail ? (
    <>
      <button type="button" className="btn btn-secondary" onClick={() => setDetailOpen(false)}>
        Đóng
      </button>
      {!currentIsPaid ? (
        <>
          <button
            type="button"
            className="btn btn-danger-ghost"
            onClick={() => setCancelTarget(detailToItem(detail))}
          >
            Hủy hóa đơn
          </button>
          <button
            type="button"
            className="btn btn-primary"
            onClick={() => {
              setPayMethod('Tiền mặt');
              setPayTarget(detailToItem(detail));
            }}
          >
            Thanh toán
          </button>
        </>
      ) : null}
    </>
  ) : null;

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>Hóa đơn</h1>
          <p className="page-subtitle">
            Tra cứu hóa đơn, xem chi tiết vé, thanh toán hoặc hủy hóa đơn chưa thanh toán.
          </p>
        </div>
        <div className="page-actions filter-inline">
          <div className="field field-compact">
            <label htmlFor="hd-tu">Từ ngày</label>
            <input
              id="hd-tu"
              type="date"
              value={fromDate}
              onChange={(event) => setFromDate(event.target.value)}
            />
          </div>
          <div className="field field-compact">
            <label htmlFor="hd-den">Đến ngày</label>
            <input
              id="hd-den"
              type="date"
              value={toDate}
              onChange={(event) => setToDate(event.target.value)}
            />
          </div>
          <div className="field field-compact">
            <label htmlFor="hd-tt">Trạng thái</label>
            <select
              id="hd-tt"
              value={paidStatus}
              onChange={(event) => setPaidStatus(event.target.value)}
            >
              <option value="">Tất cả</option>
              <option value="true">Đã thanh toán</option>
              <option value="false">Chưa thanh toán</option>
            </select>
          </div>
          <button
            type="button"
            className="btn btn-primary"
            onClick={() => void load()}
            disabled={loading}
          >
            Lọc
          </button>
        </div>
      </div>

      <div className="card">
        <div className="card-toolbar">
          <span className="text-muted">{rows.length} hóa đơn</span>
          <button type="button" className="btn btn-ghost btn-sm" onClick={() => void load()}>
            Làm mới
          </button>
        </div>
        <DataTable<InvoiceItem>
          columns={columns}
          rows={rows}
          rowKey={(row) => String(row.invoiceId ?? row.maHd)}
          loading={loading}
          emptyText="Không có hóa đơn nào trong khoảng thời gian đã chọn."
          actions={(row) => {
            const id = row.invoiceId ?? row.maHd;
            const paid = Boolean(row.isPaid ?? row.trangThai);
            return (
              <>
                <button
                  type="button"
                  className="btn btn-ghost btn-sm"
                  onClick={() => openDetail(id)}
                >
                  Chi tiết
                </button>
                {!paid ? (
                  <>
                    <button
                      type="button"
                      className="btn btn-secondary btn-sm"
                      onClick={() => {
                        setPayMethod('Tiền mặt');
                        setPayTarget(row);
                      }}
                    >
                      Thanh toán
                    </button>
                    <button
                      type="button"
                      className="btn btn-danger-ghost btn-sm"
                      onClick={() => setCancelTarget(row)}
                    >
                      Hủy
                    </button>
                  </>
                ) : null}
              </>
            );
          }}
        />
      </div>

      <Modal
        open={detailOpen}
        onClose={() => setDetailOpen(false)}
        title={detail ? `Hóa đơn #${detail.invoiceId ?? detail.maHd}` : 'Chi tiết hóa đơn'}
        subtitle={
          detail
            ? `Lập lúc ${formatDateTime(detail.createdAt ?? detail.ngayLap)}`
            : undefined
        }
        footer={detailFooter}
        wide
      >
        {detailLoading || !detail ? (
          <div className="empty-state">
            <span className="spinner" /> Đang tải chi tiết…
          </div>
        ) : (
          <>
            <div className="detail-grid">
              <div>
                <span className="text-muted">Khách hàng</span>
                <strong>{detail.customerName ?? detail.tenKhachHang}</strong>
                <span className="text-muted">{detail.customerPhone ?? detail.sdtKhachHang ?? '—'}</span>
              </div>
              <div>
                <span className="text-muted">Nhân viên lập</span>
                <strong>{detail.employeeName ?? detail.tenNhanVien ?? 'Khách tự đặt'}</strong>
              </div>
              <div>
                <span className="text-muted">Tổng tiền</span>
                <strong className="text-primary">
                  {formatVnd(detail.totalAmount ?? detail.tongTien)}
                </strong>
              </div>
              <div>
                <span className="text-muted">Trạng thái</span>
                <PaidBadge paid={Boolean(detail.isPaid ?? detail.trangThai)} />
              </div>
            </div>

            <h3 className="section-title">
              Danh sách vé ({(detail.tickets ?? detail.veXes)?.length ?? 0})
            </h3>
            <DataTable<TicketItem>
              columns={ticketColumns}
              rows={detail.tickets ?? detail.veXes ?? []}
              rowKey={(row) => String(row.ticketId ?? row.maVe)}
              emptyText="Hóa đơn chưa có vé."
            />

            {detail.payment || detail.thanhToan ? (
              (() => {
                const pId = detail.payment?.paymentId ?? detail.thanhToan?.maTt;
                const pAmount = detail.payment?.totalAmount ?? detail.thanhToan?.tongTien;
                const pDate = detail.payment?.paymentDate ?? detail.thanhToan?.ngayTt;
                const pMethod = detail.payment?.paymentMethod ?? detail.thanhToan?.phuongThuc;
                return (
                  <div className="info-box">
                    <span className="info-box-title">Thông tin thanh toán</span>
                    <div className="info-grid">
                      <div>
                        <span className="text-muted">Mã thanh toán</span>
                        <strong>#{pId}</strong>
                      </div>
                      <div>
                        <span className="text-muted">Số tiền</span>
                        <strong>{formatVnd(pAmount)}</strong>
                      </div>
                      <div>
                        <span className="text-muted">Thời điểm</span>
                        <strong>{formatDateTime(pDate)}</strong>
                      </div>
                      <div>
                        <span className="text-muted">Phương thức</span>
                        <strong>{pMethod}</strong>
                      </div>
                    </div>
                  </div>
                );
              })()
            ) : (
              <div className="alert alert-warning">Hóa đơn chưa được thanh toán.</div>
            )}
          </>
        )}
      </Modal>

      <Modal
        open={Boolean(payTarget)}
        onClose={() => setPayTarget(null)}
        title="Thanh toán hóa đơn"
        subtitle={
          payTarget
            ? `Hóa đơn #${payTarget.invoiceId ?? payTarget.maHd} · ${formatVnd(
                payTarget.totalAmount ?? payTarget.tongTien,
              )}`
            : undefined
        }
        footer={
          <>
            <button type="button" className="btn btn-secondary" onClick={() => setPayTarget(null)}>
              Hủy
            </button>
            <button
              type="button"
              className="btn btn-primary"
              onClick={() => void handlePay()}
              disabled={busy}
            >
              {busy ? 'Đang xử lý…' : 'Xác nhận thanh toán'}
            </button>
          </>
        }
      >
        <FormField
          label="Phương thức thanh toán"
          type="select"
          value={payMethod}
          onChange={setPayMethod}
          options={PAYMENT_METHODS.map((value) => ({ value, label: value }))}
        />
      </Modal>

      <Modal
        open={Boolean(cancelTarget)}
        onClose={() => setCancelTarget(null)}
        title="Hủy hóa đơn"
        footer={
          <>
            <button
              type="button"
              className="btn btn-secondary"
              onClick={() => setCancelTarget(null)}
            >
              Giữ lại
            </button>
            <button
              type="button"
              className="btn btn-danger"
              onClick={() => void handleCancel()}
              disabled={busy}
            >
              {busy ? 'Đang hủy…' : 'Hủy hóa đơn'}
            </button>
          </>
        }
      >
        <p>
          Hủy toàn bộ vé của hóa đơn <strong>#{cancelTarget?.invoiceId ?? cancelTarget?.maHd}</strong>?
          Chỉ hủy được khi hóa đơn chưa thanh toán và chuyến chưa khởi hành.
        </p>
      </Modal>
    </div>
  );
}
