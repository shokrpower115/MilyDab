using ProServi.Domain.Enums;

namespace ProServi.Domain.Entities;

public class ContactRequest
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ProfessionalId { get; set; }
    public string Description { get; set; }
    public ContactMethod PreferredMethod { get; set; }
    public ContactRequestStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relaciones
    public virtual Customer Customer { get; set; }
    public virtual Professional Professional { get; set; }
}
