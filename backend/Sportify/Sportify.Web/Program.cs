using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sportify.Infraestructura.Data;
using Sportify.Infraestructura.Identity;

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

builder.Services.AddDbContext<ApplicationDbContext>(options => //registra EF Core.
    options.UseSqlite( //le dice usar SQLite.
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentity<UsuarioIdentity, IdentityRole>() //configura ASP.NET Identity.
    .AddEntityFrameworkStores<ApplicationDbContext>(); //hace que Identity use EF Core.

//------------------------------------------------------------------------------------------

var app = builder.Build();
//construye la aplicación. Después de esto ya no se deberían registrar servicios.
//todos los servicios se registran antes del Build().

// Inicializa la base de datos SQLite (aplica migraciones al arrancar).
RepositoriosSQLites.Inicializar(app.Services);


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


app.Run();
