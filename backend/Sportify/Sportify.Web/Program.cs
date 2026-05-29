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

builder.Services.AddScoped<modificarUsuarioUseCase>();
builder.Services.AddTransient<IValidadorModificarUsuario, ValidadorModificarUsuario>();

builder.Services.AddScoped<BajaLogicaUsuarioUseCase>();

//Scoped de Turnos
builder.Services.AddScoped<TurnoListadoUseCase>();
builder.Services.AddScoped<TurnoAltaUseCase>();
builder.Services.AddScoped<TurnoModificacionUseCase>();
builder.Services.AddScoped<TurnoAltaMensualUseCase>();
builder.Services.AddScoped<TurnoModificacionMensualUseCase>();
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
    .AddEntityFrameworkStores<ApplicationDbContext>() //hace que Identity use EF Core.
    .AddDefaultTokenProviders();


//------------------------------------------------------------------------------------------

var app = builder.Build();
//construye la aplicación. Después de esto ya no se deberían registrar servicios.
//todos los servicios se registran antes del Build().

// Inicializa la base de datos SQLite (aplica migraciones al arrancar).
RepositoriosSQLites.Inicializar(app.Services);

using (var scope = app.Services.CreateScope())
{
    var repo = scope.ServiceProvider.GetRequiredService<IRepositorioDeporte>();
    if (!await repo.existeDeportePorNombre("Futbol")) await repo.crearDeporte("Futbol", "Deporte de equipo");
    if (!await repo.existeDeportePorNombre("Voley")) await repo.crearDeporte("Voley", "Deporte de equipo");

    var deportes = await repo.ListarDeportes();
    var dFutbol = deportes.First(d => d.nombre == "Futbol");
    var dVoley = deportes.First(d => d.nombre == "Voley");

    var turnoRepo = scope.ServiceProvider.GetRequiredService<IRepositorioTurno>();
    var turnosExistentes = await turnoRepo.ListarTurnos();

    Sportify.Dominio.Turnos.Turno turnoVoley = null;
    Sportify.Dominio.Turnos.Turno turnoFutbol = null;

    if (turnosExistentes == null || !turnosExistentes.Any())
    {
        // Turno a > 48hs
        turnoVoley = new Sportify.Dominio.Turnos.Turno {
            Id = Guid.NewGuid(),
            IdDeporte = dVoley.id,
            Fecha = DateTime.Now.AddDays(3),
            horaInicio = new TimeOnly(18, 0),
            horaFin = new TimeOnly(19, 0),
            nombreTurno = "Vóley Avanzado",
            nommbreProfesor = "Prof. Dibu",
            cupo = 10,
            Precio = 1500,
            ListaEsperaHabilitada = true
        };
        await turnoRepo.AltaTurno(turnoVoley);

        // Turno a < 48hs
        turnoFutbol = new Sportify.Dominio.Turnos.Turno {
            Id = Guid.NewGuid(),
            IdDeporte = dFutbol.id,
            Fecha = DateTime.Now.AddDays(1),
            horaInicio = new TimeOnly(18, 0),
            horaFin = new TimeOnly(19, 0),
            nombreTurno = "Fútbol 5",
            nommbreProfesor = "Prof. Messi",
            cupo = 10,
            Precio = 3200,
            ListaEsperaHabilitada = false
        };
        await turnoRepo.AltaTurno(turnoFutbol);

        // Turno sin cupo con lista de espera
        var turnoVoleyLlenoConEspera = new Sportify.Dominio.Turnos.Turno {
            Id = Guid.NewGuid(),
            IdDeporte = dVoley.id,
            Fecha = DateTime.Now.AddDays(4),
            horaInicio = new TimeOnly(18, 0),
            horaFin = new TimeOnly(19, 0),
            nombreTurno = "Vóley Lleno - Espera",
            nommbreProfesor = "Prof. Scaloni",
            cupo = 0,
            Precio = 2000,
            ListaEsperaHabilitada = true
        };
        await turnoRepo.AltaTurno(turnoVoleyLlenoConEspera);

        // Turno sin cupo sin lista de espera
        var turnoVoleyLlenoSinEspera = new Sportify.Dominio.Turnos.Turno {
            Id = Guid.NewGuid(),
            IdDeporte = dVoley.id,
            Fecha = DateTime.Now.AddDays(5),
            horaInicio = new TimeOnly(18, 0),
            horaFin = new TimeOnly(19, 0),
            nombreTurno = "Vóley Lleno - Sin Espera",
            nommbreProfesor = "Prof. Samuel",
            cupo = 0,
            Precio = 2000,
            ListaEsperaHabilitada = false
        };
        await turnoRepo.AltaTurno(turnoVoleyLlenoSinEspera);
    }
    else
    {
        turnoVoley = turnosExistentes.FirstOrDefault(t => t.nombreTurno == "Vóley Avanzado");
        turnoFutbol = turnosExistentes.FirstOrDefault(t => t.nombreTurno == "Fútbol 5");
    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UsuarioIdentity>>();
    var reservaRepo = scope.ServiceProvider.GetRequiredService<IRepositorioReserva>();

    // Usuario Administrador
    var adminUser = await userManager.FindByEmailAsync("admin@mail.com");
    if (adminUser == null)
    {
        adminUser = new UsuarioIdentity { NombreCompleto = "Admin", Email = "admin@mail.com", Edad = "30", Dni = "00000000", UserName = "admin@mail.com", EsAdmin = true };
        var result = await userManager.CreateAsync(adminUser, "Admin.123!");
        if (!result.Succeeded)
        {
            Console.WriteLine("Error al crear admin: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
    else
    {
        // Forzar reset de password y flags por si se modificó o bloqueó
        adminUser.EsAdmin = true;
        adminUser.Borrado = false;
        adminUser.Suspendido = false;
        await userManager.UpdateAsync(adminUser);
        
        var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
        await userManager.ResetPasswordAsync(adminUser, token, "Admin.123!");
    }

    // Usuario malva123@mail.com (0 cancelaciones)
    var malvaUser = await userManager.FindByEmailAsync("malva123@mail.com");
    if (malvaUser == null)
    {
        malvaUser = new UsuarioIdentity { NombreCompleto = "Malva", Email = "malva123@mail.com", Edad = "20", Dni = "11111111", UserName = "malva123@mail.com", CancelacionesMes = 0, Creditos = 0 };
        if ((await userManager.CreateAsync(malvaUser, "Malva.123!")).Succeeded)
        {
            await reservaRepo.agregarReserva(new Sportify.Dominio.Reservas.Reserva(Guid.Parse(malvaUser.Id), turnoVoley.Id, true, 1500, $"Vóley - {turnoVoley.Fecha:dd/MM/yy} - 18hs"));
            await reservaRepo.agregarReserva(new Sportify.Dominio.Reservas.Reserva(Guid.Parse(malvaUser.Id), turnoFutbol.Id, false, 3200, $"Fútbol - {turnoFutbol.Fecha:dd/MM/yy} - 18hs"));
        }
    }

    // Usuario milka123@mail.com (2 cancelaciones)
    var milkaUser = await userManager.FindByEmailAsync("milka123@mail.com");
    if (milkaUser == null)
    {
        milkaUser = new UsuarioIdentity { NombreCompleto = "Milka Test", Email = "milka123@mail.com", Edad = "25", Dni = "12345678", UserName = "milka123@mail.com", CancelacionesMes = 2, Creditos = 1 };
        if ((await userManager.CreateAsync(milkaUser, "Milka.123!")).Succeeded)
        {
            await reservaRepo.agregarReserva(new Sportify.Dominio.Reservas.Reserva(Guid.Parse(milkaUser.Id), turnoVoley.Id, true, 1500, $"Vóley - {turnoVoley.Fecha:dd/MM/yy} - 18hs"));
            await reservaRepo.agregarReserva(new Sportify.Dominio.Reservas.Reserva(Guid.Parse(milkaUser.Id), turnoFutbol.Id, false, 3200, $"Fútbol - {turnoFutbol.Fecha:dd/MM/yy} - 18hs"));
        }
    }

    // Usuario sus123@mail.com (suspendido)
    var susUser = await userManager.FindByEmailAsync("sus123@mail.com");
    if (susUser == null)
    {
        susUser = new UsuarioIdentity { NombreCompleto = "Sus Test", Email = "sus123@mail.com", Edad = "22", Dni = "33333333", UserName = "sus123@mail.com", CancelacionesMes = 0, Suspendido = true, Creditos = 0 };
        if ((await userManager.CreateAsync(susUser, "Sus.123!")).Succeeded)
        {
            await reservaRepo.agregarReserva(new Sportify.Dominio.Reservas.Reserva(Guid.Parse(susUser.Id), turnoFutbol.Id, false, 3200, $"Fútbol - {turnoFutbol.Fecha:dd/MM/yy} - 18hs"));
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