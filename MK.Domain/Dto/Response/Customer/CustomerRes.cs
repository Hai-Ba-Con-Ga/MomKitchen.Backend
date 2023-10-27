using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MK.Domain.Dto.Response.User;
using MK.Domain.Enum;

namespace MK.Domain.Dto.Response.Customer
{
    public class CustomerRes
    {
        public int No { get; set; }
        public Guid Id  { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? Birthday { get; set; }
        public string? AvatarUrl { get; set; }
        
        public CustomerStatus Status { get; set; }
        //TODO: handle properties
        public int? OrderQuantity { get; set; } = null;
        public double? SpentMoney { get; set; } = null;
        public Guid UserId { get; set; }
        public UserRes User { get; set; } = null;
        
        // public virtual ICollection<FavouriteKitchenRes> FavouriteKitchens { get; set; } = new List<FavouriteKitchenRes>();
        // public virtual ICollection<FeedbackRes> Feedbacks { get; set; } = new List<FeedbackRes>();
        // public virtual ICollection<OrderRes> Orders { get; set; } = new List<OrderRes>();

    }
}
