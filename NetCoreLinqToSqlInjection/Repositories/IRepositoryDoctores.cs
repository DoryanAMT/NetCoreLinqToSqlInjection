using NetCoreLinqToSqlInjection.Models;

namespace NetCoreLinqToSqlInjection.Repositories
{
    public interface IRepositoryDoctores
    {
        List<Doctor> GetDoctores();
        void InsertarDoctor
            (int idDoctor, string apellido, string especialidad, int salario, int idHospital);
        void DeleteDoctor
            (int idDoctor);
        void UpdateDoctor
            (int idDoctor, string apellido, string especialidad, int salario, int idHospital);
        Doctor FindDoctor
            (int idDoctor);
        List<string> GetEspecialidades();
        List<Doctor> GetDoctoresEspecialidad(string especialidad);

    }
}
