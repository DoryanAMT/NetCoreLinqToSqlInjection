using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using NetCoreLinqToSqlInjection.Models;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Net.NetworkInformation;
using System.Numerics;

namespace NetCoreLinqToSqlInjection.Repositories;

#region
//create or alter procedure SP_DELETE_DOCTOR
//(@iddoctor int)
//as
//delete from DOCTOR where DOCTOR_NO=@iddoctor
//go
//create or alter procedure SP_UPDATE_DOCTOR
//(@iddoctor as int, @idhospital as int, @apellido as nvarchar(50), @especialidad as nvarchar(50), @salario as int)
//as
//update DOCTOR set HOSPITAL_COD = @idhospital, APELLIDO = @apellido, ESPECIALIDAD = @especialidad, SALARIO = @salario
//where DOCTOR_NO = @iddoctor
//go
#endregion
    public class RepositoryDoctoresSQLServer : IRepositoryDoctores
{
    private DataTable tableDoctores;
    private SqlConnection cn;
    private SqlCommand com;
    private SqlDataReader reader;

    public RepositoryDoctoresSQLServer()
    {
        string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Encrypt=True;Trust Server Certificate=True";
        this.cn = new SqlConnection(connectionString);
        this.com = new SqlCommand();
        this.com.Connection = this.cn;
        this.tableDoctores = new DataTable();
        SqlDataAdapter adDoc = new SqlDataAdapter("select * from DOCTOR", connectionString);
        adDoc.Fill(this.tableDoctores);
    }
    public List<Doctor> GetDoctores()
    {
        var consulta = from datos in this.tableDoctores.AsEnumerable()
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
    public void InsertarDoctor
        (int idDoctor, string apellido, string especialidad, int salario, int idHospital)
    {
        string sql = "insert into DOCTOR values(@idhospital, @iddoctor, @apellido, @especialidad, @salario)";
        this.com.Parameters.AddWithValue("@iddoctor", idDoctor);
        this.com.Parameters.AddWithValue("@apellido", apellido);
        this.com.Parameters.AddWithValue("@especialidad", especialidad);
        this.com.Parameters.AddWithValue("@salario", salario);
        this.com.Parameters.AddWithValue("@idhospital", idHospital);
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
        string sql = "SP_DELETE_DOCTOR";
        this.com.Parameters.AddWithValue("@iddoctor", idDoctor);
        this.com.CommandType = CommandType.StoredProcedure;
        this.com.CommandText = sql;
        this.cn.Open();
        this.com.ExecuteNonQuery();
        this.cn.Close();
        this.com.Parameters.Clear();
    }
    public void UpdateDoctor
        (int idDoctor, string apellido, string especialidad, int salario, int idHospital)
    {
        string sql = "SP_UPDATE_DOCTOR";
        this.com.Parameters.AddWithValue("@iddoctor", idDoctor);
        this.com.Parameters.AddWithValue("@apellido", apellido);
        this.com.Parameters.AddWithValue("@especialidad", especialidad);
        this.com.Parameters.AddWithValue("@salario", salario);
        this.com.Parameters.AddWithValue("@idhospital", idHospital);
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
        string sql = "select * from DOCTOR where DOCTOR_NO=@iddoctor";
        this.com.Parameters.AddWithValue("@iddoctor", idDoctor);
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
        var consulta = (from datos in this.tableDoctores.AsEnumerable()
                       select datos.Field<string>("ESPECIALIDAD")).Distinct();
        return consulta.ToList();
    }

    public List<Doctor> GetDoctoresEspecialidad
        (string especialidad)
    {
        var consulta = from datos in this.tableDoctores.AsEnumerable()
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
