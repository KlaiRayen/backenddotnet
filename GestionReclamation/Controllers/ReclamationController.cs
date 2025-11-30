namespace GestionReclamation.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GestionReclamation.Models.Requests;
using GestionReclamation.Models.Responses;
using GestionReclamation.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReclamationController : ControllerBase
{
    private readonly IReclamationService _reclamationService;

    public ReclamationController(IReclamationService reclamationService)
    {
        _reclamationService = reclamationService;
    }

    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateReclamation([FromBody] CreateReclamationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Données invalides",
                Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
            });

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var reclamation = await _reclamationService.CreateReclamationAsync(userId, request);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Réclamation créée avec succès",
            Data = reclamation
        });
    }

    [HttpGet("my-reclamations")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetMyReclamations()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var reclamations = await _reclamationService.GetUserReclamationsAsync(userId);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Réclamations récupérées",
            Data = reclamations
        });
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllReclamations()
    {
        var reclamations = await _reclamationService.GetAllReclamationsAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Toutes les réclamations récupérées",
            Data = reclamations
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReclamationById(int id)
    {
        var reclamation = await _reclamationService.GetReclamationByIdAsync(id);

        if (reclamation == null)
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Réclamation introuvable"
            });

        // Vérifier si l'utilisateur a le droit de voir cette réclamation
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");

        if (reclamation.UserId != userId && !isAdmin)
            return Forbid();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Data = reclamation
        });
    }

    [HttpPost("reply")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ReplyToReclamation([FromBody] ReplyReclamationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Données invalides"
            });

        var reclamation = await _reclamationService.ReplyToReclamationAsync(request);

        if (reclamation == null)
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Réclamation introuvable"
            });

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Réponse envoyée avec succès",
            Data = reclamation
        });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteReclamation(int id)
    {
        var result = await _reclamationService.DeleteReclamationAsync(id);

        if (!result)
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Réclamation introuvable"
            });

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Réclamation supprimée"
        });
    }
}