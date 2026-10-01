using System.Net.Http.Headers;
using System.Net.Http.Json;
using Reklaim_frontend.Models;

namespace Reklaim_frontend.Services;

/// <summary>
/// Handles calls to the Hub API for item posts.
/// Depends on AuthService for token retrieval so we can set the Authorization header.
/// </summary>
public class PostService
{
    private readonly HttpClient _http;
    private readonly AuthService _auth;

    public PostService(HttpClient http, AuthService auth)
    {
        _http = http;
        _auth = auth;
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
        return items ?? new List<ItemPostDto>();
    }

    public async Task<ItemPostDto?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<ItemPostDto>($"api/itemposts/{id}");
    }

    public async Task<int?> CreateAsync(Reklaim_frontend.Models.CreateItemRequest model, Stream? imageStream, string? imageFileName)
    {
        await AddAuthHeaderAsync();

        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(model.Title ?? string.Empty), "Title");
        content.Add(new StringContent(model.Description ?? string.Empty), "Description");
        content.Add(new StringContent(model.LocationFound ?? string.Empty), "LocationFound");
        content.Add(new StringContent(model.Category ?? string.Empty), "Category");
        content.Add(new StringContent(model.PostType ?? string.Empty), "PostType");

        if (imageStream != null && imageFileName != null)
        {
            var streamContent = new StreamContent(imageStream);
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            content.Add(streamContent, "Image", imageFileName);
        }

        var response = await _http.PostAsync("api/itemposts", content);
        if (!response.IsSuccessStatusCode)
        {
            return null;
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
}
