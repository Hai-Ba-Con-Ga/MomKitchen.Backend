using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.Feedback
{
    public class CreateFeedbackReq
    {
        [Required]
        public string Content { get; set; } = null!;
        [Required]
        [Range(1, 5)]

        public float Rating { get; set; }
        
        public string? ImgUrl { get; set; }

        public Guid CustomerId { get; set; }

        public Guid OrderId { get; set; }
        public Guid KitchenId { get; set;}
        
    }
}
