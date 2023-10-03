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
        [Required]
        public CreateLocationReq North { get; set; } = null!;
        [Required]
        public CreateLocationReq South { get; set; } = null!;
        [Required]
        public CreateLocationReq East { get; set; } = null!;
        [Required]
        public CreateLocationReq West { get; set; } = null!;
    }

    public class CreateAreaReqValidator : AbstractValidator<CreateAreaReq>
    {
        public CreateAreaReqValidator()
        {
            RuleFor(area => area.Name)
                .NotEmpty()
                .WithMessage("Name is required.");

            RuleFor(area => area.North)
                .SetValidator(new CreateLocationReqValidator()).WithMessage("North is invalid");

            RuleFor(area => area.South)
                .SetValidator(new CreateLocationReqValidator()).WithMessage("South is invalid");

            RuleFor(area => area.East)
                .SetValidator(new CreateLocationReqValidator()).WithMessage("East is invalid");

            RuleFor(area => area.West)
                .SetValidator(new CreateLocationReqValidator()).WithMessage("West is invalid");
        }
    }
}
