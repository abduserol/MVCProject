using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientsProject.App.Domain;
using PatientsProject.App.Enums;

namespace PatientsProject.Mvc.Controllers;

public class DatabaseController : Controller
    {
        private readonly Db _db;

        public DatabaseController(Db db)
        {
            _db = db;
        }

       [Route("SeedDb")]
        public IActionResult Seed()
        {
            // 1. TEMİZLİK
            _db.DoctorPatients.RemoveRange(_db.DoctorPatients.ToList());
            _db.UserRoles.RemoveRange(_db.UserRoles.ToList());
            _db.Patients.RemoveRange(_db.Patients.ToList());
            _db.Doctors.RemoveRange(_db.Doctors.ToList());
            _db.Users.RemoveRange(_db.Users.ToList());
            _db.Branches.RemoveRange(_db.Branches.ToList());
            _db.Groups.RemoveRange(_db.Groups.ToList());
            _db.Roles.RemoveRange(_db.Roles.ToList());
            
            _db.SaveChanges();

            // 2. ID SIFIRLAMA
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='DoctorPatients'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='UserRoles'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Patients'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Doctors'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Users'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Branches'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Groups'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Roles'");

            // 3. VERİ EKLEME

            // --- A. ROLES ---
            var roleAdmin = new Role { Guid = Guid.NewGuid().ToString(), Name = "Admin" };
            var roleDoctor = new Role { Guid = Guid.NewGuid().ToString(), Name = "Doctor" };
            var rolePatient = new Role { Guid = Guid.NewGuid().ToString(), Name = "Patient" };
            _db.Roles.AddRange(roleAdmin, roleDoctor, rolePatient);

            // --- B. GROUPS ---
            var groupAdmin = new Group { Guid = Guid.NewGuid().ToString(), Title = "Management" };
            var groupHospital = new Group { Guid = Guid.NewGuid().ToString(), Title = "Hospital Staff" };
            var groupPatient = new Group { Guid = Guid.NewGuid().ToString(), Title = "Patients" };
            _db.Groups.AddRange(groupAdmin, groupHospital, groupPatient);

            // --- C. BRANCHES ---
            var branchOrtho = new Branch { Guid = Guid.NewGuid().ToString(), Title = "Orthodontics" };
            var branchCardio = new Branch { Guid = Guid.NewGuid().ToString(), Title = "Cardiology" };
            var branchDentist = new Branch { Guid = Guid.NewGuid().ToString(), Title = "General Dentistry" };
            _db.Branches.AddRange(branchOrtho, branchCardio, branchDentist);

            _db.SaveChanges(); 

            // --- D. USERS ---
            var userAdmin = new User
            {
                Guid = Guid.NewGuid().ToString(),
                UserName = "admin",
                Password = "1234",
                FirstName = "System",
                LastName = "Admin",
                Gender = Genders.Male,
                IsActive = true,
                RegistrationDate = DateTime.Now,
                GroupId = groupAdmin.Id,
                Address = "Merkez, Ankara",
                // DÜZELTME BURADA: UserRole için de Guid ekledik
                UserRoles = new List<UserRole> { 
                    new UserRole { Guid = Guid.NewGuid().ToString(), RoleId = roleAdmin.Id } 
                }
            };

            var userDoctor = new User
            {
                Guid = Guid.NewGuid().ToString(),
                UserName = "doctor1",
                Password = "1234",
                FirstName = "Ali",
                LastName = "Veli",
                Gender = Genders.Male,
                IsActive = true,
                RegistrationDate = DateTime.Now,
                GroupId = groupHospital.Id,
                Address = "Hastane Lojmanı, İstanbul",
                // Doctor rolü atandı - hasta ekleyebilir
                UserRoles = new List<UserRole> { 
                    new UserRole { Guid = Guid.NewGuid().ToString(), RoleId = roleDoctor.Id } 
                }
            };

            var userPatient = new User
            {
                Guid = Guid.NewGuid().ToString(),
                UserName = "patient1",
                Password = "1234",
                FirstName = "Ayşe",
                LastName = "Yılmaz",
                Gender = Genders.Female,
                IsActive = true,
                RegistrationDate = DateTime.Now,
                GroupId = groupPatient.Id,
                Address = "Kızılay, Ankara",
                // Patient rolü atandı
                UserRoles = new List<UserRole> { 
                    new UserRole { Guid = Guid.NewGuid().ToString(), RoleId = rolePatient.Id } 
                }
            };

            _db.Users.AddRange(userAdmin, userDoctor, userPatient);
            _db.SaveChanges();

            // --- E. DOCTORS ---
            var doctor1 = new Doctor
            {
                Guid = Guid.NewGuid().ToString(),
                BranchId = branchOrtho.Id,
                UserId = userDoctor.Id,
                GroupId = groupHospital.Id
            };

            var doctor2 = new Doctor
            {
                Guid = Guid.NewGuid().ToString(),
                BranchId = branchCardio.Id,
                GroupId = groupHospital.Id
            };

            _db.Doctors.AddRange(doctor1, doctor2);
            _db.SaveChanges();

            // --- F. PATIENTS ---
            var patient1 = new Patient
            {
                Guid = Guid.NewGuid().ToString(),
                Height = 165.5m,
                Weight = 58.0m,
                UserId = userPatient.Id,
                GroupId = groupPatient.Id
            };

            var patient2 = new Patient
            {
                Guid = Guid.NewGuid().ToString(),
                Height = 180.0m,
                Weight = 85.0m,
                GroupId = groupPatient.Id
            };

            _db.Patients.AddRange(patient1, patient2);
            _db.SaveChanges();

            // --- G. DOCTOR - PATIENT RELATIONS ---
            // DoctorPatient için de Guid ekledik (Eğer önceki adımda eklemediysen hata vermesin diye)
            var dp1 = new DoctorPatient { Guid = Guid.NewGuid().ToString(), DoctorId = doctor1.Id, PatientId = patient1.Id };
            var dp2 = new DoctorPatient { Guid = Guid.NewGuid().ToString(), DoctorId = doctor1.Id, PatientId = patient2.Id };
            var dp3 = new DoctorPatient { Guid = Guid.NewGuid().ToString(), DoctorId = doctor2.Id, PatientId = patient1.Id };

            _db.DoctorPatients.AddRange(dp1, dp2, dp3);
            _db.SaveChanges();

            return Content("<label style='color:green; font-size: 20px;'><b>Database seed successful!</b><br>" +
                           "Users: admin, doctor1, patient1 (Password: 1234)</label>", "text/html", Encoding.UTF8);
        }
    }