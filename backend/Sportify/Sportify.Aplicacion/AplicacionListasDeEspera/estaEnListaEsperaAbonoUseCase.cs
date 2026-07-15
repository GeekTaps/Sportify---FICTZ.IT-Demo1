using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Aplicacion.AplicacionListasDeEspera;
using Sportify.Aplicacion.AplicacionUsuarios;
using System;
using System.Threading.Tasks;
using Sportify.Dominio;
using Sportify.Dominio.Usuario;
using Sportify.Dominio.ListasDeEspera;
namespace Sportify.Aplicacion.AplicacionListasDeEspera;
public class EstaEnListaEsperaAbonoUseCase
{
    private readonly IRepositorioListaDeEsperaAbono repositorioLista;
    private readonly IRepositorioUsuarios repositorioUsuarios;

    public EstaEnListaEsperaAbonoUseCase(
        IRepositorioListaDeEsperaAbono repositorioLista,
        IRepositorioUsuarios repositorioUsuarios)
    {
        this.repositorioLista = repositorioLista;
        this.repositorioUsuarios = repositorioUsuarios;
    }

    public async Task<bool> Ejecutar(string email, Guid idDeporte, Guid idHorario)
{
    var usuario = await repositorioUsuarios.ObtenerPorMail(email);

    if (usuario == null)
        return false;

    return await repositorioLista.existeEnEspera(
        Guid.Parse(usuario.Id),
        idDeporte
    );
}
}