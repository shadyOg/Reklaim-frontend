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

    public string? ImageUrl { get; set; }
    public DateTime DatePosted { get; set; }

    // e.g. "Open", "Pending", "Claimed"
    public string Status { get; set; } = "";

    public int UserId { get; set; }
}
