using System;
using System.Threading.Tasks;

namespace Sportify.Aplicacion.AplicacionUsuarios;

public class SuspenderCuentaUseCase
{
    private readonly IRepositorioUsuarios repositorioUsuarios;

    public SuspenderCuentaUseCase(IRepositorioUsuarios repositorioUsuarios)
    {
        this.repositorioUsuarios = repositorioUsuarios;
    }

    public async Task Ejecutar(string mail)
    {
        await this.repositorioUsuarios.SuspenderAlumnoPermanente(mail);
    }
}