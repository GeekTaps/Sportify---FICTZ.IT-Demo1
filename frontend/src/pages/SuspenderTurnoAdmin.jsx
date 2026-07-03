import { useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";

function SuspenderTurnoAdmin()
{
    const { idTurno } = useParams();

    const navigate = useNavigate();

    useEffect(() =>
    {
        obtenerMails();
    }, []);

    const obtenerMails = async () =>
    {
        const response = await fetch(
            `http://localhost:5266/api/turnos/deporte/${idTurno}`
        );

        if(response.ok)
        {
            const mails = await response.json();

            if (!mails || mails.length === 0)
            {
                //acá deberia cancelar el turno y mostrar un mensaje de que no hay usuarios con ese turno reservado
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
                alert("No hay usuarios con ese turno reservado");
                navigate("/turnos"); // opcional: volver al listado
                return;
            }

            navigate("/ingresar-mail",
            {
                state:
                {
                    mails,
                    idTurno
                }
            });
        }
        else
        {
            alert("No fue posible cancelar el turno.");
        }
    };

    return(
        <h2>Cancelando turno...</h2>
    );
}

export default SuspenderTurnoAdmin;