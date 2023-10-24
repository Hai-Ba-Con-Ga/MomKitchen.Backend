using MK.Domain.Dto.Request.Customer;
using MK.Domain.Dto.Response.Customer;
using MK.Domain.Dto.Response.Feedback;
using MK.Domain.Entity;
using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Response.Order
{
    public class OrderDetailRes
    {
        public Guid Id { get; set; }
        public int No { get; set; }
        public double TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public decimal Surcharge { get; set; }
        public OrderStatus Status { get; set; }
        public CustomerRes? Customer { get; set; } = null;
        public MealRes? Meal { get; set; } = null;
        public FeedbackRes? Feedback { get; set; } = null;
    }
}
