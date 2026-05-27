using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sportify.Infraestructura.Data;
using Sportify.Infraestructura.Identity;
using Sportify.Aplicacion.AplicacionDeportes;
using Sportify.Infraestructura.Repositorios;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Aplicacion.AplicacionReservas;


var builder = WebApplication.CreateBuilder(args);
//crea el builder principal de ASP.NET. Acá es donde se registran servicios.


// permite interactuar con react sin que se bloquee
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:7001") // puerto de React al que se conecta
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});


// Add services to the container.
builder.Services.AddControllersWithViews();
//habilita MVC. Controllers + Views.

//hagan los Scoped y Transient necesarios para la comunicacion con el front.

//Scoped de Repositorios // se registran los repositorios para que puedan ser inyectados en los casos de uso
builder.Services.AddScoped<IRepositorioDeporte, RepositorioDeportes>();
builder.Services.AddScoped<IRepositorioTurno, RepositorioTurno>();
builder.Services.AddScoped<IRepositorioUsuarios, RepositorioUsuarios>();
builder.Services.AddScoped<IRepositorioReserva, RepositorioReserva>();

//Scoped de Deportes
builder.Services.AddScoped<DeporteListadoUseCase>();
builder.Services.AddScoped<DeporteAltaUseCase>();
builder.Services.AddScoped<DeporteBajaUseCase>();
builder.Services.AddScoped<DeporteModificacionUseCase>();

builder.Services.AddTransient<IValidadorDeporte, ValidadorDeporte>();

//Scoped de Usuarios 
builder.Services.AddScoped<RegistrarUsuarioUseCase>();
builder.Services.AddTransient<IValidadorRegistrarUsuario, ValidadorRegistrarUsuario>();

//Scoped de Turnos
builder.Services.AddScoped<TurnoListadoUseCase>();
builder.Services.AddScoped<TurnoAltaUseCase>();
builder.Services.AddScoped<TurnoModificacionUseCase>();
builder.Services.AddTransient<IValidadorTurno, ValidadorTurno>();

//Scoped de Reservas
builder.Services.AddScoped<ReservaListadoUseCase>();
builder.Services.AddScoped<ReservaAltaUseCase>();
builder.Services.AddScoped<ReservaBajaUseCase>();
builder.Services.AddScoped<ReservaBusquedaUseCase>();
builder.Services.AddTransient<IValidadorReserva, ValidadorReserva>();

builder.Services.AddDbContext<ApplicationDbContext>(options => //registra EF Core.
    options.UseSqlite( //le dice usar SQLite.
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentity<UsuarioIdentity, IdentityRole>(options =>  //configuracion de contraseña
    {
        options.Password.RequiredLength = 6;

        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
    }) //configura ASP.NET Identity.
    .AddEntityFrameworkStores<ApplicationDbContext>(); //hace que Identity use EF Core.


//------------------------------------------------------------------------------------------

var app = builder.Build();
//construye la aplicación. Después de esto ya no se deberían registrar servicios.
//todos los servicios se registran antes del Build().

// Inicializa la base de datos SQLite (aplica migraciones al arrancar).
RepositoriosSQLites.Inicializar(app.Services);

using (var scope = app.Services.CreateScope())
{
    var repo = scope.ServiceProvider.GetRequiredService<IRepositorioDeporte>();
    var existe = await repo.existeDeportePorNombre("Futbol");
    if (!existe)
    {
        await repo.crearDeporte("Futbol", "Deporte de equipo");
    }

    // Seed de Usuario milka123@mail.com y sus reservas de prueba
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UsuarioIdentity>>();
    var milkaUser = await userManager.FindByEmailAsync("milka123@mail.com");
    if (milkaUser == null)
    {
        milkaUser = new UsuarioIdentity
        {
            NombreCompleto = "Milka Test",
            Email = "milka123@mail.com",
            Edad = "25",
            Dni = "12345678",
            UserName = "milka123@mail.com" // Requerido por Identity
        };
        var result = await userManager.CreateAsync(milkaUser, "Milka.123!"); // Contraseña fuerte requerida por defecto

        if (result.Succeeded)
        {
            var reservaRepo = scope.ServiceProvider.GetRequiredService<IRepositorioReserva>();
            Guid idUsuario = Guid.Parse(milkaUser.Id);

            // Generamos turnos mock para las reservas
            Guid mockTurno1 = Guid.NewGuid();
            Guid mockTurno2 = Guid.NewGuid();

            var reserva1 = new Sportify.Dominio.Reservas.Reserva(idUsuario, mockTurno1, true, 1500.50, "Vóley - 23/06/26 - 18hs");
            var reserva2 = new Sportify.Dominio.Reservas.Reserva(idUsuario, mockTurno2, false, 3200.00, "Fútbol - 25/06/26 - 20hs");

            await reservaRepo.agregarReserva(reserva1);
            await reservaRepo.agregarReserva(reserva2);
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// permite interactuar con react sin que se bloquee
app.UseCors("AllowReactApp");

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); //activa autenticación. MUY importante.
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

await app.RunAsync();