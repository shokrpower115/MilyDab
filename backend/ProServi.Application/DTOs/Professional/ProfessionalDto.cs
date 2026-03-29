namespace ProServi.Application.DTOs.Auth;

public class ProfessionalDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Specialty { get; set; }
    public string City { get; set; }
    public decimal AverageRating { get; set; }
    public bool IsAvailable { get; set; }
}
