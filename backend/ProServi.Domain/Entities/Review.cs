namespace ProServi.Domain.Entities;

public class Review
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int CustomerId { get; set; }
    public int ProfessionalId { get; set; }
    public decimal Rating { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Relaciones
    public virtual Project Project { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual Professional Professional { get; set; }
}
