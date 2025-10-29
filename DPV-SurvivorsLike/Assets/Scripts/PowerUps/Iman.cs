using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iman : MonoBehaviour
{
    [Tooltip("La velocidad de la experiencia llendo hacia el jugador.")]
    public float velocidadAtraccion = 40.0f;

    [Tooltip("Tiempo de gracia que se toma el iman antes de destruirse. " +
        "(Es importante mantener un valor alto ya que el jugador puede estar " +
        "bastante tiempo escogiendo sus mejoras cuando sube de nivel).")]
    public float duracionEfecto = 120.0f;

    // Una variable que nos servirá para guardar toda la lista de objetos experiencia.
    private GameObject[] experiencias = new GameObject[1];

    /*
        Componente visual, servirá para desactivarlo más tarde, así el jugador no lo verá
        hasta que pase el tiempo de gracia en donde será destruido.
     */
    private MeshRenderer meshIman;

    /*
        Componente collider, servirá para desactivarlo más tarde, así el jugador no chocará
        accidentalmente con el por estar "invisible".
     */
    private Collider colliderIman;

    private void Awake()
    {
        meshIman = GetComponent<MeshRenderer>();
        colliderIman = GetComponent<Collider>();
    }

    private void Update()
    {
        // Solo ejecuta la función si hay objetos experiencia en la lista.
        if (experiencias.Length != 0)
        {
            RecolectarExperiencia();
        }
    }

    private void RecolectarExperiencia()
    {
        /*
            Atrae toda la experiencia del mapa hacia el jugador.
         */
        Vector3 posicionJugador = Juego.controlador.RegresaTransformJugador().position;

        foreach (GameObject experiencia in experiencias)
        {
            // Importante verificar si el objeto experiencia es nulo para que no interrumpa el proceso de los demás.
            if (experiencia != null)
            {
                experiencia.transform.position = Vector3.MoveTowards(experiencia.transform.position, posicionJugador, velocidadAtraccion * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        /*
            Si el objeto con el que entra en contacto es de tipo "Jugador",
            entonces, guarda en la lista todos los objetos experiencia del momento.
        */

        // Evita que se active varias veces.
        if (experiencias.Length == 0) 
            return;

        if (collider.gameObject.tag == "Player")
        {
            experiencias = GameObject.FindGameObjectsWithTag("Experiencia");

            meshIman.enabled = false;
            colliderIman.enabled = false;

            Destroy(this.gameObject, duracionEfecto);
        }
    }
}
