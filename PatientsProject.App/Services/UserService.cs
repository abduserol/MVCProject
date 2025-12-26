using Microsoft.EntityFrameworkCore;
using PatientsProject.App.Domain;
using PatientsProject.App.Models;
using PatientsProject.Core.App.Models;
using PatientsProject.Core.App.Services;
using PatientsProject.Core.App.Services.Authentication.MVC;
using PatientsProject.Core.App.Services.MVC;

namespace PatientsProject.App.Services;

public class UserService : Service<User>, IService<UserRequest, UserResponse>
{
    private readonly ICookieAuthService _cookieAuthService;

    public UserService(DbContext db, ICookieAuthService cookieAuthService) : base(db)
    {
        _cookieAuthService = cookieAuthService;
    }

    protected override IQueryable<User> Query(bool isNoTracking = true)
    {
        // Grup ve Roller ile birlikte getiriyoruz
        return base.Query(isNoTracking)
            .Include(u => u.Group)
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .OrderByDescending(u => u.IsActive)
            .ThenBy(u => u.UserName);
    }

    public CommandResponse Create(UserRequest request)
    {
        if (Query().Any(u => u.UserName.ToLower() == request.UserName.Trim().ToLower()))
            return Error("User with the same user name exists!");

        var entity = new User
        {
            UserName = request.UserName.Trim(),
            Password = request.Password,
            FirstName = request.FirstName?.Trim() ?? "",
            LastName = request.LastName?.Trim() ?? "",
            Gender = request.Gender,
            BirthDate = request.BirthDate,
            RegistrationDate = DateTime.Now,
            Score = request.Score,
            IsActive = request.IsActive,
            Address = request.Address?.Trim() ?? "",
            GroupId = request.GroupId,
            
            RoleIds = request.RoleIds 
        };

        Create(entity);
        return Success("User created successfully.", entity.Id);
    }

    public CommandResponse Update(UserRequest request)
    {
        if (Query().Any(u => u.Id != request.Id && u.UserName.ToLower() == request.UserName.Trim().ToLower()))
            return Error("User with the same user name exists!");

        var entity = Query(false).SingleOrDefault(u => u.Id == request.Id);
        if (entity is null) return Error("User not found!");

        Delete(entity.UserRoles.ToList());

        entity.UserName = request.UserName.Trim();
        entity.Password = request.Password;
        entity.FirstName = request.FirstName?.Trim() ?? "";
        entity.LastName = request.LastName?.Trim() ?? "";
        entity.Gender = request.Gender;
        entity.BirthDate = request.BirthDate;
        entity.Score = request.Score;
        entity.IsActive = request.IsActive;
        entity.Address = request.Address?.Trim() ?? "";
        entity.GroupId = request.GroupId;
        
        entity.RoleIds = request.RoleIds;

        Update(entity);
        return Success("User updated successfully.", entity.Id);
    }

    public CommandResponse Delete(int id)
    {
        var entity = Query(false).SingleOrDefault(u => u.Id == id);
        if (entity is null) return Error("User not found!");

        Delete(entity.UserRoles);

        Delete(entity);
        return Success("User deleted successfully.", entity.Id);
    }

    public UserRequest Edit(int id)
    {
        var entity = Query().SingleOrDefault(u => u.Id == id);
        if (entity is null) return null;

        return new UserRequest
        {
            Id = entity.Id,
            UserName = entity.UserName,
            Password = entity.Password,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Gender = entity.Gender,
            BirthDate = entity.BirthDate,
            Score = entity.Score,
            IsActive = entity.IsActive,
            Address = entity.Address,
            GroupId = entity.GroupId,
            
            RoleIds = entity.UserRoles.Select(ur => ur.RoleId).ToList()
        };
    }

    public UserResponse Item(int id)
    {
        var entity = Query().SingleOrDefault(u => u.Id == id);
        if (entity is null) return null;

        return new UserResponse
        {
            Id = entity.Id,
            Guid = entity.Guid,
            UserName = entity.UserName,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Gender = entity.Gender.ToString(),
            
            BirthDate = entity.BirthDate,
            BirthDateF = entity.BirthDate.HasValue ? entity.BirthDate.Value.ToShortDateString() : "",
            
            RegistrationDateF = entity.RegistrationDate.ToShortDateString(),
            Score = entity.Score,
            IsActive = entity.IsActive,
            IsActiveF = entity.IsActive ? "Active" : "Passive",
            Address = entity.Address,
            
            GroupId = entity.GroupId,
            Group = entity.Group?.Title,
            
            RoleIds = entity.UserRoles.Select(ur => ur.RoleId).ToList(),
            Roles = string.Join("<br>", entity.UserRoles.Select(ur => ur.Role.Name))
        };
    }

    public List<UserResponse> List()
    {
        return Query().Select(u => new UserResponse
        {
            Id = u.Id,
            Guid = u.Guid,
            UserName = u.UserName,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Gender = u.Gender.ToString(),
            BirthDateF = u.BirthDate.HasValue ? u.BirthDate.Value.ToShortDateString() : "",
            RegistrationDateF = u.RegistrationDate.ToShortDateString(),
            Score = u.Score,
            IsActive = u.IsActive,
            IsActiveF = u.IsActive ? "Active" : "Passive",
            Group = u.Group.Title,
            Roles = string.Join("<br>", u.UserRoles.Select(ur => ur.Role.Name))
        }).ToList();
    }

    // Authentication Methods
    public async Task<CommandResponse> Login(UserLoginRequest request)
    {
        var entity = Query().SingleOrDefault(
            u => u.UserName == request.UserName
              && u.Password == request.Password
              && u.IsActive);

        if (entity is null)
            return Error("Invalid user name or password!");

        await _cookieAuthService.SignIn(
            entity.Id,
            entity.UserName,
            entity.UserRoles.Select(ur => ur.Role.Name).ToArray(),
            expiration: null,       // Program.cs'teki ExpireTimeSpan kullanılacak
            isPersistent: false);   // Tarayıcı kapanınca oturum kapansın

        return Success("User logged in successfully.", entity.Id);
    }

    public async Task Logout()
    {
        await _cookieAuthService.SignOut();
    }

    public CommandResponse Register(UserRegisterRequest request)
    {
        var roleEntity = Query<Role>().SingleOrDefault(r => r.Name == "Patient");
        if (roleEntity is null)
            return Error("\"Patient\" role not found!");

        return Create(new UserRequest
        {
            UserName = request.UserName,
            Password = request.Password,
            FirstName = request.UserName, // Default to username
            LastName = "",
            IsActive = true,
            Score = 0,
            RoleIds = new List<int> { roleEntity.Id }
        });
    }
}