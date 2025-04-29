using System.ComponentModel.DataAnnotations;

public class ProfileViewModel
{
    [Required]
    [Display(Name = "Tipo di Utente")]
    public string? UserType { get; set; } // "Client" o "Professional"

    // Campi comuni
    [Required]
    [Display(Name = "Nome")]
    public string? FirstName { get; set; }

    [Required]
    [Display(Name = "Cognome")]
    public string? LastName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Data di Nascita")]
    public DateTime DateOfBirth { get; set; }

    [Display(Name = "Foto Profilo")]
    public IFormFile ProfilePicture { get; set; }

    // Campi specifici per cliente
    [Display(Name = "Stile Preferito")]
    public string? PreferredStyle { get; set; }

    [Display(Name = "Tipo di Capelli")]
    public string? HairType { get; set; }

    // Campi specifici per professionista
    [Display(Name = "Specializzazione")]
    public string? Specialization { get; set; }

    [Display(Name = "Anni di Esperienza")]
    public int YearsOfExperience { get; set; }

    [Display(Name = "Biografia")]
    public string? Bio { get; set; }

    [Display(Name = "Diploma/Certificato")]
    public IFormFile DiplomaFile { get; set; }
}