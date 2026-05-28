function BotonMostrarListadoTurnos({ onClick }) {
  return (
    <button 
      onClick={onClick}
      style={{
        padding: "10px 20px",
        fontSize: "16px",
        backgroundColor: "#4CAF50",
        color: "white",
        border: "none",
        borderRadius: "4px",
        cursor: "pointer",
        marginRight: "10px",
      }}
    >
      Mostrar Listado de Turnos
    </button>
  );
}

export default BotonMostrarListadoTurnos;
