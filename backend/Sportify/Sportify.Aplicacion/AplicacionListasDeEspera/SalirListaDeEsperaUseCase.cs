using System;
using System.Threading.Tasks;
using Sportify.Dominio.ListasDeEspera;
using Sportify.Aplicacion;
using Sportify.Aplicacion.AplicacionAbonos;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Aplicacion.Excepciones;
namespace Sportify.Aplicacion.AplicacionListasDeEspera;

public class SalirListaEsperaTurnoUseCase
{
    private readonly IRepositorioListaDeEsperaTurno repositorio;
    private readonly IRepositorioUsuarios repositorioUsuarios;

    public SalirListaEsperaTurnoUseCase(
        IRepositorioListaDeEsperaTurno repositorio,
        IRepositorioUsuarios repositorioUsuarios)
    {
        this.repositorio = repositorio;
        this.repositorioUsuarios = repositorioUsuarios;
    }

    public async Task Ejecutar(string email, Guid idTurno)
    {
        var usuario = await repositorioUsuarios.ObtenerPorMail(email);

        if (usuario == null)
            throw new EntidadNotFoundException("Usuario no encontrado.");

        await repositorio.eliminarEspera(Guid.Parse(usuario.Id), idTurno);
    }
}