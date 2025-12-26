using System.ComponentModel;
using PatientsProject.Core.App.Models;

namespace PatientsProject.App.Models;

public class RoleResponse : Response
{
    [DisplayName("Role Name")]
    public string Name { get; set; }

    [DisplayName("User Count")]
    public int UserCount { get; set; }

    public string Users { get; set; }
}