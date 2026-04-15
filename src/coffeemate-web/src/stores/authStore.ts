import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import apiClient from '../api/axios'
import type { RegisterDto, LoginDto, AuthResponseDto, AuthUser } from '../types/auth'

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('token'))
  const user = ref<AuthUser | null>(
    JSON.parse(localStorage.getItem('user') ?? 'null'),
  )

  const isAuthenticated = computed(() => !!token.value)

  async function login(dto: LoginDto): Promise<void> {
    const { data } = await apiClient.post<AuthResponseDto>('/auth/login', dto)
    _setAuth(data)
  }

  async function register(dto: RegisterDto): Promise<void> {
    const { data } = await apiClient.post<AuthResponseDto>('/auth/register', dto)
    _setAuth(data)
  }

  function logout(): void {
    token.value = null
    user.value = null
    localStorage.removeItem('token')
    localStorage.removeItem('user')
  }

  function _setAuth(data: AuthResponseDto): void {
    token.value = data.token
    user.value = { email: data.email, displayName: data.displayName }
    localStorage.setItem('token', data.token)
    localStorage.setItem('user', JSON.stringify(user.value))
  }

  return { token, user, isAuthenticated, login, register, logout }
})
