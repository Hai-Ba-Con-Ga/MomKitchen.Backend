using MK.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Dto.Request
{
    public class CreateAreaReq
    {
        [Required]
        public string Name { get; set; } = null!;
        public IEnumerable<CreateLocationReq> Boundaries { get; set; } = null!;
    }

    public class CreateAreaReqValidator : AbstractValidator<CreateAreaReq>
    {
        public CreateAreaReqValidator()
        {
            RuleFor(area => area.Name)
                .NotEmpty()
                .WithMessage("Name is required.");

            RuleForEach(area => area.Boundaries)
                .SetValidator(new CreateLocationReqValidator())
                .WithMessage("Locations are invalid");
        }
    }
}
