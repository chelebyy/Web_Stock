// using Stock.Domain.Common.Result; // Artık Error tipi kullanılmadığı için kaldırıldı

namespace Stock.Domain.Common;

public static class DomainErrors
{
    public static class User
    {
        public const string SicilEmpty = "User.SicilEmpty: Sicil cannot be empty."; // const string olarak tanımlandı
        public static string SicilTooLong(int maxLength) => $"User.SicilTooLong: Sicil cannot exceed {maxLength} characters."; // Eklendi
        public const string SicilNull = "User.Sicil.Null: Sicil cannot be null."; // Eklendi
        public static string SicilAlreadyExists(string sicil) => $"User.SicilAlreadyExists: Sicil '{sicil}' is already taken."; // string döndüren metot
        // public const string NameEmpty = "User.NameEmpty: First name cannot be empty."; // Bu AdiEmpty ile değiştirildi
        // public const string SurnameEmpty = "User.SurnameEmpty: Last name cannot be empty."; // Bu SoyadiEmpty ile değiştirildi
        public const string AdiEmpty = "User.AdiEmpty: User name (Adi) cannot be empty."; // Eklendi
        public const string SoyadiEmpty = "User.SoyadiEmpty: User surname (Soyadi) cannot be empty."; // Eklendi
        public const string FullNameNull = "User.FullName.Null: FullName cannot be null."; // Eklendi
        public static string NotFound(int id) => $"User.NotFound: User with Id '{id}' not found.";
        public const string PasswordHashEmpty = "User.PasswordHashEmpty: Password hash cannot be empty."; // Eklendi
        // İhtiyaç duyulan diğer User hataları buraya eklenebilir (örn: SicilInvalidFormat, NameTooLong vb.)
    }

    public static class Role
    {
        public const string NameEmpty = "Role.NameEmpty: Role name cannot be empty.";
        public const string NameTooLong = "Role.NameTooLong: Role name cannot exceed 50 characters.";
        public const string RoleNameNull = "Role.Name.Null: RoleName cannot be null."; // Eklendi
        public const string DescriptionTooLong = "Role.DescriptionTooLong: Role description cannot exceed 200 characters.";
        public static string NotFound(int id) => $"Role.NotFound: Role with Id '{id}' not found.";
        public static string NameAlreadyExists(string name) => $"Role.NameAlreadyExists: Role with name '{name}' already exists.";
    }

    public static class Product
    {
         public const string NameEmpty = "Product.NameEmpty: Product name cannot be empty.";
         public const string StockCannotBeNegative = "Product.StockCannotBeNegative: Stock level cannot be negative.";
         public const string StockToAddMustBePositive = "Product.StockToAddMustBePositive: Stock amount to add must be positive.";
         public const string StockToRemoveMustBePositive = "Product.StockToRemoveMustBePositive: Stock amount to remove must be positive.";
         public const string InsufficientStock = "Product.InsufficientStock: Insufficient stock level.";
         public static string NotFound(int id) => $"Product.NotFound: Product with Id '{id}' not found.";
         // Diğer Product hataları
    }
    
    public static class Permission
    {
        public const string NameEmpty = "Permission.NameEmpty: Permission name cannot be empty.";
        public const string NameTooLong = "Permission.NameTooLong: Permission name cannot exceed 100 characters.";
        public const string PermissionNameNull = "Permission.Name.Null: PermissionName cannot be null.";
        public const string DescriptionTooLong = "Permission.DescriptionTooLong: Permission description cannot exceed 200 characters.";
        public const string ResourceTypeTooLong = "Permission.ResourceTypeTooLong: ResourceType cannot exceed 50 characters.";
        public const string GroupTooLong = "Permission.GroupTooLong: Group cannot exceed 50 characters.";
        public static string NotFound(int id) => $"Permission.NotFound: Permission with Id '{id}' not found.";
        public static string NameAlreadyExists(string name) => $"Permission.NameAlreadyExists: Permission with name '{name}' already exists.";
    }

    public static class Category
    {
        public const string NameEmpty = "Category.NameEmpty: Category name cannot be empty.";
        public const string NameTooLong = "Category.NameTooLong: Category name cannot exceed 100 characters.";
        public const string CategoryNameNull = "Category.Name.Null: CategoryName cannot be null.";
        public static string NotFound(int id) => $"Category.NotFound: Category with Id '{id}' not found.";
    }
    
    // Diğer domain entity'leri için hata sınıfları buraya eklenebilir
    // public static class Order { ... }
} 