
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
            RuleFor(location => location.Lat)
                .NotEmpty()
                .InclusiveBetween(-90, 90) // Latitude range is -90 to 90 degrees
                .WithMessage("Latitude must be between -90 and 90 degrees.");

            RuleFor(location => location.Lng)
                .NotEmpty()
                .InclusiveBetween(-180, 180) // Longitude range is -180 to 180 degrees
                .WithMessage("Longitude must be between -180 and 180 degrees.");
        }
    }
}
