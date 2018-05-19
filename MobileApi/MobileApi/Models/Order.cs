using System;
using System.Collections.Generic;

namespace MobileApi.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public string MoreInformation { get; set; }
        public List<Hierarchy> OrderLines { get; set; }
        public Customer Customer { get; set; }
        public ProjectLocation Location { get; set; }
        public DateTime ProjectTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public String DocumentType
        {
            set { value = "Order"; }
        }
    }
}