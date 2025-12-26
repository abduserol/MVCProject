using System.ComponentModel;
using PatientsProject.Core.App.Models;
using System.ComponentModel.DataAnnotations;

namespace PatientsProject.App.Models
{
    public class PatientRequest : Request
    {
        [DisplayName("Height (cm)")]
        [Range(0, 300, ErrorMessage = "{0} must be between {1} and {2}!")]
        public decimal? Height { get; set; }

        [DisplayName("Weight (kg)")]
        [Range(0, 500, ErrorMessage = "{0} must be between {1} and {2}!")]
        public decimal? Weight { get; set; }

        [DisplayName("Doctors")] public List<int> DoctorIds { get; set; } = new List<int>();

        [DisplayName("User Account")] public int? UserId { get; set; }

        [DisplayName("Group")] public int? GroupId { get; set; }
    }
}