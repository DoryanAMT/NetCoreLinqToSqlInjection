using Microsoft.AspNetCore.Http.HttpResults;
using NetCoreLinqToSqlInjection.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NetCoreLinqToSqlInjection.Repositories
{
    #region
    //create or replace procedure sp_delete_doctor
    //(p_iddoctor DOCTOR.DOCTOR_NO%TYPE)
    //as
    //begin
    //  delete from DOCTOR where DOCTOR_NO = p_iddoctor;
    //    commit;
    //end;
    #endregion
    public class RepositoryDoctoresOracle : IRepositoryDoctores
    {
        private DataTable tablaDoctores;
        private OracleConnection cn;
        private OracleCommand com;
        private OracleDataReader reader;
        public RepositoryDoctoresOracle()
        {
            string connectionString = @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True; User Id=SYSTEM; Password=oracle";
            this.tablaDoctores = new DataTable();
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            OracleDataAdapter adDoc = new OracleDataAdapter("Select * from DOCTOR", connectionString);
            adDoc.Fill(this.tablaDoctores);

        }

        public List<Doctor> GetDoctores()
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                           select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach (var row in consulta)
            {
                Doctor doctor = new Doctor();
                doctor.IdDoctor = row.Field<int>("DOCTOR_NO");
                doctor.Apellido = row.Field<string>("APELLIDO");
                doctor.Especialidad = row.Field<string>("ESPECIALIDAD");
                doctor.Salario = row.Field<int>("SALARIO");
                doctor.IdHospital = row.Field<int>("HOSPITAL_COD");
                doctores.Add(doctor);
            }
            return doctores;
        }

        public void InsertarDoctor(int idDoctor, string apellido, string especialidad, int salario, int idHospital)
        {
            string sql = "insert into DOCTOR values(:idhospital, :iddoctor, :apellido, :especialidad, :salario)";
            OracleParameter pamIdHospital = new OracleParameter(":idhospital", idHospital);
            this.com.Parameters.Add(pamIdHospital);
            OracleParameter pamIdDoctor = new OracleParameter(":iddoctor", idDoctor);
            this.com.Parameters.Add(pamIdDoctor);
            OracleParameter pamApellido = new OracleParameter(":apellido", apellido);
            this.com.Parameters.Add(pamApellido);
            OracleParameter pamEspecialidad = new OracleParameter(":especialidad", especialidad);
            this.com.Parameters.Add(pamEspecialidad);
            OracleParameter pamSalario = new OracleParameter(":salario", salario);
            this.com.Parameters.Add(pamSalario);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
        public void DeleteDoctor
            (int idDoctor)
        {
            string sql = "sp_delete_doctor";
            this.com.Parameters.Add(new OracleParameter(":iddoctor", idDoctor));
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void UpdateDoctor(int idDoctor, string apellido, string especialidad, int salario, int idHospital)
        {
            string sql = "sp_update_doctor";
            OracleParameter pamIdDoctor = new OracleParameter(":p_iddoctor", idDoctor);
            this.com.Parameters.Add(pamIdDoctor);
            OracleParameter pamIdHospital = new OracleParameter(":p_idhospital", idHospital);
            this.com.Parameters.Add(pamIdHospital);
            OracleParameter pamApellido = new OracleParameter(":p_apellido", apellido);
            this.com.Parameters.Add(pamApellido);
            OracleParameter pamEspecialidad = new OracleParameter(":p_especialidad", especialidad);
            this.com.Parameters.Add(pamEspecialidad);
            OracleParameter pamSalario = new OracleParameter(":p_salario", salario);
            this.com.Parameters.Add(pamSalario);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public Doctor FindDoctor
            (int idDoctor)
        {
            string sql = "select * from DOCTOR where DOCTOR_NO=:iddoctor";
            this.com.Parameters.Add(new OracleParameter(":iddoctor", idDoctor));
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            this.reader.Read();
            Doctor doctor = new Doctor();
            doctor.IdDoctor = int.Parse(this.reader["DOCTOR_NO"].ToString());
            doctor.IdHospital = int.Parse(this.reader["HOSPITAL_COD"].ToString());
            doctor.Apellido = this.reader["APELLIDO"].ToString();
            doctor.Especialidad = this.reader["ESPECIALIDAD"].ToString();
            doctor.Salario = int.Parse(this.reader["SALARIO"].ToString());
            this.cn.Close();
            this.com.Parameters.Clear();
            this.reader.Close();
            return doctor;
        }

        public List<string> GetEspecialidades()
        {
            var consulta = (from datos in this.tablaDoctores.AsEnumerable()
                            select datos.Field<string>("ESPECIALIDAD")).Distinct();
            return consulta.ToList();
        }

        public List<Doctor> GetDoctoresEspecialidad
            (string especialidad)
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                           where datos.Field<string>("ESPECIALIDAD") == especialidad
                           select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach (var row in consulta)
            {
                Doctor doctor = new Doctor();
                doctor.IdDoctor = row.Field<int>("DOCTOR_NO");
                doctor.Apellido = row.Field<string>("APELLIDO");
                doctor.Especialidad = row.Field<string>("ESPECIALIDAD");
                doctor.Salario = row.Field<int>("SALARIO");
                doctor.IdHospital = row.Field<int>("HOSPITAL_COD");
                doctores.Add(doctor);
            }
            return doctores;
        }
    }
}

