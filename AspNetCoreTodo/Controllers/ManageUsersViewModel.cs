using AspNetCoreTodo.Services;

namespace AspNetCoreTodo.Controllers
{
    public class ManageUsersViewModel
    {
        public ApplicationUser[] Administrators { get; set; }
        public ApplicationUser[] Everyone { get; set; }
    }
}