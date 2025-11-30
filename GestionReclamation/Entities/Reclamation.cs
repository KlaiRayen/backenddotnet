namespace GestionReclamation.Entities;
public class Reclamation
{
    public int Id { get; set; }
    public string Titre { get; set; }
    public string Description { get; set; }

    public string Reponse { get; set; }
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;
    public DateTime? DateReponse { get; set; }

    // FK vers l'utilisateur
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public string Statut { get; set; } = "EnAttente"; // EnAttente / Resolue
}
