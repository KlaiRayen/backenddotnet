namespace GestionReclamation.Services.Interfaces;

using GestionReclamation.Models.DTOs;
using GestionReclamation.Models.Requests;

public interface IReclamationService
{
    Task<ReclamationDto> CreateReclamationAsync(string userId, CreateReclamationRequest request);
    Task<List<ReclamationDto>> GetUserReclamationsAsync(string userId);
    Task<List<ReclamationDto>> GetAllReclamationsAsync();
    Task<ReclamationDto> GetReclamationByIdAsync(int id);
    Task<ReclamationDto> ReplyToReclamationAsync(ReplyReclamationRequest request);
    Task<bool> DeleteReclamationAsync(int id);
}