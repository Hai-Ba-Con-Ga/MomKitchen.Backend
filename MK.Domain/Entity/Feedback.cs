using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    [Table("feedback")]
    public class Feedback : BaseEntity
    {
        [Column("content")]
        [DataType(DataType.Text)]
        public string Content { get; set; } = null!;

        [Column("rating")]
        [Range(1, 5)]
        public float Rating { get; set; }

        [DataType(DataType.Text)]
        public string? ImgUrl { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = null!;

        [Required]
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;

        [Required]
        public Guid KitchenId { get; set; }
        public virtual Kitchen Kitchen { get; set; } = null!;

    }
}
