using SistemaGestionTareas.Infrastructure.AuthProviders.Identity;

namespace SistemaGestionTareas.Infrastructure.TokenProviders
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(1);
        public bool IsRevoked { get; private set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationIdentityUser User { get; set; } = default!;

        public void Revoke()
        {
            IsRevoked = true;
        }
        
    }
}
