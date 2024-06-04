using System;
using System.Diagnostics.CodeAnalysis;

namespace Inventory.Location.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class InventoryOnhandLocation
    {
        public string AvailableQuantity { get; set; }
        public string StoreId { get; set; }
        public string ItemId { get; set; }
        public string ItemColorId { get; set; }
        public string ItemSizeId { get; set; }
        public string ItemStyleId { get; set; }
        public string ZipCode { get; set; }

    }
}