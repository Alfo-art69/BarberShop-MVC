public class Professional
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string? Specialization { get; set; }
    public int YearsOfExperience { get; set; }
    public string? Bio { get; set; }
    public string? DiplomaPath { get; set; }
    public bool IsVerified { get; set; } = false;
    public string? Portfolio { get; set; }
}