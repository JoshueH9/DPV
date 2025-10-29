using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaNuclear : MonoBehaviour
{
    [Tooltip(" Radio de la explosión de la bomba. ")]
    public float radioExplosion = 50.0f;

    private void OnTriggerEnter(Collider collider)
    {
        /*
            Si el objeto con el que entra en contacto es de tipo "Jugador",
            entonces se destruye a si mismo para crear el area que mata a los enemigos.
        */

        if (collider.gameObject.tag == "Player")
        {
            Vector3 posExplosion = this.transform.position;

            Destroy(this.gameObject);

            // Se utiliza OverlapSphere para recopilar a todos los colliders dentro de la zona de explosión.
            /*
                Variables de OverlapSphere:
                - La posición desde donde se genera la esfera.
                - El radio de la explosión.
            */
            Collider[] enemigosAtrapados = Physics.OverlapSphere(posExplosion, radioExplosion);

            // Eliminamos a toda la lista de enemigos.
            foreach (Collider enemigo in enemigosAtrapados)
            {
                if (enemigo.CompareTag("Enemigo"))
                {
                    Juego.controlador.MuerteEnemigo(enemigo);
                }
            }
        }
    }
}
