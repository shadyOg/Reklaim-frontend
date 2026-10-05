using Reklaim_frontend.Models;

namespace Reklaim_frontend.Services;

public interface IPostService
{
    Task<List<ItemPostDto>> GetAllAsync();
    Task<ItemPostDto?> GetByIdAsync(int id);
    Task<int?> CreateAsync(CreateItemRequest model, Stream? imageStream, string? imageFileName);
    Task<bool> UpdateStatusAsync(int id, string status);
    Task<bool> SubmitClaimAsync(int id, Models.ClaimRequestDto claim);
    Task<bool> DeleteAsync(int id);
}
