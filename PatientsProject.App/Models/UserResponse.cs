using System.ComponentModel;
using PatientsProject.Core.App.Models;

namespace PatientsProject.App.Models;
public class UserResponse : Response
{
    [DisplayName("User Name")]
    public string UserName { get; set; }
    public string Password { get; set; }
    [DisplayName("First Name")]
    public string FirstName { get; set; }
    [DisplayName("Last Name")]
    public string LastName { get; set; }
    public string Gender { get; set; } // Enum string dönüşümü
    
    public DateTime? BirthDate { get; set; }
    [DisplayName("Birth Date")]
    public string BirthDateF { get; set; }
    
    [DisplayName("Registration Date")]
    public string RegistrationDateF { get; set; }
    
    public decimal Score { get; set; }
    
    public bool IsActive { get; set; }
    [DisplayName("Active")]
    public string IsActiveF { get; set; }
    
    public string Address { get; set; }

    public int? GroupId { get; set; }
    [DisplayName("Group")]
    public string Group { get; set; }

    [DisplayName("Roles")]
    public List<int> RoleIds { get; set; }
    
    [DisplayName("Roles")]
    public string Roles { get; set; }
}