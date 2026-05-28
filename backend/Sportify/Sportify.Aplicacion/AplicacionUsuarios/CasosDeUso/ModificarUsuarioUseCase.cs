    using System;
    using System.Threading.Tasks;
    using Sportify.Dominio;
    using Sportify.Dominio.Usuario;

    namespace Sportify.Aplicacion.AplicacionUsuarios;

    public class modificarUsuarioUseCase
    {
    private readonly IRepositorioUsuarios repositorioUsuarios;
    private readonly IValidadorModificarUsuario validadorModificarUsuario;

    public modificarUsuarioUseCase(IValidadorModificarUsuario validadorUsuario,IRepositorioUsuarios repositorioUsuarios)
        {
            this.validadorModificarUsuario=validadorUsuario;
            this.repositorioUsuarios=repositorioUsuarios;

        }
    //esta parte me dejó muchas dudas
    public async Task Ejecutar(string idUsuario, Usuario dto)
        {
            await this.validadorModificarUsuario
                .validar(dto, idUsuario);
            
                await this.repositorioUsuarios
                    .ModificarUsuario(idUsuario, dto);
            
        }
       public async Task<Usuario> ObtenerPorId(string id)
{
    return await repositorioUsuarios.ObtenerPorId(id);
} 

    }

