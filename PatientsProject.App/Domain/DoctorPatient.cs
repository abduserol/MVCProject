using PatientsProject.Core.App.Domain;

namespace PatientsProject.App.Domain;

public class DoctorPatient : Entity
{
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    
    public int PatientId { get; set; }
    public Patient Patient { get; set; }
}