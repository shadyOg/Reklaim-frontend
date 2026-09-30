namespace Reklaim_frontend.Models;

public class CreateItemRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? LocationFound { get; set; }
    public string? Category { get; set; }
    public string? PostType { get; set; }
}
