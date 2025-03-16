using System;
using Stock.Domain.Entities;

namespace Stock.Domain.Services
{
    /// <summary>
    /// Stok tahsisi domain servisi
    /// </summary>
    public class StockAllocationService
    {
        /// <summary>
        /// Sipariş için stok tahsisi yapar
        /// </summary>
        public bool AllocateStock(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
                
            if (quantity <= 0)
                throw new ArgumentException("Miktar pozitif olmalıdır.", nameof(quantity));
                
            if (product.StockQuantity < quantity)
                return false;
                
            product.UpdateStock(-quantity);
            return true;
        }
        
        /// <summary>
        /// Stok tahsisini iptal eder
        /// </summary>
        public void DeallocateStock(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
                
            if (quantity <= 0)
                throw new ArgumentException("Miktar pozitif olmalıdır.", nameof(quantity));
                
            product.UpdateStock(quantity);
        }
    }
} 