function BotonModificarDeporte({ onClick }) {
  return (
    <button type="button" onClick={onClick} className="btn btn-outline" style={{ padding: "0.45rem 1rem", fontSize: "0.9rem" }}>
      Modificar
    </button>
  );
}

export default BotonModificarDeporte;
