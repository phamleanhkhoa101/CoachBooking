import { useCallback, useEffect, useState } from 'react';
import { api, buildQuery, getErrorMessage } from '../api/client';
import type { HoaDonDetail, HoaDonItem, VeItem } from '../api/types';
import { PaidBadge } from '../components/Badge';
import { DataTable, type DataTableColumn } from '../components/DataTable';
import { FormField } from '../components/FormField';
import { Modal } from '../components/Modal';
import { useToast } from '../components/Toast';
import { formatDateTime, formatVnd } from '../utils/format';

const PAYMENT_METHODS = ['Tiền mặt', 'Chuyển khoản', 'Thẻ ATM'];

const veColumns: DataTableColumn<VeItem>[] = [
  { key: 'maVe', label: 'Mã vé', width: '80px' },
  {
    key: 'tuyen',
    label: 'Tuyến',
    render: (row) => (
      <div className="cell-stack">
        <strong>
          {row.diemDi} → {row.diemDen}
        </strong>
        <span className="text-muted">Khởi hành {formatDateTime(row.ngayGioKhoiHanh)}</span>
      </div>
    ),
  },
  { key: 'choNgoi', label: 'Ghế', align: 'center', width: '70px' },
  { key: 'giaVe', label: 'Giá vé', align: 'right', render: (row) => formatVnd(row.giaVe) },
];

function detailToItem(detail: HoaDonDetail): HoaDonItem {
  return {
    maHd: detail.maHd,
    tenKhachHang: detail.tenKhachHang,
    tenNhanVien: detail.tenNhanVien,
    ngayLap: detail.ngayLap,
    tongTien: detail.tongTien,
    trangThai: detail.trangThai,
    soVe: detail.veXes?.length ?? 0,
    phuongThuc: detail.thanhToan?.phuongThuc ?? null,
  };
}

export function Invoices() {
  const { showToast } = useToast();
  const [tu, setTu] = useState('');
  const [den, setDen] = useState('');
  const [trangThai, setTrangThai] = useState('');
  const [rows, setRows] = useState<HoaDonItem[]>([]);
  const [loading, setLoading] = useState(true);

  const [detailOpen, setDetailOpen] = useState(false);
  const [detail, setDetail] = useState<HoaDonDetail | null>(null);
  const [detailLoading, setDetailLoading] = useState(false);
  const [detailId, setDetailId] = useState<number | null>(null);

  const [payTarget, setPayTarget] = useState<HoaDonItem | null>(null);
  const [payMethod, setPayMethod] = useState('Tiền mặt');
  const [busy, setBusy] = useState(false);

  const [cancelTarget, setCancelTarget] = useState<HoaDonItem | null>(null);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const data = await api.get<HoaDonItem[]>(`/hoa-don${buildQuery({ tu, den, trangThai })}`);
      setRows(Array.isArray(data) ? data : []);
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setLoading(false);
    }
  }, [tu, den, trangThai, showToast]);

  useEffect(() => {
    void load();
  }, [load]);

  const loadDetail = useCallback(
    async (maHd: number) => {
      setDetailLoading(true);
      try {
        const data = await api.get<HoaDonDetail>(`/hoa-don/${maHd}`);
        setDetail(data);
      } catch (error) {
        showToast(getErrorMessage(error), 'error');
      } finally {
        setDetailLoading(false);
      }
    },
    [showToast],
  );

  const openDetail = (maHd: number) => {
    setDetailId(maHd);
    setDetail(null);
    setDetailOpen(true);
    void loadDetail(maHd);
  };

  const handlePay = async () => {
    if (!payTarget) return;
    setBusy(true);
    try {
      await api.post(`/hoa-don/${payTarget.maHd}/thanh-toan`, { phuongThuc: payMethod });
      showToast(`Đã thanh toán hóa đơn #${payTarget.maHd}.`);
      setPayTarget(null);
      await load();
      if (detailOpen && detailId === payTarget.maHd) await loadDetail(payTarget.maHd);
    } catch (error) {
      showToast(getErrorMessage(error), 'error');
    } finally {
      setBusy(false);
    }
  };

  const handleCancel = async () => {
    if (!cancelTarget) return;
    setBusy(true);
    try {
      await api.post(`/hoa-don/${cancelTarget.maHd}/huy`);
      showToast(`Đã hủy hóa đơn #${cancelTarget.maHd}.`);
      setCancelTarget(null);
      if (detailOpen && detailId === cancelTarget.maHd) {
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

  const columns: DataTableColumn<HoaDonItem>[] = [
    { key: 'maHd', label: 'Mã HĐ', width: '90px', render: (row) => <strong>#{row.maHd}</strong> },
    {
      key: 'tenKhachHang',
      label: 'Khách hàng',
      render: (row) => (
        <div className="cell-stack">
          <strong>{row.tenKhachHang}</strong>
          <span className="text-muted">{row.tenNhanVien ? `NV: ${row.tenNhanVien}` : 'Khách tự đặt'}</span>
        </div>
      ),
    },
    { key: 'ngayLap', label: 'Ngày lập', render: (row) => formatDateTime(row.ngayLap) },
    { key: 'soVe', label: 'Số vé', align: 'right', width: '80px' },
    { key: 'tongTien', label: 'Tổng tiền', align: 'right', render: (row) => <strong>{formatVnd(row.tongTien)}</strong> },
    { key: 'phuongThuc', label: 'Thanh toán', render: (row) => row.phuongThuc ?? '—' },
    { key: 'trangThai', label: 'Trạng thái', render: (row) => <PaidBadge paid={row.trangThai} /> },
  ];

  const detailFooter = detail ? (
    <>
      <button type="button" className="btn btn-secondary" onClick={() => setDetailOpen(false)}>
        Đóng
      </button>
      {!detail.trangThai ? (
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
          <p className="page-subtitle">Tra cứu hóa đơn, xem chi tiết vé, thanh toán hoặc hủy hóa đơn chưa thanh toán.</p>
        </div>
        <div className="page-actions filter-inline">
          <div className="field field-compact">
            <label htmlFor="hd-tu">Từ ngày</label>
            <input id="hd-tu" type="date" value={tu} onChange={(event) => setTu(event.target.value)} />
          </div>
          <div className="field field-compact">
            <label htmlFor="hd-den">Đến ngày</label>
            <input id="hd-den" type="date" value={den} onChange={(event) => setDen(event.target.value)} />
          </div>
          <div className="field field-compact">
            <label htmlFor="hd-tt">Trạng thái</label>
            <select id="hd-tt" value={trangThai} onChange={(event) => setTrangThai(event.target.value)}>
              <option value="">Tất cả</option>
              <option value="true">Đã thanh toán</option>
              <option value="false">Chưa thanh toán</option>
            </select>
          </div>
          <button type="button" className="btn btn-primary" onClick={() => void load()} disabled={loading}>
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
        <DataTable
          columns={columns}
          rows={rows}
          rowKey={(row) => row.maHd}
          loading={loading}
          emptyText="Không có hóa đơn nào trong khoảng thời gian đã chọn."
          actions={(row) => (
            <>
              <button type="button" className="btn btn-ghost btn-sm" onClick={() => openDetail(row.maHd)}>
                Chi tiết
              </button>
              {!row.trangThai ? (
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
                  <button type="button" className="btn btn-danger-ghost btn-sm" onClick={() => setCancelTarget(row)}>
                    Hủy
                  </button>
                </>
              ) : null}
            </>
          )}
        />
      </div>

      <Modal
        open={detailOpen}
        onClose={() => setDetailOpen(false)}
        title={detail ? `Hóa đơn #${detail.maHd}` : 'Chi tiết hóa đơn'}
        subtitle={detail ? `Lập lúc ${formatDateTime(detail.ngayLap)}` : undefined}
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
                <strong>{detail.tenKhachHang}</strong>
                <span className="text-muted">{detail.sdtKhachHang ?? '—'}</span>
              </div>
              <div>
                <span className="text-muted">Nhân viên lập</span>
                <strong>{detail.tenNhanVien ?? 'Khách tự đặt'}</strong>
              </div>
              <div>
                <span className="text-muted">Tổng tiền</span>
                <strong className="text-primary">{formatVnd(detail.tongTien)}</strong>
              </div>
              <div>
                <span className="text-muted">Trạng thái</span>
                <PaidBadge paid={detail.trangThai} />
              </div>
            </div>

            <h3 className="section-title">Danh sách vé ({detail.veXes?.length ?? 0})</h3>
            <DataTable
              columns={veColumns}
              rows={detail.veXes ?? []}
              rowKey={(row) => row.maVe}
              emptyText="Hóa đơn chưa có vé."
            />

            {detail.thanhToan ? (
              <div className="info-box">
                <span className="info-box-title">Thông tin thanh toán</span>
                <div className="info-grid">
                  <div>
                    <span className="text-muted">Mã thanh toán</span>
                    <strong>#{detail.thanhToan.maTt}</strong>
                  </div>
                  <div>
                    <span className="text-muted">Số tiền</span>
                    <strong>{formatVnd(detail.thanhToan.tongTien)}</strong>
                  </div>
                  <div>
                    <span className="text-muted">Thời điểm</span>
                    <strong>{formatDateTime(detail.thanhToan.ngayTt)}</strong>
                  </div>
                  <div>
                    <span className="text-muted">Phương thức</span>
                    <strong>{detail.thanhToan.phuongThuc}</strong>
                  </div>
                </div>
              </div>
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
        subtitle={payTarget ? `Hóa đơn #${payTarget.maHd} · ${formatVnd(payTarget.tongTien)}` : undefined}
        footer={
          <>
            <button type="button" className="btn btn-secondary" onClick={() => setPayTarget(null)}>
              Hủy
            </button>
            <button type="button" className="btn btn-primary" onClick={() => void handlePay()} disabled={busy}>
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
            <button type="button" className="btn btn-secondary" onClick={() => setCancelTarget(null)}>
              Giữ lại
            </button>
            <button type="button" className="btn btn-danger" onClick={() => void handleCancel()} disabled={busy}>
              {busy ? 'Đang hủy…' : 'Hủy hóa đơn'}
            </button>
          </>
        }
      >
        <p>
          Hủy toàn bộ vé của hóa đơn <strong>#{cancelTarget?.maHd}</strong>? Chỉ hủy được khi hóa đơn chưa
          thanh toán và chuyến chưa khởi hành.
        </p>
      </Modal>
    </div>
  );
}
