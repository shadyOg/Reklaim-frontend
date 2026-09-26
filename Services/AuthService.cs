using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Reklaim_frontend.Models;

namespace Reklaim_frontend.Services;

/// <summary>
/// Handles login/register calls against the backend API and persists the
/// resulting token/user for the session. Assumes HttpClient's BaseAddress
/// is already set to the API root (see Program.cs registration).
/// </summary>
public class AuthService
{
    private readonly HttpClient _http;
    private readonly ProtectedSessionStorage _sessionStorage;

    public AuthService(HttpClient http, ProtectedSessionStorage sessionStorage)
    {
        _http = http;
        _sessionStorage = sessionStorage;
    }

    public async Task<ApiResult<AuthResponseDto>> LoginAsync(LoginRequestDto request)
    {
        return await PostAuthRequest("api/auth/login", request);
    }

    public async Task<ApiResult<AuthResponseDto>> RegisterAsync(RegisterRequestDto request)
    {
        return await PostAuthRequest("api/auth/register", request);
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        var result = await _sessionStorage.GetAsync<UserDto>("auth_user");
        return result.Success ? result.Value : null;
    }

    public async Task LogoutAsync()
    {
        await _sessionStorage.DeleteAsync("auth_token");
        await _sessionStorage.DeleteAsync("auth_user");
    }

    private async Task<ApiResult<AuthResponseDto>> PostAuthRequest<TRequest>(string endpoint, TRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(endpoint, request);

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessage(response);
                return ApiResult<AuthResponseDto>.Fail(message);
            }

            var data = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            if (data is null)
            {
                return ApiResult<AuthResponseDto>.Fail("Unexpected response from server.");
            }

            await _sessionStorage.SetAsync("auth_token", data.Token);
            await _sessionStorage.SetAsync("auth_user", data.User);

            return ApiResult<AuthResponseDto>.Ok(data);
        }
        catch (Exception)
        {
            return ApiResult<AuthResponseDto>.Fail("Could not reach the server. Please check your connection and try again.");
        }
    }

    private static async Task<string> ReadErrorMessage(HttpResponseMessage response)
    {
        try
        {
            var text = await response.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(text)
                ? $"Request failed ({(int)response.StatusCode})."
                : text;
        }
        catch
        {
            return $"Request failed ({(int)response.StatusCode}).";
        }
    }
}