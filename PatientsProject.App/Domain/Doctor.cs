using System.ComponentModel.DataAnnotations.Schema;
using PatientsProject.Core.App.Domain;

namespace PatientsProject.App.Domain;

public class Doctor : Entity
{
    public int BranchId { get; set; }
    public Branch Branch { get; set; }
    
    public List<DoctorPatient> DoctorPatients { get; set; } = new List<DoctorPatient>();

    [NotMapped]
    public List<int> PatientIds
    {
        get => DoctorPatients.Select(dp => dp.PatientId).ToList();
        set => DoctorPatients = value?.Select(patientId => new DoctorPatient() { PatientId = patientId }).ToList();
    }

    public int? UserId { get; set; }
    public User User { get; set; }

    public int? GroupId { get; set; }
    public Group Group { get; set; }
}