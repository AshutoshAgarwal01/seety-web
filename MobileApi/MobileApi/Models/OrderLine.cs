using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApi.Models
{
    public class OrderLine
    {
        public int OrderLineId { get; set; }
        public DateTime ProjectTime { get; set; }
        public String MoreInformation { get; set; }
        public HierarchyNode ServiceInfo { get; set; }

        public OrderLine(int orderLineId, DateTime projectTime, String moreInformation, HierarchyNode serviceInfo)
        {
            OrderLineId = orderLineId;
            ProjectTime = projectTime;
            MoreInformation = moreInformation;
            ServiceInfo = serviceInfo;
        }
    }
}