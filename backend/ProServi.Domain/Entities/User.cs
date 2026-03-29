using ProServi.Domain.Enums;

namespace ProServi.Domain.Entities;

public class User
{
    // Identificadores y roles
    public int Id { get; set; }
    public UserRole Role { get; set; }
    
    // Información de contacto
    public string Email { get; set; }
    public string Phone { get; set; }
    public string FullName { get; set; }
    
    // Seguridad
    public string PasswordHash { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    
    // Estado de la cuenta
    public UserStatus Status { get; set; }
    public bool EmailVerified { get; set; }
    public bool PhoneVerified { get; set; }
    
    // Auditoría
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? LastLogin { get; set; }
    
    // Relaciones
    public virtual Customer? Customer { get; set; }
    public virtual Professional? Professional { get; set; }
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<SavedCard> SavedCards { get; set; } = new List<SavedCard>();
}
