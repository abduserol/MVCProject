using PatientsProject.App.Domain;
using PatientsProject.App.Models;
using PatientsProject.Core.App.Models;
using PatientsProject.Core.App.Services;

namespace PatientsProject.App.Services
{
    [Obsolete("This service is obsolete and will be removed in future versions.")]
    public class PatientObsoleteService : ServiceBase
    {
        private readonly Db _db;

        public PatientObsoleteService(Db db)
        {
            _db = db;
        }

        public IQueryable<PatientResponse> Query()
        {
            List<PatientResponse> x = new List<PatientResponse>();
            var query = _db.Patients.Select(patientEntity => new PatientResponse
            {
                Id = patientEntity.Id,
                Height = patientEntity.Height,
                Weight = patientEntity.Weight,
            });

            return query;
        }

        public PatientRequest Edit(int id)
        {
            var entity = _db.Patients.Find(id);
            if (entity is null)
                return null;

            var request = new PatientRequest
            {
                Id = entity.Id,
                Height = entity.Height,
                Weight = entity.Weight,
            };

            return request;
        }

        public CommandResponse Create(PatientRequest request)
        {
            var entity = new Patient
            {
                Guid = Guid.NewGuid().ToString(),
                Height = request.Height,
                Weight = request.Weight
            };

            _db.Patients.Add(entity);
            _db.SaveChanges();
            return Success("Patient created successfully.", entity.Id);
        }

        public CommandResponse Update(PatientRequest request)
        {
            var entity = _db.Patients.Find(request.Id);
            if(entity is null)
                return Error("Patient not found!");

            entity.Height = request.Height;
            entity.Weight = request.Weight;

            _db.Patients.Update(entity);
            _db.SaveChanges();

            return Success("Patient updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = _db.Patients.Find(id);

            if (entity is null)
                return Error("Patient not found!");

            _db.Patients.Remove(entity);

            _db.SaveChanges();

            return Success("Patient deleted successfully.", entity.Id);
        }
    }
}