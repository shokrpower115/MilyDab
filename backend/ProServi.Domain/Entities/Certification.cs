namespace ProServi.Domain.Entities;

public class Certification
{
    public int Id { get; set; }
    public int ProfessionalId { get; set; }
    public string Title { get; set; }
    public string IssuingOrganization { get; set; }
    public string CertificateUrl { get; set; }
    public DateTime IssuedDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    
    // Relaciones
    public virtual Professional Professional { get; set; }
}
