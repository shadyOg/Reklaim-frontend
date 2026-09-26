using Reklaim_frontend.Models;

namespace Reklaim_frontend.Services;

/// <summary>
/// Placeholder listings for building and demoing the feed until PostService talks to the API.
/// Dates are relative to today so the date filters always have something to match.
/// </summary>
public static class SampleItems
{
    public static IReadOnlyList<ItemPostDto> All { get; } = Build();

    private static List<ItemPostDto> Build()
    {
        var today = DateTime.Today;

        return
        [
            new() { Id = 1, Title = "Blue water bottle", Description = "Hydro Flask with a NASA sticker on the side.", Category = "Bottles", LocationFound = "Library", PostType = "Found", Status = "Open", DatePosted = today.AddDays(-1), UserId = 3 },
            new() { Id = 2, Title = "Student ID card", Description = "ID card for a Level 200 Computer Science student.", Category = "ID & Cards", LocationFound = "Cafeteria", PostType = "Found", Status = "Pending", DatePosted = today.AddDays(-2), UserId = 5 },
            new() { Id = 3, Title = "Black laptop bag", Description = "Dell backpack with a charger and two notebooks inside. Lost after the evening lecture.", Category = "Bags", LocationFound = "Engineering Block", PostType = "Lost", Status = "Open", DatePosted = today.AddDays(-3), UserId = 2 },
            new() { Id = 4, Title = "Silver keychain", Description = "Three keys on a silver ring with a small football charm.", Category = "Keys", LocationFound = "Sports Complex", PostType = "Found", Status = "Claimed", DatePosted = today.AddDays(-6), UserId = 4 },
            new() { Id = 5, Title = "AirPods Pro case", Description = "White charging case only, small scratch on the lid.", Category = "Electronics", LocationFound = "Library", PostType = "Lost", Status = "Open", DatePosted = today, UserId = 6 },
            new() { Id = 6, Title = "Grey hoodie", Description = "University hoodie, size M, left on a bench.", Category = "Clothing", LocationFound = "Sports Complex", PostType = "Found", Status = "Open", DatePosted = today.AddDays(-4), UserId = 7 },
            new() { Id = 7, Title = "Calculus textbook", Description = "Stewart Calculus, 8th edition, name written inside the cover.", Category = "Books & Stationery", LocationFound = "Lecture Hall 2", PostType = "Lost", Status = "Open", DatePosted = today.AddDays(-9), UserId = 8 },
            new() { Id = 8, Title = "Hostel room key", Description = "Single brass key with a blue tag marked A-14.", Category = "Keys", LocationFound = "Hostel A", PostType = "Lost", Status = "Pending", DatePosted = today.AddDays(-2), UserId = 9 },
            new() { Id = 9, Title = "Samsung phone", Description = "Black Galaxy A54 in a clear case, screen locked.", Category = "Electronics", LocationFound = "Cafeteria", PostType = "Found", Status = "Open", DatePosted = today.AddDays(-1), UserId = 10 },
            new() { Id = 10, Title = "Red umbrella", Description = "Compact folding umbrella with a wooden handle.", Category = "Other", LocationFound = "Engineering Block", PostType = "Found", Status = "Claimed", DatePosted = today.AddDays(-14), UserId = 11 },
            new() { Id = 11, Title = "Green water bottle", Description = "Plastic sports bottle, name tag partly peeled off.", Category = "Bottles", LocationFound = "Sports Complex", PostType = "Lost", Status = "Open", DatePosted = today.AddDays(-5), UserId = 12 },
            new() { Id = 12, Title = "Scientific calculator", Description = "Casio fx-991ES with initials scratched on the back.", Category = "Electronics", LocationFound = "Lecture Hall 2", PostType = "Found", Status = "Open", DatePosted = today.AddDays(-7), UserId = 13 },
        ];
    }
}
