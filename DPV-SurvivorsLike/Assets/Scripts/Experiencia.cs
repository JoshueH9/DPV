using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiencia : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        /*
            Si el objeto con el que entra en contacto es de tipo "Player"
            destruye el objeto y manda a sumar experiencia con el controlador.
        */

        if (collider.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            Juego.controlador.SumarExperiencia(100);
        }
    }
}
