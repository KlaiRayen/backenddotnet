namespace GestionReclamation.Services;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using GestionReclamation.Data;
using GestionReclamation.Entities;
using GestionReclamation.Models.DTOs;
using GestionReclamation.Models.Requests;
using GestionReclamation.Services.Interfaces;

public class ReclamationService : IReclamationService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ReclamationService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReclamationDto> CreateReclamationAsync(string userId, CreateReclamationRequest request)
    {
        var reclamation = _mapper.Map<Reclamation>(request);
        reclamation.UserId = userId;

        _context.Reclamations.Add(reclamation);
        await _context.SaveChangesAsync();

        return await GetReclamationByIdAsync(reclamation.Id);
    }

    public async Task<List<ReclamationDto>> GetUserReclamationsAsync(string userId)
    {
        var reclamations = await _context.Reclamations
            .Where(r => r.UserId == userId)
            .Include(r => r.User)
            .OrderByDescending(r => r.DateCreation)
            .ToListAsync();

        return _mapper.Map<List<ReclamationDto>>(reclamations);
    }

    public async Task<List<ReclamationDto>> GetAllReclamationsAsync()
    {
        var reclamations = await _context.Reclamations
            .Include(r => r.User)
            .OrderByDescending(r => r.DateCreation)
            .ToListAsync();

        return _mapper.Map<List<ReclamationDto>>(reclamations);
    }

    public async Task<ReclamationDto> GetReclamationByIdAsync(int id)
    {
        var reclamation = await _context.Reclamations
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reclamation == null)
            return null;

        return _mapper.Map<ReclamationDto>(reclamation);
    }

    public async Task<ReclamationDto> ReplyToReclamationAsync(ReplyReclamationRequest request)
    {
        var reclamation = await _context.Reclamations.FindAsync(request.ReclamationId);

        if (reclamation == null)
            return null;

        reclamation.Reponse = request.Reponse;
        reclamation.Statut = "Resolue";
        reclamation.DateReponse = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetReclamationByIdAsync(reclamation.Id);
    }

    public async Task<bool> DeleteReclamationAsync(int id)
    {
        var reclamation = await _context.Reclamations.FindAsync(id);

        if (reclamation == null)
            return false;

        _context.Reclamations.Remove(reclamation);
        await _context.SaveChangesAsync();

        return true;
    }
}