import { test } from 'node:test';
import assert from 'node:assert/strict';
import { readFile } from 'node:fs/promises';
import ts from 'typescript';

const source = await readFile(new URL('../src/api/sepay-flow.ts', import.meta.url), 'utf8');
const js = ts.transpileModule(source, { compilerOptions: { module: ts.ModuleKind.ESNext, target: ts.ScriptTarget.ES2020 } }).outputText;
const { createSepayFlow } = await import(`data:text/javascript;base64,${Buffer.from(js).toString('base64')}`);
const pending = { paymentSessionId: 'session-id', invoiceId: 20, paymentStatus: 'PENDING', confirmationMode: 'POLLING', totalAmount: 200000, expiresAt: '2099-01-01T00:00:00Z' };
const paid = { ...pending, paymentStatus: 'PAID' };
const settle = async () => { for (let i = 0; i < 20; i++) await Promise.resolve(); };
const deferred = () => { let resolve; const promise = new Promise(done => { resolve = done; }); return { promise, resolve }; };

function setup(t, overrides = {}) {
  const events = [];
  const flow = createSepayFlow({
    startCheckout: false,
    read: async () => { events.push('read'); return pending; },
    sync: async () => { events.push('sync'); return pending; },
    open: async canSubmit => { if (canSubmit()) events.push('submit'); },
    consumeCheckoutIntent: () => events.push('consume-intent'),
    showTicket: id => events.push(`ticket:${id}`),
    errorMessage: error => error.message,
    ...overrides,
  });
  t.after(() => flow.pause());
  return { flow, events };
}

test('new payment opens QR once without sync and keeps the opening spinner', async t => {
  const { flow, events } = setup(t, { startCheckout: true });
  assert.equal(flow.getState().phase, 'opening');
  flow.start(); flow.start(); await settle();
  assert.deepEqual(events, ['read', 'consume-intent', 'submit']);
  assert.equal(flow.getState().phase, 'opening');
});

test('callback confirms and automatically redirects to the ticket exactly once', async t => {
  const { flow, events } = setup(t, { sync: async () => paid });
  flow.start(); await settle();
  assert.equal(flow.getState().phase, 'paid');
  assert.deepEqual(events, ['read', 'ticket:20']);
  flow.check(); flow.start(); await settle();
  assert.equal(events.filter(e => e === 'ticket:20').length, 1);
});

test('already-paid invoice redirects even with an old checkout intent', async t => {
  const { flow, events } = setup(t, { startCheckout: true, read: async () => paid });
  flow.start(); await settle();
  assert.deepEqual(events, ['ticket:20']);
});

test('pending return polls every three seconds and follows backend PAID without reopening QR', async t => {
  t.mock.timers.enable({ apis: ['setTimeout'] });
  let reads = 0;
  const { flow, events } = setup(t, { read: async () => ++reads === 1 ? pending : paid });
  flow.start(); await settle();
  assert.equal(flow.getState().phase, 'confirming');
  assert.equal(events.includes('submit'), false);
  t.mock.timers.tick(3000); await settle();
  assert.equal(flow.getState().phase, 'paid');
  assert.equal(reads, 2);
  assert.equal(events.filter(e => e === 'sync').length, 1);
  assert.equal(events.filter(e => e === 'ticket:20').length, 1);
});

test('IPN mode waits for database confirmation without calling sync', async t => {
  const { flow, events } = setup(t, { read: async () => ({ ...pending, confirmationMode: 'IPN' }) });
  flow.start(); await settle();
  assert.equal(flow.getState().phase, 'confirming');
  assert.deepEqual(events, []);
});

test('thirty-second wait exposes recovery but still detects a later payment', async t => {
  t.mock.timers.enable({ apis: ['setTimeout'] });
  let result = pending;
  const { flow, events } = setup(t, { read: async () => result });
  flow.start(); await settle();
  t.mock.timers.tick(30000); await settle();
  assert.equal(flow.getState().phase, 'delayed');
  result = paid;
  t.mock.timers.tick(3000); await settle();
  assert.equal(flow.getState().phase, 'paid');
  assert.equal(events.includes('submit'), false);
});

test('expired, cancelled and review sessions never open QR or show a paid ticket', async t => {
  for (const paymentStatus of ['EXPIRED', 'CANCELLED', 'REVIEW_REQUIRED']) {
    const { flow, events } = setup(t, { startCheckout: true, read: async () => ({ ...pending, paymentStatus }) });
    flow.start(); await settle();
    assert.equal(flow.getState().phase, 'terminal');
    assert.deepEqual(events, []);
    flow.pause();
  }
});

test('Strict Mode setup-cleanup-setup ignores stale reads and submits once', async t => {
  const read = deferred();
  const { flow, events } = setup(t, { startCheckout: true, read: () => read.promise });
  flow.start(); flow.pause(); flow.start();
  read.resolve(pending); await settle();
  assert.deepEqual(events, ['consume-intent', 'submit']);
});

test('leaving during checkout prevents a late response from submitting', async t => {
  const checkout = deferred();
  const { flow, events } = setup(t, {
    startCheckout: true,
    open: async canSubmit => { await checkout.promise; if (canSubmit()) events.push('submit'); },
  });
  flow.start(); await settle();
  flow.pause(); checkout.resolve(); await settle();
  assert.equal(events.includes('submit'), false);
});

test('opening failure requires explicit retry and does not loop automatically', async t => {
  t.mock.timers.enable({ apis: ['setTimeout'] });
  let attempts = 0;
  const { flow, events } = setup(t, { startCheckout: true, open: async canSubmit => {
    attempts++;
    if (attempts === 1) throw new Error('Cannot open checkout');
    if (canSubmit()) events.push('submit');
  } });
  flow.start(); await settle();
  assert.equal(flow.getState().phase, 'error');
  t.mock.timers.tick(6000); await settle();
  assert.equal(attempts, 1);
  flow.retryCheckout(); flow.retryCheckout(); await settle();
  assert.equal(attempts, 2);
  assert.equal(events.filter(e => e === 'submit').length, 1);
});

test('watchdog invalidates a hung checkout before retrying', async t => {
  t.mock.timers.enable({ apis: ['setTimeout'] });
  const checkout = deferred();
  let attempts = 0;
  const { flow, events } = setup(t, { startCheckout: true, open: async canSubmit => {
    attempts++;
    if (attempts === 1) await checkout.promise;
    if (canSubmit()) events.push('submit');
  } });
  flow.start(); await settle();
  t.mock.timers.tick(10000); await settle();
  assert.equal(flow.getState().phase, 'error');
  assert.equal(flow.getState().checking, false);
  flow.retryCheckout(); await settle();
  checkout.resolve(); await settle();
  assert.equal(attempts, 2);
  assert.equal(events.filter(e => e === 'submit').length, 1);
});

test('network failure never fabricates success and later confirmation recovers', async t => {
  t.mock.timers.enable({ apis: ['setTimeout'] });
  let result = pending;
  const { flow, events } = setup(t, { read: async () => result, sync: async () => { throw new Error('Offline'); } });
  flow.start(); await settle();
  assert.equal(flow.getState().phase, 'error');
  assert.equal(flow.getState().error, 'Offline');
  assert.equal(events.some(e => e.startsWith('ticket:')), false);
  result = paid;
  t.mock.timers.tick(3000); await settle();
  assert.equal(events.filter(e => e === 'ticket:20').length, 1);
});

test('pause cancels timers and suppresses updates after navigation', async t => {
  t.mock.timers.enable({ apis: ['setTimeout'] });
  const { flow, events } = setup(t);
  flow.start(); await settle();
  flow.pause();
  const count = events.length;
  t.mock.timers.tick(60000); await settle();
  assert.equal(events.length, count);
});
