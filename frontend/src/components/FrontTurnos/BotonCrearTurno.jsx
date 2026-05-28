import { useNavigate } from "react-router-dom";

function BotonCrearTurno() {
  const navigate = useNavigate();

  const handleCrearTurno = () => {
    navigate("/turnos/crear");
  };

  return (
    <button 
      onClick={handleCrearTurno}
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
      Crear Nuevo Turno
    </button>
  );
}

export default BotonCrearTurno;
