import { useState, useContext, useEffect, useRef } from "react";
import { useNavigate, Link, useLocation } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";
import { initMercadoPago, Wallet } from '@mercadopago/sdk-react';

function SimulacionDia11Page() {
  const { user } = useContext(AuthContext);
  const navigate = useNavigate();
  const location = useLocation();
  const [showPayment, setShowPayment] = useState(false);
  const [preferenceId, setPreferenceId] = useState(null);
  const yaSeEjecuto = useRef(false);

    const simular = async () =>
    {
        const response = await fetch(
            "http://localhost:5266/api/mails/recordatorio-pagos",
            {
                method: "POST"
            });

        if(response.ok)
        {
            alert("Recordatorios enviados.");
        }
        else
        {
            alert("Ocurrió un error.");
        }
    }

    useEffect(() =>
    {
        if (yaSeEjecuto.current)
        {
            return;
        }

        yaSeEjecuto.current = true;
        simular();
    }, []);

  return (
    <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', marginTop: '50px' }}>
        <h1>Simulando Día 11 del mes...</h1>
    </div>
  );
}

export default SimulacionDia11Page;