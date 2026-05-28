using System;
using System.Threading.Tasks;
using Sportify.Dominio;
using Sportify.Dominio.Usuario;
using Sportify.Aplicacion.AplicacionReservas;
using Sportify.Dominio.Reservas;
using Sportify.Aplicacion.Excepciones;
namespace Sportify.Aplicacion.AplicacionUsuarios;
 //FALTA IMPLEMENTAR QUE AVISE SI EL USUARIO ESTÁ ANOTADO A CLASES TODAVIA 
public class BajaLogicaUsuarioUseCase
{
    private readonly IRepositorioUsuarios repositorioUsuarios;
     private readonly IRepositorioReserva repositorioReserva;

public BajaLogicaUsuarioUseCase(IRepositorioUsuarios repositorioUsuarios,IRepositorioReserva repositorioReserva)
    {
        
        this.repositorioUsuarios=repositorioUsuarios;
        this.repositorioReserva=repositorioReserva;
    }
 public async Task Ejecutar(string id)
    {
        Guid idUsuario = Guid.Parse(id);

        List<Reserva> reservas =
            await repositorioReserva.listarReservasUsuario(idUsuario);

        if (reservas.Count > 0)
        {
            throw new ValidacionException(
                "Todavía tenés reservas pendientes"
            );
        }

        await repositorioUsuarios.BajaLogica(id);
    }
}

    
