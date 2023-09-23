using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    public class Feedback : BaseEntity
    {
        [ForeignKey("CustomerId")]
        [InverseProperty("Feedbacks")]
        public virtual Customer Customer { get; set; } = null!;

        [ForeignKey("KitchenId")]
        [InverseProperty("Feedbacks")]
        public virtual Kitchen Kitchen { get; set; } = null!;

        [Column("content")]
        [StringLength(500)]
        public string Content { get; set; } = null!;

        [Column("rating")]
        public int Rating { get; set; }

    }
}
