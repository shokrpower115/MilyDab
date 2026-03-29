namespace ProServi.Application.DTOs.Auth;

public class ProfessionalDetailDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Specialty { get; set; }
    public string Bio { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public int YearsOfExperience { get; set; }
    public decimal AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public int CompletedProjects { get; set; }
    public bool IsVerified { get; set; }
    public bool IsAvailable { get; set; }
    public string HourlyRate { get; set; }
}
