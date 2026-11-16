(() => {
  'use strict';
  const status = document.getElementById('return-status');
  const link = document.getElementById('return-link');
  const spinner = document.getElementById('return-spinner');
  const params = new URLSearchParams(window.location.search);
  const sessionId = params.get('sessionId');
  const port = params.get('port');
  // Ignore all other parameters, including any returnUrl, host, token or result.
  const valid = params.getAll('sessionId').length === 1 && params.getAll('port').length === 1
    && /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i.test(sessionId || '')
    && (port === '3000' || port === '5173');
  if (!valid) {
    spinner.hidden = true;
    status.textContent = 'Thông tin quay về không hợp lệ. Vui lòng tự mở lại ứng dụng localhost.';
    return;
  }
  const target = `http://localhost:${port}/thanh-toan/sepay/${sessionId}`;
  link.href = target;
  const showFallback = () => {
    spinner.hidden = true;
    link.hidden = false;
    status.textContent = 'Nếu trang chưa tự chuyển, hãy bấm tiếp tục để xem kết quả.';
  };
  setTimeout(showFallback, 3000);
  try { window.location.replace(target); } catch { showFallback(); }
})();
