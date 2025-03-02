export interface LoginRequest {
    username: string;
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
    username: string;
    isAdmin: boolean;
    createdAt: string;
    lastLoginAt: string | null;
}

export interface CreateUserRequest {
    username: string;
    password: string;
    isAdmin: boolean;
}
