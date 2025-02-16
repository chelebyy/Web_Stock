using System;
using System.ComponentModel.DataAnnotations;
using StockAPI.Models.Base;

namespace StockAPI.Models
{
    public class ITEquipment : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string SerialNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string Brand { get; set; }

        [Required]
        [MaxLength(50)]
        public string Model { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public EquipmentStatus Status { get; set; }

        [Required]
        [MaxLength(100)]
        public string Location { get; set; }

        [MaxLength(100)]
        public string AssignedTo { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }
    }

    public enum EquipmentStatus
    {
        Available,
        InUse,
        UnderMaintenance,
        Broken,
        Disposed
    }
}
