    using System;
    using System.Threading.Tasks;
    using Sportify.Aplicacion.Excepciones;
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
    public async Task Ejecutar(string id, Usuario dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.PasswordNueva))
            {
                if (string.IsNullOrWhiteSpace(dto.PasswordActual))
                    throw new ValidacionException("Para cambiar la contraseña debes ingresar la contraseña actual.");

                bool passwordCorrecta = await repositorioUsuarios.VerificarPasswordActual(id, dto.PasswordActual);
                if (!passwordCorrecta)
                    throw new ValidacionException("La contraseña actual no coincide con la contraseña del usuario.");
            }

            Usuario usuarioActual =
        await repositorioUsuarios.ObtenerPorId(id);
            await this.validadorModificarUsuario
                .validar(dto,usuarioActual);
            
                await this.repositorioUsuarios
                    .ModificarUsuario(id, dto);
            
        }
public async Task<Usuario> ObtenerPorMail(string mail)
{
    return await repositorioUsuarios.ObtenerPorMail(mail);
}
public async Task<Usuario> ObtenerPorId(string id)
{
    return await repositorioUsuarios.ObtenerPorId(id);
}
    }

