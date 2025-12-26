using Microsoft.EntityFrameworkCore;
using PatientsProject.App.Domain;
using PatientsProject.App.Models;
using PatientsProject.Core.App.Models;
using PatientsProject.Core.App.Services;
using PatientsProject.Core.App.Services.MVC;

namespace PatientsProject.App.Services;

public class PatientService : Service<Patient>, IService<PatientRequest, PatientResponse>
{
    public PatientService(DbContext db) : base(db)
    {
    }

    protected override IQueryable<Patient> Query(bool isNoTracking = true)
    {
        return base.Query(isNoTracking)
            .Include(p => p.DoctorPatients).ThenInclude(dp => dp.Doctor).ThenInclude(d => d.User)
            .Include(p => p.User)
            .Include(p => p.Group)
            .OrderByDescending(p => p.Id);
    }

    public List<PatientResponse> List()
    {
        return Query().Select(p => new PatientResponse
        {
            Id = p.Id,
        
            // Boy ve Kilo Formatlama
            HeightF = p.Height.HasValue ? p.Height.Value.ToString("N2") + " cm" : "-",
            WeightF = p.Weight.HasValue ? p.Weight.Value.ToString("N2") + " kg" : "-",
        
            UserName = p.User != null ? p.User.UserName : "-",
            Group = p.Group != null ? p.Group.Title : "-"

        }).ToList();
    }

    public PatientResponse Item(int id)
    {
        var entity = Query().SingleOrDefault(p => p.Id == id);
        if (entity is null)
            return null;

        return new PatientResponse
        {
            Id = entity.Id,
            Guid = entity.Guid,
            Height = entity.Height,
            Weight = entity.Weight,

            HeightF = entity.Height.HasValue ? entity.Height.Value.ToString("N2") + " cm" : "-",
            WeightF = entity.Weight.HasValue ? entity.Weight.Value.ToString("N2") + " kg" : "-",

            UserName = entity.User != null ? entity.User.UserName : "-",
            Group = entity.Group != null ? entity.Group.Title : "-",

            Doctors = entity.DoctorPatients.Select(dp => 
                dp.Doctor.User != null ? dp.Doctor.User.UserName : "Doctor #" + dp.Doctor.Id)
                .ToList(),
            DoctorIds = entity.DoctorPatients.Select(dp => dp.DoctorId).ToList()
        };
    }

    public PatientRequest Edit(int id)
    {
        var entity = Query().SingleOrDefault(p => p.Id == id);
        if (entity is null)
            return null;

        return new PatientRequest
        {
            Id = entity.Id,
            Height = entity.Height,
            Weight = entity.Weight,
            UserId = entity.UserId,
            GroupId = entity.GroupId,
            DoctorIds = entity.DoctorPatients.Select(dp => dp.DoctorId).ToList()
        };
    }

    public CommandResponse Create(PatientRequest request)
    {
        // Seçilen doktorların varlık kontrolü
        if (request.DoctorIds != null && request.DoctorIds.Any())
        {
            var doctorIds = request.DoctorIds.Distinct().ToList();
            var existingDoctorCount = Query<Doctor>().Count(d => doctorIds.Contains(d.Id));
            if (existingDoctorCount != doctorIds.Count)
                return Error("One or more selected doctors were not found!");
        }

        var entity = new Patient
        {
            Height = request.Height,
            Weight = request.Weight,
            UserId = request.UserId,
            GroupId = request.GroupId,

            DoctorPatients = (request.DoctorIds ?? new List<int>()).Select(doctorId => new DoctorPatient
            {
                Guid = Guid.NewGuid().ToString(),
                DoctorId = doctorId
            }).ToList()
        };

        Create(entity);

        return Success("Patient created successfully.", entity.Id);
    }

    public CommandResponse Update(PatientRequest request)
    {
        var entity = Query(false).SingleOrDefault(p => p.Id == request.Id);

        if (entity is null)
            return Error("Patient not found!");

        // Seçilen doktorların varlık kontrolü
        if (request.DoctorIds != null && request.DoctorIds.Any())
        {
            var doctorIds = request.DoctorIds.Distinct().ToList();
            var existingDoctorCount = Query<Doctor>().Count(d => doctorIds.Contains(d.Id));
            if (existingDoctorCount != doctorIds.Count)
                return Error("One or more selected doctors were not found!");
        }

        Delete(entity.DoctorPatients);

        entity.Height = request.Height;
        entity.Weight = request.Weight;
        entity.UserId = request.UserId;
        entity.GroupId = request.GroupId;

        entity.DoctorPatients = (request.DoctorIds ?? new List<int>()).Select(doctorId => new DoctorPatient
        {
            Guid = Guid.NewGuid().ToString(),
            PatientId = entity.Id,
            DoctorId = doctorId
        }).ToList();

        Update(entity);

        return Success("Patient updated successfully.", entity.Id);
    }

    public CommandResponse Delete(int id)
    {
        var entity = Query(false).SingleOrDefault(p => p.Id == id);
        if (entity is null)
            return new CommandResponse(false, "Patient not found!");

        Delete(entity.DoctorPatients);

        Delete(entity);

        return new CommandResponse(true, "Patient deleted successfully.", entity.Id);
    }
}