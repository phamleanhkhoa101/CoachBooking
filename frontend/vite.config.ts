import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

// Cấu hình dev server: mọi request /api được chuyển tiếp sang backend Spring Boot.
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'http://localhost:8080',
        changeOrigin: true,
      },
    },
  },
});
