using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHorda : MonoBehaviour
{
    [Tooltip(" Lugar de spawn de enemigos. ")]
    public Transform[] spawns = new Transform[20];

    [Tooltip(" GameObject prefab del enemigo. ")]
    public GameObject enemigo;

    [Tooltip(" Es el tiempo que tarda el juego entre hordas grandes.  ")]
    private float tiempoEntreHordas = 20.0f;

    [Tooltip(" Cantidad de oleadas por horda. (Cada oleada son 20 enemigos) . ")]
    private float cantidadOleadas = 1;

    void Start()
    {
        StartCoroutine(SubirSpawn());
    }

    private IEnumerator SubirSpawn()
    {    
        while (true)
        {
            // Tiempo de espera entre las hordas.
            yield return new WaitForSeconds(tiempoEntreHordas);

            for (int i = 0; i < cantidadOleadas; i++)
            {
                Spawn();
                yield return new WaitForSeconds(2.0f);
            }

            cantidadOleadas++;
        }
    }


    private void Spawn()
    {
        /*
            Genera un enemigo en la posición de cada punto del arreglo.
        */

        for (int i = 0; i < 20; i++)
        {
            Transform punto = spawns[i];

            GameObject enem = Instantiate(enemigo, punto.position, punto.rotation);
        }
    }
}