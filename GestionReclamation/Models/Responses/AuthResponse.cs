namespace GestionReclamation.Models.Responses;

public class AuthResponse
{
    public string Token { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public DateTime ExpiresAt { get; set; }
}