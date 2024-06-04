using System;
using System.Diagnostics.CodeAnalysis;

namespace Inventory.Location.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class RequestBody
    {
        public string zipCode { get; set; }
        public string itemId { get; set; }
        public string itemColorId { get; set; }


    }
}

