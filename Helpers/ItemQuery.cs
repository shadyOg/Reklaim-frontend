using Reklaim_frontend.Models;

namespace Reklaim_frontend.Helpers;

/// <summary>
/// Search, filter and sort logic for the listing feed. Pure functions, no UI or HTTP,
/// so the same rules apply wherever items are listed.
/// </summary>
public static class ItemQuery
{
    /// <param name="postType">"Lost" or "Found" to scope the feed, null for both.</param>
    public static List<ItemPostDto> Apply(IEnumerable<ItemPostDto> items, ItemFilter filter, string? postType = null)
    {
        var terms = SplitTerms(filter.Search);

        // Accept a reversed range rather than returning nothing.
        var (from, to) = (filter.DateFrom?.Date, filter.DateTo?.Date);
        if (from > to)
        {
            (from, to) = (to, from);
        }

        var query = items.Where(item =>
            MatchesText(item.PostType, postType)
            && MatchesText(item.Category, filter.Category)
            && MatchesText(item.LocationFound, filter.Location)
            && MatchesText(item.Status, filter.Status)
            && (from is null || item.DatePosted.Date >= from)
            && (to is null || item.DatePosted.Date <= to)
            && MatchesSearch(item, terms));

        return Sort(query, filter.Sort).ToList();
    }

    /// <summary>Distinct, sorted values of a field, for building filter dropdowns from the data.</summary>
    public static List<string> DistinctValues(IEnumerable<ItemPostDto> items, Func<ItemPostDto, string?> selector) =>
        items.Select(selector)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => value!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToList();

    private static IEnumerable<ItemPostDto> Sort(IEnumerable<ItemPostDto> items, ItemSort sort) => sort switch
    {
        ItemSort.Oldest => items.OrderBy(i => i.DatePosted).ThenBy(i => i.Id),
        ItemSort.TitleAz => items.OrderBy(i => i.Title, StringComparer.OrdinalIgnoreCase),
        _ => items.OrderByDescending(i => i.DatePosted).ThenByDescending(i => i.Id),
    };

    // Empty filter value means "any".
    private static bool MatchesText(string? value, string? wanted) =>
        string.IsNullOrWhiteSpace(wanted)
        || string.Equals(value?.Trim(), wanted.Trim(), StringComparison.OrdinalIgnoreCase);

    // Every term must appear somewhere in the item, so "blue bottle library" narrows results.
    private static bool MatchesSearch(ItemPostDto item, string[] terms)
    {
        if (terms.Length == 0)
        {
            return true;
        }

        var haystack = string.Join(' ', item.Title, item.Description, item.Category, item.LocationFound);
        return terms.All(term => haystack.Contains(term, StringComparison.OrdinalIgnoreCase));
    }

    private static string[] SplitTerms(string? search) =>
        string.IsNullOrWhiteSpace(search)
            ? []
            : search.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
