using BarberShop.Services;
using Microsoft.AspNetCore.Identity;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IWebHostEnvironment _env;

    public UserService(ApplicationDbContext context, UserManager<User> userManager, IWebHostEnvironment env)
    {
        _context = context;
        _userManager = userManager;
        _env = env;
    }

    public async Task<ServiceResult> CompleteProfileAsync(string userId, ProfileViewModel model, IFormFile diplomaFile)
    {
        var errors = new List<string>();
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            errors.Add("Utente non trovato.");
            return new ServiceResult { Success = false, Errors = errors };
        }

        // Aggiorna i campi base
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.DateOfBirth = model.DateOfBirth;
        user.UserType = model.UserType;

        // Gestione immagine profilo
        if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
        {
            user.ProfilePicture = await SaveFileAsync(model.ProfilePicture, "profile-pictures");
        }

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            errors.AddRange(result.Errors.Select(e => e.Description));
            return new ServiceResult { Success = false, Errors = errors };
        }

        // Crea profilo specifico in base al tipo
        if (model.UserType == "Client")
        {
            var client = new Client
            {
                UserId = userId,
                PreferredStyle = model.PreferredStyle,
                HairType = model.HairType
            };
            _context.Clients.Add(client);
        }
        else if (model.UserType == "Professional")
        {
            var diplomaPath = "";
            if (diplomaFile != null && diplomaFile.Length > 0)
            {
                diplomaPath = await SaveFileAsync(diplomaFile, "diplomas");
            }

            var professional = new Professional
            {
                UserId = userId,
                Specialization = model.Specialization,
                YearsOfExperience = model.YearsOfExperience ?? 0,
                Bio = model.Bio,
                DiplomaPath = diplomaPath
            };
            _context.Professionals.Add(professional);
        }

        try
        {
            await _context.SaveChangesAsync();
            return new ServiceResult { Success = true };
        }
        catch (Exception ex)
        {
            errors.Add($"Errore durante il salvataggio: {ex.Message}");
            return new ServiceResult { Success = false, Errors = errors };
        }
    }

    private async Task<string> SaveFileAsync(IFormFile file, string folder)
    {
        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", folder);
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return $"/uploads/{folder}/{uniqueFileName}";
    }

    // Altri metodi dell'interfaccia...
}