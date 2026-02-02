import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: { 
    proxy: {
      '/api': {
        // target: 'http://localhost:5070',
        target: 'https://localhost:7095',
        changeOrigin: true,
        secure: false
      }
    }
  }
})
