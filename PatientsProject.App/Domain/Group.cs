using System.ComponentModel.DataAnnotations;
using PatientsProject.Core.App.Domain;

namespace PatientsProject.App.Domain;

public class Group : Entity
{
    [Required, StringLength(100)]
    public string Title { get; set; }
        
    public List<User> Users { get; set; } = new List<User>(); 
}