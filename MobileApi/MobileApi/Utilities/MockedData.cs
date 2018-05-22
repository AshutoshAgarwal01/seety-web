using MobileApi.Models;
using System;
using System.Collections.Generic;

namespace MobileApi.Utilities
{
    public static class MockedData
    {
        public static Order GetOrder()
        {
            var order = new Order();
            order.CreatedDateTime = DateTime.Now;
            order.Customer = new Customer()
            {
                Email = "a@a.com",
                FirstName = "Test First Name",
                LastName = "Test Last Name",
                PhoneNumber = "1234567890"
            };

            order.OrderId = "Test-" + DateTime.Now;
            order.Location = new ProjectLocation()
            {
                City = "Wonderland",
                Pin = "9876",
                State = "Very Bad",
                Street = "1 My Way"
            };

            //order.ProjectTime = DateTime.Now;

            order.OrderLines = getOrderLines();

            return order;
        }

        private static List<OrderLine> getOrderLines()
        {
            var node1 = new HierarchyNode()
            {
                NodeId = 1,
                Name = "Air Conditioner",
                Description = "What kind of air conditioner is this?",
                OptionType = Enum.OptionType.SINGLE,
                ChildrenNodes = new List<HierarchyNode>()
            };

            var node2 = new HierarchyNode()
            {
                NodeId = 2,
                Name = "Split",
                Description = "",
                OptionType = Enum.OptionType.SINGLE,
                ChildrenNodes = new List<HierarchyNode>()
            };

            var node3 = new HierarchyNode()
            {
                NodeId = 3,
                Name = "Window",
                Description = "",
                OptionType = Enum.OptionType.SINGLE,
                ChildrenNodes = null
            };


            node2.ChildrenNodes.Add(node3);
            node1.ChildrenNodes.Add(node2);

            var orderLines = new List<OrderLine>();
            orderLines.Add(new OrderLine(1, DateTime.Now, "This is a test order line.", node1));

            return orderLines;
        }
    }
}