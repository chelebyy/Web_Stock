namespace Stock.Domain.Events
{
    /// <summary>
    /// Ürün stok değişikliği domain olayı
    /// </summary>
    public class ProductStockChangedEvent : DomainEvent
    {
        /// <summary>
        /// Ürün ID
        /// </summary>
        public int ProductId { get; }
        
        /// <summary>
        /// Yeni stok miktarı
        /// </summary>
        public int NewStockQuantity { get; }
        
        /// <summary>
        /// Yeni bir ProductStockChangedEvent oluşturur
        /// </summary>
        public ProductStockChangedEvent(int productId, int newStockQuantity)
        {
            ProductId = productId;
            NewStockQuantity = newStockQuantity;
        }
    }
} 