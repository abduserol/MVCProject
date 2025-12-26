using System.ComponentModel;
using PatientsProject.Core.App.Models;

namespace PatientsProject.App.Models;

public class DoctorResponse : Response
{
    public int BranchId { get; set; }
    public int? UserId { get; set; }
    public int? GroupId { get; set; }

    public string Branch { get; set; }
    public int PatientCount { get; set; }

    [DisplayName("Doctor Name")]
    public string DoctorName { get; set; } 
}