using Microsoft.AspNetCore.Identity;

namespace Mindwell.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add custom properties if needed (e.g., FullName)
        public string FullName { get; set; }
    }
}
