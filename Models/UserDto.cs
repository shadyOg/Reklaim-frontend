namespace Reklaim_frontend.Models;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string StudentEmail { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime DateJoined { get; set; }
}