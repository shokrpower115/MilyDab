namespace ProServi.Domain.Entities;

public class Budget
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int ProfessionalId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Relaciones
    public virtual Project Project { get; set; }
    public virtual Professional Professional { get; set; }
}
