using System.ComponentModel;
using PatientsProject.Core.App.Models;

namespace PatientsProject.App.Models;

public class BranchResponse : Response
{
    [DisplayName("Branch Title")]
    public string Title { get; set; }

    [DisplayName("Doctor Count")]
    public int DoctorCount { get; set; }

    [DisplayName("Doctors")]
    public string Doctors { get; set; }
}