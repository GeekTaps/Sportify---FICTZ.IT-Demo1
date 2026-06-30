import './DetallePagoModal.css';

function DetallePagoModal({ pago, cerrar }) {

    return (

        <div className="modal-overlay">

            <div className="modal-content">

                <h1>Detalles del Pago</h1>

                <p>
                    <strong>Actividad:</strong>
                    {" "}
                    {pago.tituloReserva}
                </p>

                <p>
                    <strong>Fecha del pago:</strong>
                    {" "}
                    {new Date(pago.fecha).toLocaleDateString()}
                </p>

                <p>
                    <strong>Monto:</strong>
                    ${pago.monto}
                </p>

                <hr />

                <button
                    className="cerrar-btn"
                    onClick={cerrar}
                >
                    Cerrar
                </button>

            </div>

        </div>

    );
}

export default DetallePagoModal;