export interface User {
    id: number;
    username: string;
    passwordHash?: string;
    password?: string;
    isAdmin: boolean;
    createdAt: Date;
    lastLoginAt?: Date;
}

export interface LoginRequest {
    username: string;
    password: string;
}

export interface LoginResponse {
    token: string;
    user: User;
}
