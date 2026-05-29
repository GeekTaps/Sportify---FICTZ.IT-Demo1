function BotonEliminarDeporte({ onClick }) {
  return (
    <button type="button" onClick={onClick} className="btn btn-danger" style={{ padding: "0.45rem 1rem", fontSize: "0.9rem" }}>
      Eliminar
    </button>
  );
}

export default BotonEliminarDeporte;
