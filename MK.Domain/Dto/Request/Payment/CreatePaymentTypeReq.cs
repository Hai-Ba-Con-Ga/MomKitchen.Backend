using System.ComponentModel.DataAnnotations;
using MK.Domain.Enum;

namespace MK.Domain.Dto.Request.Payment
{
    public class CreatePaymentTypeReq
    {
        [Required]
        public string Provider { get; set; } = null!;
        
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public PaymentTypeStatus Status { get; set; } = PaymentTypeStatus.Active;
    }
}