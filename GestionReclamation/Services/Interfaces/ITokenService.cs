namespace GestionReclamation.Services.Interfaces;

using GestionReclamation.Entities;

public interface ITokenService
{
    string GenerateToken(ApplicationUser user, IList<string> roles);
}