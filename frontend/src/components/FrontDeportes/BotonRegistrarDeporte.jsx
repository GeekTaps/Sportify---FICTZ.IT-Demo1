function BotonRegistrarDeporte({ loading }) {
  return (
    <button type="submit" disabled={loading} className="btn btn-primary">
      {loading ? "Registrando..." : "Registrar deporte"}
    </button>
  );
}

export default BotonRegistrarDeporte;
