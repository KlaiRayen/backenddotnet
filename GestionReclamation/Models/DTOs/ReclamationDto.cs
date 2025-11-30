namespace GestionReclamation.Models.DTOs;

public class ReclamationDto
{
    public int Id { get; set; }
    public string Titre { get; set; }
    public string Description { get; set; }
    public string Reponse { get; set; }
    public string Statut { get; set; }
    public string UserId { get; set; }
    public string UserEmail { get; set; }
    public DateTime DateCreation { get; set; }
    public DateTime? DateReponse { get; set; }
}