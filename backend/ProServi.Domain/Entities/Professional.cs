namespace ProServi.Domain.Entities;

public class Professional
{
    public int Id { get; set; }
    public int UserId { get; set; }
    
    // Especialidad y ubicación
    public int SpecialtyId { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    
    // Geolocalización
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    // Información laboral
    public string Bio { get; set; }
    public bool IsVerified { get; set; }
    public int YearsOfExperience { get; set; }
    
    // Calificaciones
    public decimal AverageRating { get; set; } = 0;
    public int TotalReviews { get; set; } = 0;
    public int CompletedProjects { get; set; } = 0;
    
    // Disponibilidad
    public bool IsAvailable { get; set; } = true;
    public string? HourlyRate { get; set; }
    
    // Auditoría
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? LastActive { get; set; }
    
    // Relaciones
    public virtual User User { get; set; }
    public virtual Specialty Specialty { get; set; }
    public virtual ICollection<ContactRequest> ContactRequests { get; set; } = new List<ContactRequest>();
    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    public virtual ICollection<Review> ReviewsReceived { get; set; } = new List<Review>();
    public virtual ICollection<Certification> Certifications { get; set; } = new List<Certification>();
}
