import { test } from 'node:test';
import assert from 'node:assert/strict';
import { readFile } from 'node:fs/promises';
import vm from 'node:vm';

const script = await readFile(new URL('../public/sepay-local-return.js', import.meta.url), 'utf8');
const id = '1d5ca891-63ce-4409-9bd3-ac5846233469';
function run(query, blocked = false) {
  let target;
  const status = {};
  const link = { hidden: true };
  const spinner = { hidden: false };
  const timers = [];
  vm.runInNewContext(script, {
    URLSearchParams,
    window: { location: { search: query, replace(value) { if (blocked) throw new Error('blocked'); target = value; } } },
    document: { getElementById: name => ({ 'return-status': status, 'return-link': link, 'return-spinner': spinner })[name] },
    setTimeout(callback, delay) { timers.push({ callback, delay }); },
    fetch() { assert.fail('bridge must not contact any API'); },
    localStorage: { getItem() { assert.fail('bridge must not access credentials'); } },
  });
  return { target, status, link, spinner, timers };
}

test('public bridge returns only to supported localhost origins with same session', () => {
  for (const port of ['3000', '5173']) {
    const { target, link, timers } = run(`?sessionId=${id}&port=${port}`);
    assert.equal(target, `http://localhost:${port}/thanh-toan/sepay/${id}`);
    assert.equal(link.href, target);
    assert.equal(link.hidden, true);
    assert.equal(timers[0].delay, 3000);
  }
});

test('bridge ignores result, tokens and attacker-controlled redirect destinations', () => {
  const { target } = run(`?sessionId=${id}&port=3000&returnUrl=https://evil.example&host=evil.example&token=private&paymentStatus=PAID`);
  assert.equal(target, `http://localhost:3000/thanh-toan/sepay/${id}`);
  assert.ok(!target.includes('private'));
});

test('invalid, duplicate and missing inputs never redirect', () => {
  for (const query of ['', `?sessionId=${id}`, '?sessionId=invalid&port=3000', `?sessionId=${id}&port=8080`, `?sessionId=${id}&port=3000@evil.example`, `?sessionId=${id}&port=3000&port=5173`, `?sessionId=${id}&sessionId=${id}&port=3000`, '?sessionId=%3Cscript%3E&port=3000']) {
    const { target, status, link } = run(query);
    assert.equal(target, undefined);
    assert.equal(link.hidden, true);
    assert.match(status.textContent, /không hợp lệ/);
  }
});

test('manual return remains available if automatic navigation is blocked', () => {
  const { target, link } = run(`?sessionId=${id}&port=5173`, true);
  assert.equal(target, undefined);
  assert.equal(link.href, `http://localhost:5173/thanh-toan/sepay/${id}`);
  assert.equal(link.hidden, false);
});

test('bridge shows a manual link after a short wait without further navigation', () => {
  const { link, spinner, timers } = run(`?sessionId=${id}&port=3000`);
  assert.equal(spinner.hidden, false);
  timers[0].callback();
  assert.equal(link.hidden, false);
  assert.equal(spinner.hidden, true);
});

test('static bridge has no SPA login dependency and suppresses referrer leakage', async () => {
  const html = await readFile(new URL('../public/sepay-local-return.html', import.meta.url), 'utf8');
  assert.match(html, /name="referrer" content="no-referrer"/);
  assert.match(html, /<script src="\/sepay-local-return.js" defer>/);
  assert.match(html, /<noscript>/);
  assert.doesNotMatch(html, /src="\/src\/main|Authorization|SEPAY_SECRET/);
});
