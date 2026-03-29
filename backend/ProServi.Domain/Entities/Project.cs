using ProServi.Domain.Enums;

namespace ProServi.Domain.Entities;

public class Project
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ProfessionalId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ProjectStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal Budget { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relaciones
    public virtual Customer Customer { get; set; }
    public virtual Professional Professional { get; set; }
    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}
