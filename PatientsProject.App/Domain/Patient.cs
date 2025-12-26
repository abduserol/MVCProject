using System.ComponentModel.DataAnnotations.Schema;
using PatientsProject.Core.App.Domain;

namespace PatientsProject.App.Domain
{
    public class Patient : Entity
    {
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        
        public int? UserId { get; set; }
        public User User { get; set; }

        public int? GroupId { get; set; }
        public Group Group { get; set; }
        
        public List<DoctorPatient> DoctorPatients { get; set; } = new List<DoctorPatient>();

        [NotMapped]
        public List<int> DoctorIds
        {
            get => DoctorPatients.Select(dp => dp.DoctorId).ToList();
            set => DoctorPatients = value?.Select(doctorId => new DoctorPatient() { DoctorId = doctorId }).ToList();
        }
    }
}