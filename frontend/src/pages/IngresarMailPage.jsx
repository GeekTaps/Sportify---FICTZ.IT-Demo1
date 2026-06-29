import { useLocation } from "react-router-dom";
import { useState } from "react";

function IngresarMailPage()
{
    const location = useLocation();

    const mails = location.state?.mails || [];

    const [texto,setTexto] = useState("");

    const enviar = async () =>
    {
        const response = await fetch(
            "http://localhost:5266/api/mails/enviar",
            {
                method:"POST",
                headers:
                {
                    "Content-Type":"application/json"
                },
                body:JSON.stringify(
                {
                    asunto:"Cancelación de clase",
                    cuerpo:texto,
                    mails:mails
                })
            });

        if(response.ok)
        {
            alert("Mails enviados.");
        }
        else
        {
            alert("Error enviando mails.");
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