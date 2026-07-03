using System;
using System.Threading.Tasks;
using Sportify.Dominio.Abonos;

namespace Sportify.Aplicacion.AplicacionAbonos;

public interface IRepositorioAbono
{
    Task<bool> ExisteAbonoActivo(Guid idUsuario, Guid idHorario);
    Task CrearAbono(Abono abono);
}
