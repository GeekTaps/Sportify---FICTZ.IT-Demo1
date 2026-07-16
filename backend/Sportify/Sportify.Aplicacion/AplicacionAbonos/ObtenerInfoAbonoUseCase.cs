using System;
using System.Linq;
using System.Threading.Tasks;
using Sportify.Aplicacion.AplicacionAbonos;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionReservas;
using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Dominio.Turnos;

namespace Sportify.Aplicacion.AplicacionAbonos;

public class ObtenerInfoAbonoUseCase
{
    private readonly IRepositorioTurno _repositorioTurno;
    private readonly IRepositorioAbono _repositorioAbono;
    private readonly IRepositorioUsuarios _repositorioUsuarios;
    private readonly IRepositorioReserva _repositorioReserva;
    private readonly IRepositorioCreditos _repositorioCreditos;

    public ObtenerInfoAbonoUseCase(
        IRepositorioTurno repositorioTurno,
        IRepositorioAbono repositorioAbono,
        IRepositorioUsuarios repositorioUsuarios,
        IRepositorioReserva repositorioReserva,
        IRepositorioCreditos repositorioCreditos)
    {
        _repositorioTurno = repositorioTurno;
        _repositorioAbono = repositorioAbono;
        _repositorioUsuarios = repositorioUsuarios;
        _repositorioReserva = repositorioReserva;
        _repositorioCreditos = repositorioCreditos;
    }

    public async Task<AbonoInfoDTO> Ejecutar(Guid idTurno, string email)
    {
        var response = new AbonoInfoDTO();
        
        var turno = await _repositorioTurno.ObtenerTurnoPorId(idTurno);
        if (turno == null) 
            throw new Exception("Turno no encontrado.");

        var usuario = await _repositorioUsuarios.ObtenerPorMail(email);
        if (usuario == null) 
            throw new Exception("Usuario no encontrado.");

        Guid idUsuario = Guid.Parse(usuario.Id);

        // 1. IsAlreadySubscribed
        response.IsAlreadySubscribed = await _repositorioAbono.ExisteAbonoActivo(idUsuario, turno.IdHorario);

        // 2. HasConflict (Check if user has any reservation overlapping with the Turno's time)
        var reservasUsuario = await _repositorioReserva.listarReservasUsuario(idUsuario);
        var todosLosTurnos = await _repositorioTurno.ListarTurnos();
        
        response.HasConflict = reservasUsuario.Any(r => 
        {
            if (r.eliminada) return false;
            var tReserva = todosLosTurnos.FirstOrDefault(x => x.Id == r.idTurno);
            if (tReserva == null) return false;
            // Check if tReserva conflicts with this specific Turno's date and time (overlap)
            return tReserva.Fecha.Date == turno.Fecha.Date && 
                   tReserva.horaInicio < turno.horaFin && 
                   tReserva.horaFin > turno.horaInicio;
        });

        // 3. IsPast10thDay
        var now = DateTime.Now;
        response.IsPast10thDay = now.Day > 10;

        // Fetch all turnos for this Horario
        var turnosDelHorario = todosLosTurnos
            .Where(t => t.IdHorario == turno.IdHorario && t.Fecha >= now)
            .OrderBy(t => t.Fecha)
            .ToList();

        // 4. HasFewClasses: next 30 days
        var turnosProximos30Dias = turnosDelHorario.Count(t => t.Fecha <= now.AddDays(30));
        response.HasFewClasses = turnosProximos30Dias < 3;

        // Determine relevant classes for pricing and cupo (current month + first 10 days of next month)
        // If we are in July, next month is August.
        var startOfNextMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1);
        var dateLimit = startOfNextMonth.AddDays(9).AddHours(23).AddMinutes(59); // up to the 10th of next month (inclusive)

        var clasesAbono = turnosDelHorario.Where(t => t.Fecha <= dateLimit).ToList();

        // 5. NoCupo
        // If ANY of the relevant classes is full, the abono is full
        response.NoCupo = clasesAbono.Any(t => t.cupo == 0);

        // Populate info
        var nombreLimpio = turno.nombreTurno.Split('-')[0].Trim();
        var culture = new System.Globalization.CultureInfo("es-ES");
        var diaSemana = culture.TextInfo.ToTitleCase(culture.DateTimeFormat.GetDayName(turno.Fecha.DayOfWeek));
        
        response.Actividad = nombreLimpio;
        response.Horario = $"{nombreLimpio} - {diaSemana} - {turno.horaInicio:HH:mm}hs";

        // Calculate Price
        int cantidadClases = clasesAbono.Count;
        double precioClase = turno.Precio;

        var creditoEntity = await _repositorioCreditos.ObtenerCredito(Guid.Parse(usuario.Id), turno.IdDeporte);
        int creditos = creditoEntity != null ? creditoEntity.Cantidad : 0;
        int creditosAplicables = Math.Min(creditos, cantidadClases);

        double precioBase = cantidadClases * precioClase;
        response.PrecioTotal = (precioBase - (creditosAplicables*precioClase)) * 0.80;
        response.DescuentoAplicado = precioBase - response.PrecioTotal;
        if (response.PrecioTotal < 0){
            response.PrecioTotal = 0;
            response.DescuentoAplicado = precioBase;
        }

        return response;
    }
}
