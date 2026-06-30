import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { apiClient } from '../api/api-client';
import DetallePagoModal from '../components/DetallePagoModal';
import './ListadoPagosAdmin.css';

function ListadoPagosAdmin() {

    const { usuarioId } = useParams();

    const [pagos, setPagos] = useState([]);
    const [pagoSeleccionado, setPagoSeleccionado] = useState(null);
    const [mensajeError, setMensajeError] = useState("");

    useEffect(() => {

    const obtenerPagos = async () => {

        try {

            const response =
                await apiClient.get(`/pagos/usuario/${usuarioId}`);

            setPagos(response.data);

        }
        catch (error) {

            setMensajeError(
                error.response?.data?.message ||
                "Error al obtener los pagos."
            );

        }
    };

    obtenerPagos();

}, [usuarioId]);

    if (mensajeError !== "") {
    return (
        <div className="contenedor-pagos">
            <h1>{mensajeError}</h1>
        </div>
    );
    }

    return (

        <div className="contenedor-pagos">

            <h1>Pagos realizados</h1>

            {
                pagos.map(pago => (

                    <div
                        className="card-pago"
                        key={pago.id}
                        onClick={() => setPagoSeleccionado(pago)}
                    >

                        <h2>{pago.tituloReserva}</h2>

                        <p>
                            Fecha:
                            {" "}
                            {new Date(pago.fecha).toLocaleDateString()}
                        </p>

                        <p>
                            Monto: ${pago.monto}
                        </p>

                    </div>

                ))
            }

            {
                pagoSeleccionado &&
                <DetallePagoModal
                    pago={pagoSeleccionado}
                    cerrar={() => setPagoSeleccionado(null)}
                />
            }

        </div>

    );
}

export default ListadoPagosAdmin;