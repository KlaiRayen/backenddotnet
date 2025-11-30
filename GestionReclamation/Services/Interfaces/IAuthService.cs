namespace GestionReclamation.Services.Interfaces;

using GestionReclamation.Models.Requests;
using GestionReclamation.Models.Responses;

public interface IAuthService
{
    Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request);
}