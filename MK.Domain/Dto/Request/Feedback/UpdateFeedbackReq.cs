using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request.Feedback
{
    public class UpdateFeedbackReq
    {
        public string? Content { get; set; }
        [Range(1, 5)]

        public float Rating { get; set; }

        public string? ImgUrl { get; set; }
    }
}
