using Microsoft.AspNetCore.Identity;

namespace Zooterapp.Web.Data.Entities
{
    public class User: IdentityUser
    {
        public string Document { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string FullName => $"{Name} {LastName}";

        public string FullNameWithDocument => $"{FullName} - {Document}";
    }
}
