export type { Role } from './role.model';
import { Role } from './role.model';

export interface User {
    id?: number;
    username: string;
    passwordHash?: string;
    password?: string;
    sicil: string;
    isAdmin: boolean;
    createdAt: Date;
    lastLoginAt?: Date;
    roleId?: number;
    role?: Role;
    roleName?: string;
    firstName?: string;
    lastName?: string;
}

export interface LoginRequest {
    sicil: string;
    password: string;
}

export interface LoginResponse {
    token: string;
    user: User;
}

export interface CreateUserDto {
    username: string;
    password: string;
    sicil: string;
    passwordHash?: string;
    isAdmin: boolean;
    roleId?: number;
    createdAt?: Date;
}

export interface UpdateUserDto {
    username?: string;
    password?: string;
    sicil?: string;
    isAdmin?: boolean;
    roleId?: number;
}
