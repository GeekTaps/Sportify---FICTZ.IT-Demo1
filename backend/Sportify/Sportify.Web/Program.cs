using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sportify.Infraestructura.Data;
using Sportify.Infraestructura.Identity;

using Sportify.Aplicacion.AplicacionDeportes;
using Sportify.Infraestructura.Repositorios;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Aplicacion.AplicacionReservas;
using Sportify.Aplicacion.AplicacionPagos;
using Sportify.Aplicacion;
using Sportify.Aplicacion.Mails;
using Sportify.Infraestructura;


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
builder.Services.AddScoped<IRepositorioPago, RepositorioPagos>();

//Scoped de Deportes
builder.Services.AddScoped<DeporteListadoUseCase>();
builder.Services.AddScoped<DeporteAltaUseCase>();
builder.Services.AddScoped<DeporteBajaUseCase>();
builder.Services.AddScoped<DeporteModificacionUseCase>();

builder.Services.AddTransient<IValidadorDeporte, ValidadorDeporte>();

//Scoped de Usuarios 
builder.Services.AddScoped<RegistrarUsuarioUseCase>();
builder.Services.AddScoped<RegistrarPagoUseCase>();
builder.Services.AddScoped<ListarPagosUsuarioUseCase>();
builder.Services.AddScoped<ListarMailsDeUnTurnoUseCase>();
builder.Services.AddScoped<ListarMailsDeUsuariosConPagosPendientesUseCase>();
builder.Services.AddTransient<IValidadorRegistrarUsuario, ValidadorRegistrarUsuario>();

builder.Services.AddScoped<modificarUsuarioUseCase>();
builder.Services.AddTransient<IValidadorModificarUsuario, ValidadorModificarUsuario>();

builder.Services.AddScoped<BajaLogicaUsuarioUseCase>();
builder.Services.AddTransient<IRepositorioCreditos, RepositorioCreditos>();

builder.Services.AddScoped<RecuperarCuentaUseCase>();

builder.Services.AddScoped<RegistrarEmpleadoUseCase>();

//Scoped de Turnos
builder.Services.AddScoped<TurnoListadoUseCase>();
builder.Services.AddScoped<TurnoAltaUseCase>();
builder.Services.AddScoped<TurnoModificacionUseCase>();
builder.Services.AddScoped<TurnoAltaMensualUseCase>();
builder.Services.AddScoped<TurnoModificacionMensualUseCase>();
builder.Services.AddScoped<TurnoBajaUseCase>();
builder.Services.AddScoped<SuspenderTurnoAdminUseCase>();
builder.Services.AddTransient<IValidadorTurno, ValidadorTurno>();

//Scoped de Reservas
builder.Services.AddScoped<ReservaListadoUseCase>();
builder.Services.AddScoped<ReservaAltaUseCase>();
builder.Services.AddScoped<ReservaBajaUseCase>();
builder.Services.AddScoped<ReservaBusquedaUseCase>();
builder.Services.AddTransient<IValidadorReserva, ValidadorReserva>();

//Mails papá
builder.Services.Configure<ModeloMail>(
    builder.Configuration.GetSection("ModeloMail"));
builder.Services.AddTransient<IServicioEmail, ServicioEmail>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//construye la aplicación. Después de esto ya no se deberían registrar servicios.
//todos los servicios se registran antes del Build().

// Inicializa la base de datos SQLite (aplica migraciones al arrancar).
RepositoriosSQLites.Inicializar(app.Services);

// Sembrar cuentas de administrador por defecto
await RepositoriosSQLites.SeedUsuariosAdmin(app.Services);

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