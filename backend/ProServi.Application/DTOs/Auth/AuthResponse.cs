namespace ProServi.Application.DTOs.Auth;

public class AuthResponse
{
    public string Token { get; set; }
    public UserData User { get; set; }
}

public class UserData
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }
    public string ProfilePhotoUrl { get; set; }
}
