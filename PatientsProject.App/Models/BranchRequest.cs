using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PatientsProject.Core.App.Models;

namespace PatientsProject.App.Models;

public class BranchRequest : Request
{
    [Required(ErrorMessage = "{0} is required!")]
    [StringLength(200, ErrorMessage = "{0} must be maximum {1} characters!")]
    [DisplayName("Branch Title")]
    public string Title { get; set; }
}