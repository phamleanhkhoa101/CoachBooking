import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { App } from './App';
import './styles.css';

// Dọn sạch mọi Service Worker và CacheStorage tồn dư từ các dự án khác trên localhost:3000
if (typeof window !== 'undefined' && 'serviceWorker' in navigator) {
  navigator.serviceWorker.getRegistrations().then((registrations) => {
    for (const registration of registrations) {
      registration.unregister();
    }
  });
}
if (typeof window !== 'undefined' && 'caches' in window) {
  caches.keys().then((names) => {
    for (const name of names) {
      caches.delete(name);
    }
  });
}

const container = document.getElementById('root');
if (!container) {
  throw new Error('Không tìm thấy phần tử gốc #root.');
}

createRoot(container).render(
  <StrictMode>
    <App />
  </StrictMode>,
);
