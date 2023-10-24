using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Response.Feedback
{
    public class FeedbackRes
    {
        public string Content { get; set; }
        public float Rating { get; set; }
        public string img_url { get; set; }
    }
}
