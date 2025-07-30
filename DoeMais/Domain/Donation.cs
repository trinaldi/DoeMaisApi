using DoeMais.Domain.Entities;
using DoeMais.Domain.Enums;

namespace DoeMais.Domain;

public class Donation
{
    public long DonationId { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public int? Quantity { get; set; }
    public Status Status { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public List<string> Images { get; set; }
}