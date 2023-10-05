namespace MK.Domain.Dto.Request.User
{
    public class UpdateUserRequest
    {
        public string Email { get; set; } = null!;
        public string? FullName { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public string Phone { get; set; } = null!;
        public DateTime? Birthday { get; set; }
    }
}
