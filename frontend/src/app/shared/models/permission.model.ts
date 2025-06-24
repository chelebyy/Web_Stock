export interface Permission {
    id: number;
    name: string;
    description: string;
    displayName?: string;
    resourceType?: string;
    group: string;
    isFromRole?: boolean;
    isCustom?: boolean;
    roleName?: string;
    isGranted?: boolean;
}

export interface PermissionGroup {
    group: string;
    name: string;
    permissions: Permission[];
}

export interface AssignPermissionsRequest {
    permissionIds: number[];
} 