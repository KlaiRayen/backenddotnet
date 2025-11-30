namespace GestionReclamation.Services;

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using GestionReclamation.Entities;
using GestionReclamation.Models.Requests;
using GestionReclamation.Models.Responses;
using GestionReclamation.Services.Interfaces;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        var response = new ApiResponse<AuthResponse>();

        // Vérifier si l'email existe déjà
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            response.Success = false;
            response.Message = "L'email est déjà utilisé";
            response.Errors.Add("Email déjà existant");
            return response;
        }

        // Vérifier les mots de passe
        if (request.Password != request.ConfirmPassword)
        {
            response.Success = false;
            response.Message = "Les mots de passe ne correspondent pas";
            response.Errors.Add("Password mismatch");
            return response;
        }

        // Créer l'utilisateur
        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            response.Success = false;
            response.Message = "Erreur lors de la création du compte";
            response.Errors = result.Errors.Select(e => e.Description).ToList();
            return response;
        }

        // Assigner le rôle User par défaut
        await _userManager.AddToRoleAsync(user, "User");

        // Générer le token
        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user, roles);

        response.Success = true;
        response.Message = "Inscription réussie";
        response.Data = new AuthResponse
        {
            Token = token,
            Email = user.Email,
            Roles = roles.ToList(),
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        };

        return response;
    }

    public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var response = new ApiResponse<AuthResponse>();

        // Trouver l'utilisateur
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            response.Success = false;
            response.Message = "Email ou mot de passe incorrect";
            response.Errors.Add("Invalid credentials");
            return response;
        }

        // Vérifier le mot de passe
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            response.Success = false;
            response.Message = "Email ou mot de passe incorrect";
            response.Errors.Add("Invalid credentials");
            return response;
        }

        // Générer le token
        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user, roles);

        response.Success = true;
        response.Message = "Connexion réussie";
        response.Data = new AuthResponse
        {
            Token = token,
            Email = user.Email,
            Roles = roles.ToList(),
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        };

        return response;
    }
}