using System.Net.Http.Headers;
using System.Net.Http.Json;
using Reklaim_frontend.Models;

namespace Reklaim_frontend.Services;

/// <summary>
/// Handles calls to the Hub API for item posts.
/// Depends on AuthService for token retrieval so we can set the Authorization header.
/// </summary>
public class PostService : IPostService
{
    private readonly HttpClient _http;
    private readonly AuthService _auth;
    private readonly ILogger<PostService> _logger;

    public PostService(HttpClient http, AuthService auth, ILogger<PostService> logger)
    {
        _http = http;
        _auth = auth;
        _logger = logger;
    }

    private async Task AddAuthHeaderAsync()
    {
        var token = await _auth.GetTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            _http.DefaultRequestHeaders.Authorization = null;
        }
    }

    public async Task<List<ItemPostDto>> GetAllAsync()
    {
        var items = await _http.GetFromJsonAsync<List<ItemPostDto>>("api/itemposts");
        return (items ?? []).Select(NormalizeItem).ToList();
    }

    public async Task<ItemPostDto?> GetByIdAsync(int id)
    {
        var item = await _http.GetFromJsonAsync<ItemPostDto>($"api/itemposts/{id}");
        return item is null ? null : NormalizeItem(item);
    }

    public async Task<int?> CreateAsync(Reklaim_frontend.Models.CreateItemRequest model, Stream? imageStream, string? imageFileName)
    {
        var token = await _auth.GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new UnauthorizedAccessException("Please sign in before creating a post.");
        }

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(model.Title ?? string.Empty), "Title");
        content.Add(new StringContent(model.Description ?? string.Empty), "Description");
        content.Add(new StringContent(model.LocationFound ?? string.Empty), "LocationFound");
        content.Add(new StringContent(model.Category ?? string.Empty), "Category");
        content.Add(new StringContent(model.PostType ?? string.Empty), "PostType");

        if (imageStream != null && imageFileName != null)
        {
            imageStream.Position = 0;
            var streamContent = new StreamContent(imageStream);
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(GetImageContentType(imageFileName));
            streamContent.Headers.ContentLength = imageStream.Length;
            content.Add(streamContent, "Image", imageFileName);
        }

        _logger.LogInformation("Submitting item post to {Endpoint}; imageAttached={ImageAttached}; imageName={ImageName}",
            new Uri(_http.BaseAddress!, "api/ItemPosts"), imageStream is not null, imageFileName ?? "none");
        var response = await _http.PostAsync("api/itemposts", content);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Item post API returned {StatusCode}: {ResponseBody}", (int)response.StatusCode, error);
            throw new HttpRequestException($"The API rejected the post ({(int)response.StatusCode}): {error}");
        }

        var created = await response.Content.ReadFromJsonAsync<ItemPostDto>();
        return created?.Id;
    }

    public async Task<bool> UpdateStatusAsync(int id, string status)
    {
        await AddAuthHeaderAsync();
        var payload = new { Status = status };
        var response = await _http.PatchAsync($"api/itemposts/{id}/status", JsonContent.Create(payload));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await AddAuthHeaderAsync();
        var response = await _http.DeleteAsync($"api/itemposts/{id}");
        return response.IsSuccessStatusCode;
    }

    public class CreateItemRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? LocationFound { get; set; }
        public string? Category { get; set; }
        public string? PostType { get; set; }
    }

    private ItemPostDto NormalizeItem(ItemPostDto item)
    {
        item.ImageUrl = NormalizeImageUrl(item.ImageUrl);
        item.ImageUrls = item.ImageUrls.Select(NormalizeImageUrl)
            .Where(url => !string.IsNullOrWhiteSpace(url))
            .ToList()!;
        if (item.ImageUrls.Count == 0 && !string.IsNullOrWhiteSpace(item.ImageUrl))
        {
            item.ImageUrls.Add(item.ImageUrl);
        }

        return item;
    }

    private string? NormalizeImageUrl(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl) || Uri.TryCreate(imageUrl, UriKind.Absolute, out _))
        {
            return imageUrl;
        }

        var apiBaseUrl = _http.BaseAddress?.ToString().TrimEnd('/');
        return string.IsNullOrWhiteSpace(apiBaseUrl)
            ? imageUrl
            : $"{apiBaseUrl}/{imageUrl.TrimStart('/')}";
    }

    private static string GetImageContentType(string fileName) =>
        Path.GetExtension(fileName).ToLowerInvariant() switch
        {
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            _ => "application/octet-stream"
        };
}
