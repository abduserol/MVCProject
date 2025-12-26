using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PatientsProject.Core.App.Models;

namespace PatientsProject.App.Models;

public class RoleRequest : Request
{
    [Required(ErrorMessage = "{0} is required!")]
    [StringLength(25, ErrorMessage = "{0} must be maximum {1} characters!")]
    [DisplayName("Role Name")]
    public string Name { get; set; }
}