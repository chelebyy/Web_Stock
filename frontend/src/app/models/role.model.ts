import { Permission } from './permission.model';

export interface Role {
    id: number;
    name: string;
    description: string;
    userCount?: number;
    permissions?: Permission[];
}

export interface RoleWithUsers extends Role {
    users: any[];
} 