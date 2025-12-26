using System.ComponentModel;
using PatientsProject.Core.App.Models;

namespace PatientsProject.App.Models;

public class GroupResponse : Response
{
    [DisplayName("Group Title")]
    public string Title { get; set; }
}