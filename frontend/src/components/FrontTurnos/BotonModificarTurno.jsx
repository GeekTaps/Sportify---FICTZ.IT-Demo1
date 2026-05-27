function BotonModificarTurno({ onClick }) {
  return (
    <button 
      onClick={onClick}
      style={{
        padding: "8px 16px",
        fontSize: "14px",
        backgroundColor: "#2196F3",
        color: "white",
        border: "none",
        borderRadius: "4px",
        cursor: "pointer",
        marginLeft: "10px",
      }}
    >
      Modificar Turno
    </button>
  );
}

export default BotonModificarTurno;
