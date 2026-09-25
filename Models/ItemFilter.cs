namespace Reklaim_frontend.Models;

/// <summary>
/// Filter state for the listing feed. Null/empty values mean "any".
/// Search and Sort live here too so the whole feed state can be saved and restored together,
/// but they are not counted as panel filters (IsEmpty, ActiveCount, Reset).
/// </summary>
public class ItemFilter
{
    public string? Search { get; set; }
    public ItemSort Sort { get; set; } = ItemSort.Newest;

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

    public bool HasSearch => !string.IsNullOrWhiteSpace(Search);

    /// <summary>Clears the panel filters. Search and sort are kept.</summary>
    public void Reset()
    {
        Category = null;
        Location = null;
        Status = null;
        DateFrom = null;
        DateTo = null;
    }

    /// <summary>Clears the panel filters and the search text. Sort is kept.</summary>
    public void ResetAll()
    {
        Reset();
        Search = null;
    }
}
