using ProServi.Domain.Enums;

namespace ProServi.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public string TransactionId { get; set; }
    public string PaymentMethod { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    // Relaciones
    public virtual Project Project { get; set; }
    public virtual Customer Customer { get; set; }
}
