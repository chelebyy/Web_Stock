namespace Stock.Application.Common.CQRS
{
    /// <summary>
    /// Void dönüş tipi yerine kullanılan Unit tipi
    /// </summary>
    public struct Unit
    {
        /// <summary>
        /// Unit tipinin tek değeri
        /// </summary>
        public static Unit Value => new Unit();
    }
} 