using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Aplicacion.AplicacionListasDeEspera;
using Sportify.Aplicacion.AplicacionUsuarios;
using System;
using System.Threading.Tasks;
using Sportify.Dominio;
using Sportify.Dominio.Usuario;
namespace Sportify.Aplicacion.AplicacionListasDeEspera;
public class estaEnListaEsperaTurnoUseCase
{
    private readonly IRepositorioListaDeEsperaTurno repositorioLista;
    private readonly IRepositorioUsuarios repositorioUsuarios;

    public estaEnListaEsperaTurnoUseCase(
        IRepositorioListaDeEsperaTurno repositorioLista,
        IRepositorioUsuarios repositorioUsuarios)
    {
        this.repositorioLista = repositorioLista;
        this.repositorioUsuarios = repositorioUsuarios;
    }

    public async Task<bool> Ejecutar(string email, Guid idTurno)
    {
        var usuario = await repositorioUsuarios.ObtenerPorMail(email);

        if (usuario == null)
            return false;

        return await repositorioLista.existeEnEspera(Guid.Parse(usuario.Id), idTurno);
    }
}

