export interface Permission {
    id: number;
    name: string;
    description: string;
    group: string;
    resourceType?: string;
    resourceName?: string;
    action?: string;
    isFromRole?: boolean;
    isCustom?: boolean;
    roleName?: string;
}

export interface PermissionGroup {
    group: string;
    permissions: Permission[];
} 