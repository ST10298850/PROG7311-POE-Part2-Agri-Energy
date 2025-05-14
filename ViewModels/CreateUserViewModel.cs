using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } = "Farmer"; // Default role for new users

        [Required]
        public string FullName { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string FarmName { get; set; }

        [Required]
        public string FarmType { get; set; }
    }
}