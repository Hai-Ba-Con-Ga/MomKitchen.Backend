using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MK.Domain.Dto.Response.Customer;

namespace MK.Domain.Dto.Response.Feedback
{
    public class FeedbackRes
    {
        public int No { get; set; }
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public float Rating { get; set; }
        public string? ImgUrl { get; set; }
        public Guid KitchenId { get; set; }
        public OwnerRes Owner { get; set; } = null!;
        public Guid OrderId { get; set; }
        
    }
}
