using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AreaAtaque : MonoBehaviour
{
    [Tooltip(" Punto de origen de los minions. ")]
    public Transform[] origenes = new Transform[3];

    [Tooltip(" Lugar en donde se encuentran los minions. ")]
    public Transform[] minions = new Transform[3];

    // Esta lista guarda a los enemigos que estén dentro del area de ataque.
    private List<GameObject> enemigosEnRango = new List<GameObject>();

    /*
     * Los puntos actuales dicen hacia que posición se están moviendo los minions
     *   0 - significa que se se mueven hacia un enemigo.
     *   1 - significa que se mueven hacia su punto de origen 
    */
    private int[] puntosActuales = new int[3] { 0, 0, 0 };

    private void Update()
    {
        /*
         *   Lo que hace el update es remover a todos los enemigos de la lista,
         *   despues si no hay ninguno regresa a los minions al origen.
         *
         *   Si hay alguien en la lista entonces se les ataca.
        */
        enemigosEnRango.RemoveAll(item => item == null);

        if (enemigosEnRango.Count == 0)
        {
            RegresarOrigen();
        }
        else
        {
            Atacar(enemigosEnRango.ToArray());
        }
    }


    private void Atacar(GameObject[] todosLosEnemigos)
    {
        /*
         *   Se mueven los minions hacia la dirección del enemigo, 
         *   cuando lo golpean regresan a su origen.
        */

        /*
         *   Ordenamos la lista de enemigos.
         *
         *  - .OrderBy() - Es la función principal que se usa para ordenar.
         *  - enemigo => ... - Significa, "Para cada enemigo en la lista, usa el siguiente valor para ordenarlo".
         *  - Vector3.Distance - Calcula el valor entre jugador y el enemigo 
         *  - .ToList() - Convierte el resultado en una lista.
        */
        var enemigosOrdenados = todosLosEnemigos.OrderBy(enemigo => Vector3.Distance(Juego.controlador.RegresaTransformJugador().position, enemigo.transform.position)).ToList();

        // Calcula la cantidad de los enemigos en la lista.
        int numEnemigosEncontrados = enemigosOrdenados.Count;

        for (int i = 0; i < 3; i++)
        {
            /*
             *  Esta es una de las partes más importantes, esta es la linea que distribuye los minios entre los enemigos.
             *
             *   Ejemplos de cómo funciona:
             *
             *   Caso 1: Cuando hay un enemigo  
             *       - Minion 0: 0 % 1 = 0  (Va al enemigo 1)
             *       - Minion 1: 1 % 1 = 0  (Va al enemigo 1)
             *       - Minion 2: 2 % 1 = 0  (Va al enemigo 1)
             *
             *   Caso 2: Cuando hay dos enemigos 
             *       - Minion 0: 0 % 2 = 0  (Va al enemigo 1)
             *       - Minion 1: 1 % 2 = 1  (Va al enemigo 2)
             *       - Minion 2: 2 % 2 = 0  (Va al enemigo 1)
             *
             *   Caso 3: Cuando hay tres enemigos
             *       - Minion 0: 0 % 3 = 0  (Va al enemigo 1)
             *       - Minion 1: 1 % 3 = 1  (Va al enemigo 2)
             *       - Minion 2: 2 % 3 = 2  (Va al enemigo 3)
             *
             *   Todos los demás casos se engloban en el tercero, pero lo que hace es
             *   tomar los primeros 3 enemigos más cercanos e ignorar a los demás.
             */
            GameObject enemigoAsignado = enemigosOrdenados[i % numEnemigosEncontrados];

            // Se crea la ruta que seguirá el minion, primero va al enemigo, luego vuelve al origen.
            Transform[] ruta = new Transform[] { enemigoAsignado.transform, origenes[i].transform };

            // Se utiliza MoveTowards para mover la posición del minion.
            /*
             *   Variables de MoveTowards:
             *   - La posición inicial desde donde se moverá.
             *   - La posición del destino.
             *   - La distancia máxima que recorre en un frame.
            */
            minions[i].transform.position = Vector3.MoveTowards(minions[i].transform.position, ruta[puntosActuales[i]].position, 0.5f * Juego.controlador.RegresaVelocidadMinion() * Time.deltaTime);

            /*
             *   Si la distancia está lo suficientemente cerca del destino 
             *   entonces cambiamos a 0 por 1 o viceversa
            */
            if (Vector3.Distance(minions[i].transform.position, ruta[puntosActuales[i]].position) < 0.9f)
            {
                puntosActuales[i] = (puntosActuales[i] == 0) ? 1 : 0;
            }
        }
    }

    private void RegresarOrigen()
    {
        /*
         *   Regresa al origen a los minions
        */
        for (int i = 0; i < 3; i++)
        {
            puntosActuales[i] = 0;
            minions[i].transform.position = Vector3.MoveTowards(minions[i].transform.position, origenes[i].transform.position, 2.0f * Juego.controlador.RegresaVelocidadMinion() * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
         *   Si un enemigo entra a la zona, lo añadimos a la lista.
        */
        if (other.CompareTag("Enemigo"))
        {
            if (!enemigosEnRango.Contains(other.gameObject))
            {
                enemigosEnRango.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        /*
         *   Si un enemigo sale de la zona, lo quitamos de la lista.
        */
        if (other.CompareTag("Enemigo"))
        {
            if (enemigosEnRango.Contains(other.gameObject))
            {
                enemigosEnRango.Remove(other.gameObject);
            }
        }
    }
}
