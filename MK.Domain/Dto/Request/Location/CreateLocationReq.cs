
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request
{
    public class CreateLocationReq
    {
        [Required]
        public double Lat { get; set; }
        [Required]
        public double Lng { get; set; }
    }

    public class CreateLocationReqValidator : AbstractValidator<CreateLocationReq>
    {
        public CreateLocationReqValidator()
        {
            RuleFor(x => x.Lat).NotEqual(0).WithMessage("Lat is required");
            RuleFor(x => x.Lng).NotEqual(0).WithMessage("Lng is required");
        }
    }
}
