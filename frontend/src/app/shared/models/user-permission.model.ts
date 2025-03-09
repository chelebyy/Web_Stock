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