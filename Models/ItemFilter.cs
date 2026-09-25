namespace Reklaim_frontend.Models;

/// <summary>
/// Filter state for the listing feed. Null/empty values mean "any".
/// </summary>
public class ItemFilter
{
    public string? Category { get; set; }
    public string? Location { get; set; }
    public string? Status { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }

    public bool IsEmpty =>
        string.IsNullOrEmpty(Category)
        && string.IsNullOrEmpty(Location)
        && string.IsNullOrEmpty(Status)
        && DateFrom is null
        && DateTo is null;

    public int ActiveCount =>
        (string.IsNullOrEmpty(Category) ? 0 : 1)
        + (string.IsNullOrEmpty(Location) ? 0 : 1)
        + (string.IsNullOrEmpty(Status) ? 0 : 1)
        + (DateFrom is null && DateTo is null ? 0 : 1);

    public void Reset()
    {
        Category = null;
        Location = null;
        Status = null;
        DateFrom = null;
        DateTo = null;
    }
}
