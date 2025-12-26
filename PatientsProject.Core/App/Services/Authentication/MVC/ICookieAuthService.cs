namespace PatientsProject.Core.App.Services.Authentication.MVC;

public interface ICookieAuthService
{
    Task SignIn(int userId, string userName, string[] userRoleNames, DateTime? expiration = default, bool isPersistent = true);
    Task SignOut();
}
