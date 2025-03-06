export interface Permission {
    id: number;
    name: string;
    description: string;
    resourceType?: string;
    group: string;
    isFromRole?: boolean;
    isCustom?: boolean;
    roleName?: string;
}

export interface PermissionGroup {
    group: string;
    permissions: Permission[];
}

export interface AssignPermissionsRequest {
    permissionIds: number[];
}

export interface UserPermission {
    id: number;
    userId: number;
    permissionId: number;
    isGranted: boolean;
    userName?: string;
    permissionName?: string;
    resourceType?: string;
    resourceName?: string;
    action?: string;
    description?: string;
    group?: string;
} 