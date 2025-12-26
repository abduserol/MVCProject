using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PatientsProject.App.Enums;
using PatientsProject.Core.App.Models;

namespace PatientsProject.App.Models;

public class UserRequest : Request
{
    [Required(ErrorMessage = "{0} is required!")]
    [StringLength(30)]
    [DisplayName("User Name")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "{0} is required!")]
    [StringLength(15)]
    [DisplayName("Password")]
    public string Password { get; set; }

    [StringLength(50)]
    [DisplayName("First Name")]
    public string FirstName { get; set; }

    [StringLength(50)]
    [DisplayName("Last Name")]
    public string LastName { get; set; }

    public Genders Gender { get; set; }

    [DisplayName("Birth Date")]
    public DateTime? BirthDate { get; set; }

    public decimal Score { get; set; }

    [DisplayName("Active")]
    public bool IsActive { get; set; }

    public string Address { get; set; }

    // İlişkiler
    [Required(ErrorMessage = "{0} is required!")]
    [DisplayName("Group")]
    public int? GroupId { get; set; }

    // Çoka-Çok Rol İlişkisi (Multi-Select ListBox için)
    [DisplayName("Roles")]
    public List<int> RoleIds { get; set; } = new List<int>();
}