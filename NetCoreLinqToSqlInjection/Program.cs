using NetCoreLinqToSqlInjection.Models;
using NetCoreLinqToSqlInjection.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IRepositoryDoctores, RepositoryDoctoresOracle>();

//  RESOLVEMOS EL SERVICIO Coche
//builder.Services.AddTransient<Coche>();
//builder.Services.AddSingleton<Coche>();
//builder.Services.AddSingleton<Deportivo>();
//builder.Services.AddSingleton<ICoche, Deportivo>();
Coche car = new Coche();
car.Marca = "PONTIAC";
car.Modelo = "RAYO";
car.Imagen = "21630oazpbr62stuyjq2.jpg";
car.Velocidad = 0;
car.VelocidadMaxima = 280;
//  PARA ENVIAR NUESTRO OBJETO PERSONALIZADO SE UTILIZA LAMBDA
builder.Services.AddSingleton<ICoche, Coche>(x => car);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
