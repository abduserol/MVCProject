namespace PatientsProject.Core.App.Models
{
    public abstract class Response
    {
        public virtual int  Id{ get; set; }
        public virtual string Guid { get; set; }

        protected Response(int id)
        {
            Id = id;
        }

        protected Response()
        {
            
        }
    }
}