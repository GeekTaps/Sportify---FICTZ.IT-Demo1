using System;
using System.Threading.Tasks;
using Sportify.Dominio.Abonos;

namespace Sportify.Aplicacion.AplicacionAbonos;

public interface IRepositorioAbono
{
    Task<bool> ExisteAbonoActivo(Guid idUsuario, Guid idHorario);
    Task CrearAbono(Abono abono);
    Task<System.Collections.Generic.List<Abono>> ObtenerAbonosActivosPorHorario(Guid idHorario);
}
