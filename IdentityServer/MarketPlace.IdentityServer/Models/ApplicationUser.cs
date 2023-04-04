using Microsoft.AspNetCore.Identity;

namespace MarketPlace.IdentityServer.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirsName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
    }
}
