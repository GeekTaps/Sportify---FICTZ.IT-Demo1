import { useLocation } from "react-router-dom";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

function IngresarMailPage()
{
    const location = useLocation();
    const navigate = useNavigate();

    if (!location.state)
    {
        return <h2>No hay un turno seleccionado.</h2>;
    }

    const mails = location.state?.mails || [];
    const idTurno = location.state?.idTurno;

    const [texto,setTexto] = useState("");

    const enviar = async () =>
    {
        // 1. Cancelar el turno
        const cancelarResponse = await fetch(
            `http://localhost:5266/api/turnos/deporte/${idTurno}`,
            {
                method: "POST"
            }
        );

        if (!cancelarResponse.ok)
        {
            alert("No se pudo cancelar el turno.");
            return;
        }

        // 2. Enviar los mails
        const mailResponse = await fetch(
            "http://localhost:5266/api/mails/enviar",
            {
                method: "POST",
                headers:
                {
                    "Content-Type":"application/json"
                },
                body: JSON.stringify(
                {
                    asunto:"Cancelación de clase",
                    cuerpo:texto,
                    mails:mails
                })
            });

        if(mailResponse.ok)
        {
            alert("Turno cancelado y mails enviados.");
            navigate("/turnos");
        }
        else
        {
            alert("El turno se canceló pero ocurrió un error enviando los mails.");
        }
    };

    return(
        <div>

            <h1>Cancelar turno</h1>

            <p>
                Destinatarios: {mails.length}
            </p>

            <textarea
                rows={10}
                style={{width:"100%"}}
                value={texto}
                onChange={(e)=>setTexto(e.target.value)}
            />

            <br/><br/>

            <button
                className="btn btn-primary"
                onClick={enviar}
            >
                Enviar
            </button>

        </div>
    );
}

export default IngresarMailPage;