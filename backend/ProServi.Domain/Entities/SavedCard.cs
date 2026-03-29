namespace ProServi.Domain.Entities;

public class SavedCard
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string LastFourDigits { get; set; }
    public string CardholderName { get; set; }
    public string ExpiryMonth { get; set; }
    public string ExpiryYear { get; set; }
    public string CardToken { get; set; }
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Relaciones
    public virtual User User { get; set; }
}
