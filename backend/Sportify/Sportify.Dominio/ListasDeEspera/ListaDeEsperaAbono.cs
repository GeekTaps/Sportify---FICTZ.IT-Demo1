            namespace Sportify.Dominio.ListasDeEspera;
            using System;
            public class ListaDeEsperaAbono{
                public Guid id { get; private set; }
                public Guid idUsuario { get; private set; }
                public Guid idDeporte { get; private set; } //esto esta al pedo, no lo borro por si las duda
                public DateTime fecha { get; private set; }
                public Guid idHorario { get; private set; }

                public ListaDeEsperaAbono(Guid idUsuario, Guid idDeporte, Guid idHorario)
                {
                    this.id = Guid.NewGuid();
                    this.idUsuario = idUsuario;
                    this.idDeporte = idDeporte;
                    this.fecha = DateTime.Now;
                    this.idHorario = idHorario;
                }
            }
