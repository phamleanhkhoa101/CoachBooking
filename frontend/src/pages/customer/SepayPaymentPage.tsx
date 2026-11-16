import { useEffect, useMemo, useState } from 'react';
import { Link, useLocation, useNavigate, useParams } from 'react-router-dom';
import { api, getErrorMessage } from '../../api/client';
import { canPaySepay, openSepay, sepayLabel } from '../../api/sepay';
import { createSepayFlow } from '../../api/sepay-flow';
import type { SepayStatus } from '../../api/types';
import { Button } from '../../components/common/Button';
import { Spinner } from '../../components/common/LoadingState';
import './SepayPaymentPage.css';

export function SepayPaymentPage() {
  const { sessionId = '' } = useParams<{ sessionId: string }>();
  const location = useLocation();
  const navigate = useNavigate();
  const startCheckout = (location.state as { startCheckout?: boolean } | null)?.startCheckout === true;
  const flow = useMemo(() => createSepayFlow({
    startCheckout,
    read: () => api.get<SepayStatus>(`/payments/sepay/${encodeURIComponent(sessionId)}`),
    sync: () => api.post<SepayStatus>(`/payments/sepay/${encodeURIComponent(sessionId)}/sync`),
    open: canSubmit => openSepay(sessionId, canSubmit),
    consumeCheckoutIntent: () => {
      // Preserve React Router's history key/index while clearing the one-use intent.
      const historyState = window.history.state;
      if (historyState?.usr?.startCheckout) {
        window.history.replaceState({ ...historyState, usr: { ...historyState.usr, startCheckout: false } }, '');
      }
    },
    showTicket: invoiceId => navigate(`/ve-dien-tu/${invoiceId}`, { replace: true }),
    errorMessage: getErrorMessage,
  }), [sessionId, location.key, navigate]);
  const [state, setState] = useState(flow.getState);

  useEffect(() => {
    const unsubscribe = flow.subscribe(setState);
    flow.start();
    return () => { unsubscribe(); flow.pause(); };
  }, [flow]);

  const { phase, session, error, checking } = state;
  const waiting = phase === 'opening' || phase === 'confirming' || phase === 'paid';
  const pending = session?.paymentStatus === 'PENDING';
  const title = {
    opening: 'Đang mở trang thanh toán…',
    confirming: 'Đang xác nhận thanh toán…',
    paid: 'Đang mở vé điện tử…',
    delayed: 'Thanh toán chưa được xác nhận',
    error: 'Chưa thể hoàn tất thanh toán',
    terminal: session ? sepayLabel(session) : 'Không tìm thấy thanh toán',
  }[phase];

  return (
    <main className="payment-transition">
      <section className="payment-transition-content" aria-busy={waiting || checking}>
        {(waiting || pending) && <Spinner size="lg" className="payment-transition-spinner" label={title} />}
        <h1 aria-live="polite">{title}</h1>
        {waiting ? (
          <p>{phase === 'opening' ? 'Vui lòng chờ trong khi chúng tôi mở mã QR của bạn.' : 'Vui lòng giữ trang này trong khi chúng tôi kiểm tra giao dịch.'}</p>
        ) : <>
          {error && <p className="payment-transition-error" role="alert">{error}</p>}
          {pending && <p>Nếu bạn đã thanh toán, vui lòng không chuyển thêm tiền. Chúng tôi vẫn đang kiểm tra kết quả.</p>}
          {session?.syncError && <p>Chưa kết nối được để xác nhận giao dịch. Vui lòng kiểm tra lại sau ít phút.</p>}
          {['EXPIRED', 'CANCELLED'].includes(session?.paymentStatus ?? '') && <p>Ghế đã được nhả. Bạn có thể chọn chuyến và đặt vé mới.</p>}
          {session?.paymentStatus === 'REVIEW_REQUIRED' && <p>Vui lòng liên hệ nhà xe và cung cấp mã hóa đơn #{session.invoiceId}. Không thanh toán thêm cho đơn này.</p>}
          <div className="payment-transition-actions">
            {(pending || !session) && <Button onClick={() => flow.check()} disabled={checking}>Kiểm tra lại</Button>}
            {canPaySepay(session) && !session?.syncError && <Button variant="secondary" onClick={() => flow.retryCheckout()} disabled={checking}>Tiếp tục thanh toán</Button>}
            <Link className="btn btn-secondary" to="/ve-cua-toi">Vé của tôi</Link>
            {['EXPIRED', 'CANCELLED'].includes(session?.paymentStatus ?? '') && <Link className="btn btn-primary" to="/tim-chuyen">Tìm chuyến xe</Link>}
          </div>
        </>}
        <p className="payment-transition-sandbox">Thanh toán thử nghiệm · Không sử dụng tiền thật</p>
      </section>
    </main>
  );
}
