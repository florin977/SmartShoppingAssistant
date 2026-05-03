using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.DataAccess.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public Cart Cart { get; set; } = null!;
    }
}
