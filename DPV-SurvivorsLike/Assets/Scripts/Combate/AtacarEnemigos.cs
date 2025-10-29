using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtacarEnemigos : MonoBehaviour
{
    private void OnTriggerStay(Collider collider)
    {
        /*
            Si el objeto con el que entra en contacto es de tipo "Enemigo",
            crea en el suelo una instancia de experiencia en la posicion en la que
            antes estaba el enemigo y destruye al enemigo.

            ESTO SE CAMBIARÁ DESPUES YA QUE LOS ENEMIGOS TENDRAN VIDA Y NO MORIRAN SOLO POR
            CONTACTO.
        */

        if (collider.gameObject.tag == "Enemigo")
        {
            Juego.controlador.MuerteEnemigo(collider);
        }
    }

}
