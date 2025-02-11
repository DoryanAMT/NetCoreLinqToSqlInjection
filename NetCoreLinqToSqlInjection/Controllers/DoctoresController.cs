using Microsoft.AspNetCore.Mvc;
using NetCoreLinqToSqlInjection.Models;
using NetCoreLinqToSqlInjection.Repositories;

namespace NetCoreLinqToSqlInjection.Controllers
{
    public class DoctoresController : Controller
    {
        IRepositoryDoctores repo;
        public DoctoresController(IRepositoryDoctores repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()

        {
            ViewData["ESPECIALIDADES"] = this.repo.GetEspecialidades();
            List<Doctor> doctores = this.repo.GetDoctores();
            return View(doctores);
        }
        [HttpPost]
        public IActionResult Index
            (string especialidad)
        {
            ViewData["ESPECIALIDADES"] = this.repo.GetEspecialidades();
            List<Doctor> doctores = this.repo.GetDoctoresEspecialidad(especialidad);
            return View(doctores);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create
            (Doctor doctor)
        {
            this.repo.InsertarDoctor(doctor.IdDoctor,doctor.Apellido, doctor.Especialidad, doctor.Salario, doctor.IdHospital);
            return RedirectToAction("Index");
        }
        public IActionResult Delete
            (int idDoctor)
        {
            this.repo.DeleteDoctor(idDoctor);
            return RedirectToAction("Index");
        }
        public IActionResult Update
            (int idDoctor)
        {
            Doctor doctor = this.repo.FindDoctor(idDoctor);
            return View(doctor);
        }
        [HttpPost]
        public IActionResult Update
            (Doctor doctor)
        {
            this.repo.UpdateDoctor(doctor.IdDoctor, doctor.Apellido, doctor.Especialidad, doctor.Salario, doctor.IdHospital);
            return RedirectToAction("Index");
        }
    }
}
