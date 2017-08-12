using System.ComponentModel.DataAnnotations;

namespace GitHub.Models
{
    public class UserSearchViewModel
    {
        [Required]
        public string UserName { get; set; }

        public bool NoResults { get; set; }
    }
}