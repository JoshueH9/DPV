using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AumentoEstadisticas : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        /*
            Si el objeto con el que entra en contacto es de tipo "Jugador",
            entonces, aumenta la estadistica de Velocidad de movimiento de los minions
            temporalmente.
        */

        if (collider.gameObject.tag == "Player")
        {
            Juego.controlador.AumentarEstadisticasAE();
            Destroy(this.gameObject);
        }
    }
}
