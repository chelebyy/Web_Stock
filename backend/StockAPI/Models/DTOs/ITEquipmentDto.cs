using System;
using StockAPI.Models;

namespace StockAPI.Models.DTOs
{
    public class ITEquipmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime PurchaseDate { get; set; }
        public EquipmentStatus Status { get; set; }
        public string Location { get; set; }
        public string AssignedTo { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateITEquipmentDto
    {
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime PurchaseDate { get; set; }
        public EquipmentStatus Status { get; set; }
        public string Location { get; set; }
        public string AssignedTo { get; set; }
        public string Notes { get; set; }
    }

    public class UpdateITEquipmentDto
    {
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime PurchaseDate { get; set; }
        public EquipmentStatus Status { get; set; }
        public string Location { get; set; }
        public string AssignedTo { get; set; }
        public string Notes { get; set; }
    }
}
