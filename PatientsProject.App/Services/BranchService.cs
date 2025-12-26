using Microsoft.EntityFrameworkCore;
using PatientsProject.App.Domain;
using PatientsProject.App.Models;
using PatientsProject.Core.App.Models;
using PatientsProject.Core.App.Services;
using PatientsProject.Core.App.Services.MVC;

namespace PatientsProject.App.Services;

public class BranchService : Service<Branch>, IService<BranchRequest, BranchResponse>
{
    public BranchService(DbContext db) : base(db)
    {
    }

    protected override IQueryable<Branch> Query(bool isNoTracking = true)
    {
        return base.Query(isNoTracking)
            .Include(b => b.Doctors) 
            .OrderBy(b => b.Title);
    }

    public CommandResponse Create(BranchRequest request)
    {
        if (Query().Any(b => b.Title.ToLower() == request.Title.Trim().ToLower()))
            return Error("Branch with the same title exists!");

        var entity = new Branch
        {
            Title = request.Title.Trim()
        };

        Create(entity);

        return Success("Branch created successfully.", entity.Id);
    }

    public CommandResponse Update(BranchRequest request)
    {
        if (Query().Any(b => b.Id != request.Id && b.Title.ToLower() == request.Title.Trim().ToLower()))
            return Error("Branch with the same title exists!");

        var entity = Query(false).SingleOrDefault(b => b.Id == request.Id);
        if (entity is null)
            return Error("Branch not found!");

        entity.Title = request.Title.Trim();

        Update(entity);

        return Success("Branch updated successfully.", entity.Id);
    }

    public CommandResponse Delete(int id)
    {
        var entity = Query(false).SingleOrDefault(b => b.Id == id);
        if (entity is null)
            return Error("Branch not found!");

        if (entity.Doctors.Count > 0)
            return Error("Branch cannot be deleted because it has related doctors!");

        Delete(entity);

        return Success("Branch deleted successfully.", entity.Id);
    }

    public BranchRequest Edit(int id)
    {
        var entity = Query().SingleOrDefault(b => b.Id == id);
        if (entity is null) return null;

        return new BranchRequest
        {
            Id = entity.Id,
            Title = entity.Title
        };
    }

    public BranchResponse Item(int id)
    {
        var entity = Query().SingleOrDefault(b => b.Id == id);
        if (entity is null) return null;

        return new BranchResponse
        {
            Id = entity.Id,
            Guid = entity.Guid,
            Title = entity.Title,
            
            DoctorCount = entity.Doctors.Count,
            Doctors = string.Join("<br>", entity.Doctors.Select(d => "Doctor ID: " + d.Id)) 
        };
    }

    public List<BranchResponse> List()
    {
        return Query().Select(entity => new BranchResponse
        {
            Id = entity.Id,
            Guid = entity.Guid,
            Title = entity.Title,
            DoctorCount = entity.Doctors.Count,
            Doctors = string.Join("<br>", entity.Doctors.Select(d => "Doctor ID: " + d.Id))
        }).ToList();
    }
}