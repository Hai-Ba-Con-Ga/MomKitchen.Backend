using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    [Table("order")]
    public partial class Order : BaseEntity
    {
        public double TotalPrice { get; set; }

        public int TotalQuantity { get; set; }

        public decimal Surcharge { get; set; }

        public OrderStatus Status { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = null!;

        [Required]
        public Guid MealId { get; set; }
        public virtual Meal Meal { get; set; } = null!;

        public virtual Feedback? Feedback { get; set; }

        public virtual ICollection<OrderPayment> OrderPayments { get; set; } = new List<OrderPayment>();
    }
}
