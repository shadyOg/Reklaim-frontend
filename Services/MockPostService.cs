using System.Collections.Concurrent;
using System.Net.Mime;
using Reklaim_frontend.Models;

namespace Reklaim_frontend.Services;

public class MockPostService : IPostService
{
    private readonly List<ItemPostDto> _items;
    private readonly object _lock = new();
    private int _nextId;

    public MockPostService()
    {
        // copy sample items so we can mutate locally
        _items = SampleItems.All.Select(i => new ItemPostDto
        {
            Id = i.Id,
            Title = i.Title,
            Description = i.Description,
            LocationFound = i.LocationFound,
            Category = i.Category,
            PostType = i.PostType,
            ImageUrl = i.ImageUrl,
            ImageUrls = i.ImageUrls ?? new List<string>(),
            DatePosted = i.DatePosted,
            Status = i.Status,
            UserId = i.UserId,
            Condition = i.Condition,
            EstimatedValue = i.EstimatedValue,
            OwnerName = i.OwnerName
        }).ToList();

        _nextId = _items.Any() ? _items.Max(x => x.Id) + 1 : 1;
    }

    public Task<List<ItemPostDto>> GetAllAsync()
    {
        lock (_lock)
        {
            // return a copy to avoid external mutation
            return Task.FromResult(_items.Select(Clone).ToList());
        }
    }

    public Task<ItemPostDto?> GetByIdAsync(int id)
    {
        lock (_lock)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(item is null ? null : Clone(item));
        }
    }

    public async Task<int?> CreateAsync(CreateItemRequest model, Stream? imageStream, string? imageFileName)
    {
        string? imageDataUrl = null;
        if (imageStream != null && imageFileName != null)
        {
            // read stream (cap at 5 MB)
            const int maxBytes = 5 * 1024 * 1024;
            using var ms = new MemoryStream();
            await imageStream.CopyToAsync(ms);
            var bytes = ms.ToArray();
            if (bytes.Length > maxBytes)
            {
                // truncate to maxBytes
                var truncated = new byte[maxBytes];
                Array.Copy(bytes, truncated, maxBytes);
                bytes = truncated;
            }

            var mime = GetMimeTypeFromFileName(imageFileName) ?? MediaTypeNames.Application.Octet;
            var base64 = Convert.ToBase64String(bytes);
            imageDataUrl = $"data:{mime};base64,{base64}";
        }

        ItemPostDto created;
        lock (_lock)
        {
            created = new ItemPostDto
            {
                Id = _nextId++,
                Title = model.Title ?? string.Empty,
                Description = model.Description,
                LocationFound = model.LocationFound,
                Category = model.Category,
                PostType = model.PostType ?? string.Empty,
                ImageUrl = imageDataUrl,
                ImageUrls = imageDataUrl is null ? new List<string>() : new List<string> { imageDataUrl },
                DatePosted = DateTime.UtcNow,
                Status = "Active",
                UserId = 1
            };

            _items.Add(created);
        }

        return created.Id;
    }

    public Task<bool> UpdateStatusAsync(int id, string status)
    {
        lock (_lock)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item is null) return Task.FromResult(false);
            item.Status = status;
            return Task.FromResult(true);
        }
    }

    public Task<bool> DeleteAsync(int id)
    {
        lock (_lock)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item is null) return Task.FromResult(false);
            _items.Remove(item);
            return Task.FromResult(true);
        }
    }

    private static ItemPostDto Clone(ItemPostDto src) => new()
    {
        Id = src.Id,
        Title = src.Title,
        Description = src.Description,
        LocationFound = src.LocationFound,
        Category = src.Category,
        PostType = src.PostType,
        ImageUrl = src.ImageUrl,
        ImageUrls = src.ImageUrls is null ? new List<string>() : new List<string>(src.ImageUrls),
        DatePosted = src.DatePosted,
        Status = src.Status,
        UserId = src.UserId,
        Condition = src.Condition,
        EstimatedValue = src.EstimatedValue,
        OwnerName = src.OwnerName
    };

    private static string? GetMimeTypeFromFileName(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return ext switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".gif" => "image/gif",
            _ => null,
        };
    }
}
