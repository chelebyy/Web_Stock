export interface Permission {
    id: number;
    name: string;
    description: string;
    group: string;
}

export interface PermissionGroup {
    group: string;
    permissions: Permission[];
} 