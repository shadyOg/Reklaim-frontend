namespace Reklaim_frontend.Models;

public class ClaimRequestDto
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int ClaimerUserId { get; set; }
    public string ProofDescription { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public string? PostTitle { get; set; }
    public string? ClaimerName { get; set; }
    public string? ClaimerPhoneNumber { get; set; }
    public string? FinderPhoneNumber { get; set; }
    public DateTime DateSubmitted { get; set; } = DateTime.UtcNow;
}
