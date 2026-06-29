import { useNavigate } from "react-router-dom";

function BotonCancelarTurno({ idTurno })
{
    const navigate = useNavigate();

    const handleCancelarTurno = () =>
    {
        navigate(`/suspender-turno-admin/${idTurno}`);
    };

    return (
        <button
            onClick={handleCancelarTurno}
            className="btn btn-warning"
            style={{
                padding: "10px 20px",
                fontSize: "16px",
                backgroundColor: "#FF9800",
                color: "white",
                border: "none",
                borderRadius: "4px",
                cursor: "pointer",
                marginRight: "10px",
            }}
        >
            Cancelar Turno
        </button>
    );
}

export default BotonCancelarTurno;