namespace GestionReclamation.Services;
using AutoMapper;
using GestionReclamation.Entities;
using GestionReclamation.Models.DTOs;
using GestionReclamation.Models.Responses;
using GestionReclamation.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<UserDto>>> GetAllUsersAsync()
    {
        var response = new ApiResponse<List<UserDto>>();

        try
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = roles.ToList()
                };
                userDtos.Add(userDto);
            }

            response.Success = true;
            response.Message = "Utilisateurs récupérés avec succès";
            response.Data = userDtos;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = "Erreur lors de la récupération des utilisateurs";
            response.Errors.Add(ex.Message);
        }

        return response;
    }

    public async Task<ApiResponse<UserDto>> GetUserByIdAsync(string userId)
    {
        var response = new ApiResponse<UserDto>();

        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Utilisateur non trouvé";
                response.Errors.Add("User not found");
                return response;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList()
            };

            response.Success = true;
            response.Message = "Utilisateur récupéré avec succès";
            response.Data = userDto;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = "Erreur lors de la récupération de l'utilisateur";
            response.Errors.Add(ex.Message);
        }

        return response;
    }
}