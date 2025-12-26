namespace PatientsProject.Core.App.Models
{
    public class CommandResponse : Response
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }

        public CommandResponse(bool isSuccessful, string message = "", int id =0): base(id)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }
    }
}