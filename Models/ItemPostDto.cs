namespace Reklaim_frontend.Models;

/// <summary>
/// A lost/found listing. Mirrors the ItemPost contract in the README.
/// </summary>
public class ItemPostDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public string? LocationFound { get; set; }
    public string? Category { get; set; }

    // "Lost" or "Found"
    public string PostType { get; set; } = "";

    // Single-image compatibility property (some components still use this)
    public string? ImageUrl { get; set; }

    // Prefer this for multiple images: gallery or upload results
    public List<string> ImageUrls { get; set; } = new();

    public DateTime DatePosted { get; set; } = DateTime.UtcNow;

    // e.g. "Open", "Pending", "Claimed"
    public string Status { get; set; } = "";

    public int UserId { get; set; }

    // Additional metadata (non-breaking additions)
    public string? Condition { get; set; }
    public decimal? EstimatedValue { get; set; }
    public string? OwnerName { get; set; }
}
