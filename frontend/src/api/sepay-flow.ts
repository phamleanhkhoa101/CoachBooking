import type { SepayStatus } from './types';

export type PaymentPhase = 'opening' | 'confirming' | 'delayed' | 'paid' | 'terminal' | 'error';
export interface PaymentFlowState {
  phase: PaymentPhase;
  session: SepayStatus | null;
  error: string;
  checking: boolean;
}
interface PaymentFlowOptions {
  startCheckout: boolean;
  read: () => Promise<SepayStatus>;
  sync: () => Promise<SepayStatus>;
  open: (canSubmit: () => boolean) => Promise<void>;
  consumeCheckoutIntent: () => void;
  showTicket: (invoiceId: number) => void;
  errorMessage: (error: unknown) => string;
}

/** Owns the async lifecycle so rendering cannot submit a payment form or redirect twice. */
export function createSepayFlow(options: PaymentFlowOptions) {
  let state: PaymentFlowState = {
    phase: options.startCheckout ? 'opening' : 'confirming',
    session: null,
    error: '',
    checking: false,
  };
  const listeners = new Set<(value: PaymentFlowState) => void>();
  let generation = 0;
  let active = false;
  let busy = false;
  let intentConsumed = false;
  let ticketShown = false;
  let pollTimer: ReturnType<typeof setTimeout> | undefined;
  let waitTimer: ReturnType<typeof setTimeout> | undefined;

  const current = (token: number) => active && token === generation;
  const emit = (change: Partial<PaymentFlowState>) => {
    state = { ...state, ...change };
    for (const listener of listeners) listener(state);
  };
  const clearTimers = () => {
    clearTimeout(pollTimer);
    clearTimeout(waitTimer);
    pollTimer = undefined;
    waitTimer = undefined;
  };
  const accept = (session: SepayStatus): boolean => {
    emit({ session });
    if (session.paymentStatus === 'PAID') {
      clearTimers();
      emit({ phase: 'paid', checking: false, error: '' });
      if (!ticketShown) {
        ticketShown = true;
        options.showTicket(session.invoiceId);
      }
      return false;
    }
    if (session.paymentStatus !== 'PENDING') {
      clearTimers();
      emit({ phase: 'terminal', checking: false, error: '' });
      return false;
    }
    return true;
  };
  const beginWait = (token: number) => {
    if (state.phase !== 'delayed') emit({ phase: 'confirming' });
    if (waitTimer === undefined && state.phase !== 'delayed') {
      waitTimer = setTimeout(() => {
        waitTimer = undefined;
        if (current(token) && state.session?.paymentStatus === 'PENDING') emit({ phase: 'delayed' });
      }, 30_000);
    }
  };
  const openCheckout = async (token: number) => {
    clearTimers();
    intentConsumed = true;
    options.consumeCheckoutIntent();
    emit({ phase: 'opening', error: '', checking: true });
    waitTimer = setTimeout(() => {
      waitTimer = undefined;
      if (current(token)) {
        generation++; // A late checkout response must not submit after the user retries.
        busy = false;
        emit({ phase: 'error', checking: false, error: 'Trang thanh toán chưa mở được. Vui lòng thử lại.' });
      }
    }, 10_000);
    try {
      await options.open(() => current(token));
    } catch (error) {
      if (current(token)) {
        clearTimers();
        emit({ phase: 'error', checking: false, error: options.errorMessage(error) });
      }
    }
  };

  const load = async (token: number, action: 'initial' | 'read' | 'sync' | 'open') => {
    if (!current(token) || busy || ticketShown) return;
    busy = true;
    let opening = false;
    emit({ checking: true });
    try {
      let data = await options.read();
      if (!current(token) || !accept(data)) return;
      const shouldOpen = action === 'open' || (action === 'initial' && options.startCheckout && !intentConsumed);
      if (shouldOpen) {
        opening = true;
        await openCheckout(token);
        return;
      }
      beginWait(token);
      if ((action === 'initial' || action === 'sync') && data.confirmationMode === 'POLLING') {
        data = await options.sync();
        if (!current(token) || !accept(data)) return;
      }
      emit({ error: '' });
      if (data.syncError) {
        clearTimeout(waitTimer);
        waitTimer = undefined;
        emit({ phase: 'delayed' });
      }
    } catch (error) {
      if (current(token)) emit({ phase: 'error', error: options.errorMessage(error) });
    } finally {
      if (current(token)) {
        busy = false;
        if (state.phase !== 'opening') {
          emit({ checking: false });
          if (!opening && state.session?.paymentStatus === 'PENDING') {
            pollTimer = setTimeout(() => { pollTimer = undefined; void load(token, 'read'); }, 3000);
          }
        }
      }
    }
  };

  return {
    getState: () => state,
    subscribe(listener: (value: PaymentFlowState) => void) {
      listeners.add(listener);
      listener(state);
      return () => { listeners.delete(listener); };
    },
    start() {
      if (active) return;
      active = true;
      generation++;
      void load(generation, 'initial');
    },
    pause() {
      active = false;
      generation++;
      busy = false;
      clearTimers();
    },
    check() {
      clearTimeout(pollTimer);
      pollTimer = undefined;
      void load(generation, 'sync');
    },
    retryCheckout() {
      clearTimeout(pollTimer);
      pollTimer = undefined;
      void load(generation, 'open');
    },
  };
}
