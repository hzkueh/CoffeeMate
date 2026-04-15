export interface RegisterDto {
  email: string
  password: string
  displayName: string
}

export interface LoginDto {
  email: string
  password: string
}

export interface AuthResponseDto {
  token: string
  email: string
  displayName: string
}

export interface AuthUser {
  email: string
  displayName: string
}
