using System.ComponentModel.DataAnnotations;
using PatientsProject.Core.App.Domain;

namespace PatientsProject.App.Domain
{
    public class Branch : Entity
    {
        [Required,StringLength(200)]
        public string Title { get; set; }

        public List<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}