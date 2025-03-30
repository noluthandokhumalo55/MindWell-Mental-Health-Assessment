public class UserModel
{
    public int Id { get; set; }
    public string? FullName { get; set; }  // Nullable
    public string? Email { get; set; }  // Nullable
    public string? Password { get; set; }  // Nullable
    public string? ConfirmPassword { get; set; }  // Nullable
    public string PasswordHash { get; set; } = string.Empty; // Default value
}
