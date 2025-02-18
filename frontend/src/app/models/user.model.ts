import { Role } from './role.model';

export interface User {
    id?: number;
    username: string;
    passwordHash?: string;
    password?: string;
    isAdmin: boolean;
    createdAt: Date;
    lastLoginAt?: Date;
    roleId?: number;
    role?: Role;
}

export interface LoginRequest {
    username: string;
    password: string;
}

export interface LoginResponse {
    token: string;
    user: User;
}

export interface CreateUserDto {
    username: string;
    password: string;
    passwordHash?: string;
    isAdmin: boolean;
    roleId?: number;
    createdAt?: Date;
}

export interface UpdateUserDto {
    username?: string;
    password?: string;
    isAdmin?: boolean;
    roleId?: number;
}
