namespace Sportify.Dominio.Usuario
{
    public class Credito
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; } //usuario que tiene el credito
        public Guid DeporteId { get; set; } //deporte al que pertenece el credito
        public int Cantidad { get; set; } = 0; //cantidad de creditos que tiene el usuario para ese deporte

        public Credito(Guid usuarioId, Guid deporteId)
        {
            Id = Guid.NewGuid();
            UsuarioId = usuarioId;
            DeporteId = deporteId;
            Cantidad = 1;
        }

        public void AgregarCredito()
        {
            Cantidad += 1;
        }

        public void UsarCredito()
        {
            if (Cantidad > 0)
            {
                Cantidad -= 1;
            }
            else
            {
                throw new InvalidOperationException("No hay créditos disponibles para usar.");
            }
        }
    }
}