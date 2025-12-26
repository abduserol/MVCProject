using System.ComponentModel;
using PatientsProject.Core.App.Models;

namespace PatientsProject.App.Models
{
    public class PatientResponse : Response
    {
        [DisplayName("Height")]
        public decimal? Height { get; set; }

        [DisplayName("Weight")]
        public decimal? Weight { get; set; }

        public List<int> DoctorIds { get; set; }

        [DisplayName("Height")]
        public string HeightF { get; set; }

        [DisplayName("Weight")]
        public string WeightF { get; set; }

        [DisplayName("Doctors")]
        public List<string> Doctors { get; set; }
        public string UserName { get; set; }
        public string Group { get; set; }
    }
}