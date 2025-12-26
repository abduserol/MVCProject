namespace PatientsProject.Core.App.Domain
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public string Guid { get; set; }
    }
}