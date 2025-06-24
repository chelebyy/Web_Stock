export interface LoginRequest {
    sicil: string;
    password: string;
}

export interface LoginResponse {
    success: boolean;
    token: string;
    username: string;
    isAdmin: boolean;
    roleName?: string;
    errorMessage?: string;
}

export interface User {
    id: number;
    username?: string;
    adi?: string;
    soyadi?: string;
    sicil: string;
    isAdmin: boolean;
    createdAt: string;
    lastLoginAt: string | null;
}

export interface CreateUserRequest {
    username?: string;
    password: string;
    sicil: string;
    adi?: string;
    soyadi?: string;
    isAdmin: boolean;
    roleId?: number;
}

export interface ChangePasswordRequest {
    currentPassword: string;
    newPassword: string;
} 