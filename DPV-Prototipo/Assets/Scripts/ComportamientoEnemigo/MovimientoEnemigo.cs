using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    void Update()
    {
        /*
            El enemigo siempre caminará recto hacia la dirección del jugador.
        
            Como se trabaja con un prefabricado es mejor buscar al jugador por su tag que directamente con el inspector.
        */
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        // Se utiliza MoveTowards para mover la posición del enemigo al jugador.
        /*
            Variables de MoveTowards:
            - La posición inicial desde donde se moverá.
            - La posición del destino.
            - La distancia máxima que recorre en un frame.
        */
        this.transform.position = Vector3.MoveTowards(this.transform.position, jugador.transform.position, 0.5f * Juego.controlador.RegresaVelocidadEnemigo() * Time.deltaTime);
    }
}
