using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpawnEnemigos : MonoBehaviour
{
    [Tooltip(" Lugar de spawn de enemigos. ")]
    public Transform[] spawns = new Transform[3];
    [Tooltip(" GameObject prefab del enemigo. ")]
    public GameObject enemigo;

    [Tooltip(" Tiempo para máxima dificultad, este será el tiempo que le toma al juego llegar a su máxima dificultad.  ")]
    private float tiempoMaximo = 600.0f;

    void Start()
    {
        StartCoroutine(SubirSpawn());
    }

    private void Update()
    {
        SubirSpawn();
    }

private IEnumerator SubirSpawn()
{
    /*
        Corrutina en donde se va llamando cada cierto tiempo a la
        función spawn de enemigos.

        Las llamadas a spawn() al inicio del juego se hace cada 2.0f segundos,
        el juego baja ese valor hasta 0.1f, el tiempo que tarde depende 
        de la variable "tiempoMaximo", al inicio este valor baja rápido y conforme
        avanza el juego se hace un poco más lento.
    */

    while (true)
    {
        float tiempoActual = Juego.controlador.RegresaTiempoTranscurrido();

        float fraccion = tiempoActual / tiempoMaximo;

        if (fraccion > 1.0f)
            fraccion = 1.0f;

        float exponente = (float)Math.Pow((1.0f - fraccion), 2.0f);

        float tiempoDeEspera = 0.1f + (1.9f * exponente);

        Spawn();

        yield return new WaitForSeconds(tiempoDeEspera);
    }
}

private void Spawn()
{
    /*
        Genera un enemigo en la posición de cada punto del arreglo.
    */

    for (int i = 0; i < 3; i++)
    {
        Transform punto = spawns[i];

        GameObject enem = Instantiate(enemigo, punto.position, punto.rotation);
    }
}
}
