namespace GestionReclamation.Services.Interfaces;
using GestionReclamation.Models.DTOs;
using GestionReclamation.Models.Responses;

public interface IUserService
{
    Task<ApiResponse<List<UserDto>>> GetAllUsersAsync();
    Task<ApiResponse<UserDto>> GetUserByIdAsync(string userId);
}