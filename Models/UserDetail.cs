using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergyConnect.Models;

public class UserDetail
{
    [Key]
    public int UserDetailID { get; set; }

    [ForeignKey("User")]
    public string UserID { get; set; } = string.Empty;

    [Required]
    public string FullName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public User? User { get; set; }
}