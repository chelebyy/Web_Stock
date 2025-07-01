namespace Stock.Application.Common.Constants
{
    public static class LogMessages
    {
        public const string AddingMissingPermissions = "Eksik izinler kontrol ediliyor ve ekleniyor...";
        public const string LogPasswordChangeAttempt = "User {UserId} is attempting to change their password.";
        public const string LogUserNotFoundById = "User with ID {UserId} not found during password change attempt.";
        public const string LogInvalidCurrentPassword = "User {UserId} provided an invalid current password.";
        public const string LogPasswordChangedSuccessfully = "User {UserId} successfully changed their password.";
        public const string LogPasswordChangeError = "An error occurred while changing password for user {UserId}.";
        public const string ResettingPermissionsForUser = "Resetting all direct permissions for user {UserId}.";
        public const string PermissionsResetForUser = "All direct permissions have been reset for user {UserId}.";
        public const string UserPermissionsResetNoAction = "User {UserId} had no direct permissions to reset.";
        public const string PermissionRemovedFromUser = "Permission {PermissionId} was removed from user {UserId}.";
        public const string CreatingUserWithSicil = "Sicil {Sicil} ile kullanıcı oluşturuluyor";
        public const string UserCreatedWithSicil = "Kullanıcı başarıyla oluşturuldu. ID: {UserId}, Sicil: {Sicil}";
        public const string ErrorCreatingUserWithSicil = "Sicil {Sicil} ile kullanıcı oluşturulurken hata oluştu";
        
        public const string UpdatingUser = "Kullanıcı güncelleniyor. ID: {UserId}";
        public const string UserUpdated = "Kullanıcı başarıyla güncellendi. ID: {UserId}";
        public const string ErrorUpdatingUser = "Kullanıcı güncellenirken hata oluştu. ID: {UserId}";
        
        public const string DeletingUser = "Kullanıcı siliniyor. ID: {UserId}";
        public const string UserDeleted = "Kullanıcı başarıyla silindi. ID: {UserId}";
        public const string ErrorDeletingUser = "Kullanıcı silinirken hata oluştu. ID: {UserId}";
    }
} 