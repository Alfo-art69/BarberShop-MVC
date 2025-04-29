namespace BarberShop.Services
{
    public interface IUserService
    {
        Task<ServiceResult> RegisterAsync(RegisterViewModel model);
        Task<ServiceResult> CompleteProfileAsync(string userId, ProfileViewModel model, IFormFile diplomaFile);
        Task<UserProfileDto> GetUserProfileAsync(string userId);
        Task<ServiceResult> UpdateProfileAsync(string userId, ProfileViewModel model);
        Task<IEnumerable<ProfessionalDto>> GetAllProfessionalsAsync();
    }

    public class ServiceResult
    {
        public bool Success { get; set; }
        public required IEnumerable<string> Errors { get; set; }
    }
}
