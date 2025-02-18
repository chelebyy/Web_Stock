export interface Role {
    id: number;
    name: string;
    createdAt: string | Date;
    updatedAt?: string | Date;
}

export interface RoleWithUsers extends Role {
    users: {
        id: number;
        username: string;
        isAdmin: boolean;
        createdAt: string;
    }[];
} 