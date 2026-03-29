namespace ProServi.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public int UserId { get; set; }
    
    // Información de direcciones
    public string City { get; set; }
    public string Country { get; set; }
    public string? Street { get; set; }
    public string? ApartmentNumber { get; set; }
    
    // Geolocalización
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    // Preferencias
    public string? PreferredContactMethod { get; set; }
    public decimal AverageRating { get; set; } = 0;
    public int TotalProjects { get; set; } = 0;
    
    // Auditoría
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relaciones
    public virtual User User { get; set; }
    public virtual ICollection<ContactRequest> ContactRequests { get; set; } = new List<ContactRequest>();
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    public virtual ICollection<Review> ReviewsGiven { get; set; } = new List<Review>();
}
