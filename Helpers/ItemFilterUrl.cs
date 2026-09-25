using System.Globalization;
using Microsoft.AspNetCore.WebUtilities;
using Reklaim_frontend.Models;

namespace Reklaim_frontend.Helpers;

/// <summary>
/// Reads and writes the feed's filter state to the query string, so a filtered feed
/// survives refresh and the back button and can be shared as a link.
/// e.g. ?q=bottle&amp;category=Bottles&amp;from=2026-09-01&amp;sort=oldest
/// </summary>
public static class ItemFilterUrl
{
    private const string DateFormat = "yyyy-MM-dd";

    // All keys this helper owns. Other query parameters on the page are left alone.
    public static readonly string[] Keys = ["q", "category", "location", "status", "from", "to", "sort"];

    public static ItemFilter Read(string uri)
    {
        var query = QueryHelpers.ParseQuery(new Uri(uri).Query);
        string? Get(string key) => query.TryGetValue(key, out var v) && !string.IsNullOrWhiteSpace(v) ? v.ToString() : null;

        return new ItemFilter
        {
            Search = Get("q"),
            Category = Get("category"),
            Location = Get("location"),
            Status = Get("status"),
            DateFrom = ParseDate(Get("from")),
            DateTo = ParseDate(Get("to")),
            Sort = Enum.TryParse<ItemSort>(Get("sort"), ignoreCase: true, out var sort) ? sort : ItemSort.Newest,
        };
    }

    /// <summary>Values for NavigationManager.GetUriWithQueryParameters. Null removes the key.</summary>
    public static Dictionary<string, object?> ToParameters(ItemFilter filter) => new()
    {
        ["q"] = NullIfBlank(filter.Search?.Trim()),
        ["category"] = NullIfBlank(filter.Category),
        ["location"] = NullIfBlank(filter.Location),
        ["status"] = NullIfBlank(filter.Status),
        ["from"] = filter.DateFrom?.ToString(DateFormat, CultureInfo.InvariantCulture),
        ["to"] = filter.DateTo?.ToString(DateFormat, CultureInfo.InvariantCulture),
        ["sort"] = filter.Sort == ItemSort.Newest ? null : filter.Sort.ToString().ToLowerInvariant(),
    };

    private static DateTime? ParseDate(string? value) =>
        DateTime.TryParseExact(value, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            ? date
            : null;

    private static string? NullIfBlank(string? value) => string.IsNullOrWhiteSpace(value) ? null : value;
}
