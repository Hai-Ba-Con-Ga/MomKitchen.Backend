using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Entity
{
    [Table("conversation")]
    public partial class Conversation : BaseEntity
    {
        [ForeignKey("CustomerId")]
        [InverseProperty("Conversations")]
        public virtual Customer Customer { get; set; } = null!;

        [ForeignKey("KitchenId")]
        [InverseProperty("Conversations")]
        public virtual Kitchen Kitchen { get; set; } = null!;

        [Column("content")]
        [StringLength(500)]
        public string Content { get; set; } = null!;
    }
}
