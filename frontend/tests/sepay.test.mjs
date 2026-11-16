import { test } from 'node:test';
import assert from 'node:assert/strict';
import { readFile } from 'node:fs/promises';
import ts from 'typescript';

const source = await readFile(new URL('../src/api/sepay.ts', import.meta.url), 'utf8');
const js = ts.transpileModule(source, { compilerOptions: { module: ts.ModuleKind.ESNext, target: ts.ScriptTarget.ES2020 } }).outputText
  .replace(/import \{ api \} from ['"]\.\/client['"];?/, 'const api = globalThis.__sepayApi;');
let serial = 0;
const load = () => import(`data:text/javascript;base64,${Buffer.from(js + `\n// ${serial++}`).toString('base64')}`);
const pending = () => ({ paymentStatus: 'PENDING', expiresAt: new Date(Date.now() + 600_000).toISOString() });

test('only pending, unexpired sessions can be paid and every terminal state has a label', async () => {
  const { canPaySepay, sepayLabel } = await load();
  assert.equal(canPaySepay(pending()), true);
  assert.equal(canPaySepay({ ...pending(), expiresAt: '2000-01-01T00:00:00Z' }), false);
  assert.equal(canPaySepay(null), false);
  for (const paymentStatus of ['PAID', 'CANCELLED', 'EXPIRED', 'REVIEW_REQUIRED']) {
    const session = { ...pending(), paymentStatus };
    assert.equal(canPaySepay(session), false);
    assert.ok(sepayLabel(session).length > 0);
  }
  assert.match(sepayLabel({ ...pending(), expiresAt: '2000-01-01T00:00:00Z' }), /hết/);
  assert.match(sepayLabel({ ...pending(), paymentStatus: 'REVIEW_REQUIRED' }), /đối soát/);
});

test('checkout posts ordered hidden fields as text to the sandbox and removes the form', async () => {
  let requested;
  const fields = [{ name: 'order_amount', value: '150000' }, { name: 'order_description', value: '<script>not HTML</script>' }, { name: 'signature', value: 'signed+base64=' }];
  globalThis.__sepayApi = { post: async path => {
    requested = path;
    return { checkoutUrl: 'https://pay-sandbox.sepay.vn/v1/checkout/init', fields };
  } };
  let added;
  let submitted;
  globalThis.HTMLFormElement = class {};
  globalThis.HTMLFormElement.prototype.submit = function () { submitted = this; };
  globalThis.document = {
    createElement: tag => tag === 'form' ? { children: [], appendChild(child) { this.children.push(child); }, remove() { this.removed = true; } } : {},
    body: { appendChild: form => { added = form; } },
  };
  const { openSepay } = await load();
  await openSepay('session-id');
  assert.equal(requested, '/payments/sepay/session-id/checkout');
  assert.equal(submitted, added);
  assert.equal(added.method, 'POST');
  assert.equal(added.action, 'https://pay-sandbox.sepay.vn/v1/checkout/init');
  assert.deepEqual(added.children.map(({ name, value }) => ({ name, value })), fields);
  assert.ok(added.children.every(field => field.type === 'hidden'));
  assert.equal(added.removed, true);
});

test('leaving the payment route discards a late checkout response before creating a form', async () => {
  let resolve;
  let active = true;
  globalThis.__sepayApi = { post: () => new Promise(done => { resolve = done; }) };
  globalThis.document = { createElement() { assert.fail('must not create a form after navigation'); } };
  const { openSepay } = await load();
  const opening = openSepay('id', () => active);
  active = false;
  resolve({ checkoutUrl: 'https://pay-sandbox.sepay.vn/v1/checkout/init', fields: [] });
  await opening;
});

test('unexpected payment destinations and API failures do not submit a form', async () => {
  globalThis.document = { createElement() { assert.fail('must not create a form'); } };
  globalThis.__sepayApi = { post: async () => ({ checkoutUrl: 'https://attacker.example/checkout', fields: [] }) };
  await assert.rejects((await load()).openSepay('id'), /không hợp lệ/);
  globalThis.__sepayApi = { post: async () => { throw new Error('Phiên đã hết hạn'); } };
  await assert.rejects((await load()).openSepay('id'), /hết hạn/);
});
