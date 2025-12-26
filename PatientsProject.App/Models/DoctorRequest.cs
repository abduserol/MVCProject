using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PatientsProject.Core.App.Models;

namespace PatientsProject.App.Models;

public class DoctorRequest : Request
{
    [DisplayName("Branch")]
    [Required(ErrorMessage = "{0} is required!")]
    public int? BranchId { get; set; }

    [DisplayName("User ID")]
    public int? UserId { get; set; }

    [DisplayName("Group ID")]
    public int? GroupId { get; set; }
}