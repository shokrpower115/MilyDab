namespace ProServi.Domain.Entities;

public class Specialty
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    // Relaciones
    public virtual ICollection<Professional> Professionals { get; set; } = new List<Professional>();
}
