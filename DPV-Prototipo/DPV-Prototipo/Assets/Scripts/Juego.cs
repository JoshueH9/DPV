using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juego : MonoBehaviour
{
    /* 
        Esta variable servira como controlador del juego, servira para llevar un mejor control
        de todas las variables que afectan cualquier variable dentro del juego.
    */
    public static Juego controlador;

    /*
    
        [Header("  ")] - Sirve para tener una descripción rapida de la variable.

        [Tooltip("  ")] - Sirve para poner una descripción a la variable, 
                          para ver la descripción se pone el mouse encima de la variable
                          en el inspector de Unity.

        [Space( x )] - Añade un espacios de x píxeles ente lineas en el inspector.

        Debug.Log() - Imprimir en consola.

    */

    /* --------------------------------------------- DATOS DEL JUEGO --------------------------------------------- */
    [Header(" DATOS DEL JUEGO. ")]
    [Space(5)]

    [Tooltip(" Tiempo de juego total. ")]
    public float tiempoJuego = 0;

    /* ----------------------------------------------------------------------------------------------------------- */


    /* --------------------------------------------- DATOS DEL JUGADOR --------------------------------------------- */
    [Space(30)]
    [Header(" DATOS DEL JUGADOR. ")]
    [Space(5)]

    [Tooltip(" Lugar en donde se encuentra el jugador. ")]
    public Transform jugador;

    [Tooltip(" Experiencia del jugador. ")]
    public int experiencia = 0;

    [Tooltip(" Nivel del jugador. ")]
    public int nivel = 0;

    /* ------------------------------------------------------------------------------------------------------------- */



    /* --------------------------------------------- DATOS DE POWERUPS --------------------------------------------- */

    [Space(30)]
    [Header(" DATOS DE POWERUPS. ")]
    [Space(5)]

    [Tooltip("Cantidad de tiempo que duran los powerUp a las estadisticas del jugador.")]
    public float tiempoPowerUp = 30.0f;

    [Tooltip(" La cantidad de aumento que dan los powerUp a las estadisticas del jugador.")]
    public float modificacionPowerUp = 30.0f;

    [Space(10)]

    [Tooltip(" Variable que indica cuando 'AumentoEstadisticas' está activo.")]
    public bool aumentoEstadisticas = false; 

    [Tooltip(" Es el tiempo restante del aumento de velocidad de movimiento" +
            " en los minions que dio el powerUp 'AumentoEstadisticas'. ")]
    public float tiempoRestanteAE = 0;

    

    /* ------------------------------------------------------------------------------------------------------------- */



    /* --------------------------------------------- ESTADISTICAS DEL PERSONAJE --------------------------------------------- */

    [Space(30)]
    [Header(" ESTADISTICAS DEL PERSONAJE. ")]
    [Space(5)]

    [Tooltip(" Velocidad del jugador. ")]
    public float velocidadJugador = 0;

    [Tooltip(" Velocidad a la que se mueven los minions del jugador. ")]
    public float velocidadMinion = 10.0f;

    /* ----------------------------------------------------------------------------------------------------------------------- */



    /* --------------------------------------------- DATOS DE ENEMIGOS --------------------------------------------- */

    [Space(30)]
    [Header(" DATOS DE ENEMIGOS. ")]
    [Space(5)]

    [Tooltip(" Velocidad de movimiento del enemigo. ")]
    public int velocidadEnemigo = 0;

    /* ------------------------------------------------------------------------------------------------------------- */



    /* --------------------------------------------- OBJETOS PREFABRICADOS --------------------------------------------- */

    [Space(30)]
    [Header(" OBJETOS PREFABRICADOS. ")]
    [Space(5)]

    [Tooltip(" GameObject prefab del enemigo. ")]
    public GameObject goEnemigo;

    [Tooltip(" GameObject prefab de la experiencia. ")]
    public GameObject goExperiencia;

    [Tooltip(" GameObject prefab de la bomba nuclear. ")]
    public GameObject goBombaNuclear;

    [Tooltip(" GameObject prefab del Iman. ")]
    public GameObject goIman;

    [Tooltip(" GameObject prefab del AumentoEstadisticas. ")]
    public GameObject goAumentoEstadisticas;



    /* ----------------------------------------------------------------------------------------------------------------- */



    /* --------------------------------------------- FUNCIONES --------------------------------------------- */


    private void Awake()
    {
        /*
            Se comprueba si ya existe un controlador, si no existe lo crea, en caso contrario la reemplaza.
        */
        if (controlador == null)
        {
            controlador = this;

            // DontDestroyOnLoad(gameObject); Evita que se destruya el controlador si se cambia de escena (Por si implementamos niveles).
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        tiempoJuego = Time.time;

        if (aumentoEstadisticas)
        {
            ActualizaTiempoRestanteAE(-Time.deltaTime);
        }
        
    }

    /* --------------------------------------------- FUNCIONES PARA DATOS DEL JUEGO --------------------------------------------- */

    public float RegresaTiempoTranscurrido()
    {
        /*
            Regresa el tiempo transcurrido del juego.
         */
        return tiempoJuego;
    }



    /* --------------------------------------------- FUNCIONES PARA DATOS DEL JUGADOR --------------------------------------------- */

    public Transform RegresaTransformJugador()
    {
        /*
            Regresa el transform del jugador.
         */
        return jugador;
    }

    public float RegresaVelocidadJugador()
    {
        /*
            Regresa la velocidad de movimiento del jugador.
         */
        return velocidadJugador;
    }

    public void SumarExperiencia(int xp)
    {
        /*
            Suma experiencia al total del jugador y verifica si el personaje subió de nivel.
        */

        experiencia += xp;

        int nivelActual = experiencia / 1000;

        if (nivelActual > nivel)
        {
            int nivelesSubidos = nivelActual - nivel;
            nivel = nivelActual;
            SubirNivel(nivelesSubidos); 
        }
    }

    public void SubirNivel(int niveles)
    {
        /*
            Cada que el personaje sube de nivel tiene que escojer mejoras, dependiendo de cuantos niveles subio son las iteraciones 
            en las que el for le mostrará el menú para subir sus habilidades.
         */

        for (int i = 0; i < niveles; i++)
        {
            ModificarVelocidadMinion(0.5f);        
        }
    }

    public int RegresaNiveles()
    {
        /*
            Regresa un entero con el nivel del jugador,
            Cada 1000 puntos de experiencia se considera un nivel.
        */

        return nivel;
    }

    /* --------------------------------------------- FUNCIONES PARA DATOS DE POWERUPS --------------------------------------------- */

    public void ActualizaTiempoRestanteAE(float modificacionTiempo)
    {
        /*
         * Actualiza el contador de tiempo del aumento "AumentoEstadisticas" cuando el jugador toma el aumento
         * o se quiere actualizar el tiempo que queda.
         */

        tiempoRestanteAE += modificacionTiempo;


        /* 
         * Caso para cuando se está restando tiempo y llega a 0.
         * Entonces se resta la modificación que se hizo anteriormente y el tiempo se vuelve 0.
         */
        if (tiempoRestanteAE <= 0)
        {
            aumentoEstadisticas = false;
            ModificarVelocidadMinion(-modificacionPowerUp);
            tiempoRestanteAE = 0;
        }     

        /* 
         * Caso donde al aplicar la modificación se pasa del valor máximo permitido.
         */

        if(tiempoRestanteAE > tiempoPowerUp)
        {
            tiempoRestanteAE = tiempoPowerUp;
        }
    }

    public void AumentarEstadisticasAE()
    {
        /* 
         * Aumenta las estadisticas del jugador segun el powerUp "AumentarEstadisticas"
         */

        if (tiempoRestanteAE <= 0)
        {
            ModificarVelocidadMinion(modificacionPowerUp);
            aumentoEstadisticas = true;
        }

        ActualizaTiempoRestanteAE(tiempoPowerUp); 
    }

    public float RegresaTiempoRestanteAE()
    {
        /*
            Regresa cuanto tiempo queda el aumento del powerUp de "AumentoEstadisticas",
            además que si detecta que el contador llegó a 0 entonces quita el aumento del powerUp.
         */

        return tiempoRestanteAE;
    }

    /* --------------------------------------------- FUNCIONES PARA ESTADISTICAS DEL PERSONAJE --------------------------------------------- */

    public float RegresaVelocidadMinion()
    {
        /*
            Regresa un valor flotante con la velocidad a la que se mueven los minions del jugador hacia el enemigo.
         */
        return velocidadMinion;
    }
    public void ModificarVelocidadMinion(float modificación)
    {
        /*
            Modifica la estadistica de velocidad de disparo de los minions,
            segun el nivel del jugador.

            *** De momento tenemos una formula lineal muy sencilla para aumentar el valor de la velocidad 
                del jugador cada que sube de nivel. --- 10 + (0.5 * NivelDelJugador).
         */

        velocidadMinion += modificación;
    }

    /* --------------------------------------------- FUNCIONES PARA DATOS DE ENEMIGOS --------------------------------------------- */

    public float RegresaVelocidadEnemigo()
    {
        return velocidadEnemigo;
    }

    public void MuerteEnemigo(Collider collider)
    {
        /*
            Función que simula la muerte del enemigo, genera un objeto de experiencia,
            da oro al jugador y tiene una probabilidad de soltar un powerUp. 
         */
        Instantiate(goExperiencia, collider.gameObject.transform.position, collider.gameObject.transform.rotation);

        // Probabilidad del 2%.
        if (Random.Range(0.0f, 1.0f) < 0.02f)
        {
            int opcion = Random.Range(0, 3);

            switch (opcion)
            {
                case 0:
                    Instantiate(goBombaNuclear, collider.gameObject.transform.position, collider.gameObject.transform.rotation);
                    break;
                case 1:
                    Instantiate(goIman, collider.gameObject.transform.position, collider.gameObject.transform.rotation);
                    break;
                case 2:
                    Instantiate(goAumentoEstadisticas, collider.gameObject.transform.position, collider.gameObject.transform.rotation);
                    break;
                default:
                    break;
            }
        }

        Destroy(collider.gameObject);
    }

    /* --------------------------------------------- FUNCIONES PARA OBJETOS PREFABRICADOS --------------------------------------------- */

    public GameObject RegresaEnemigo()
    {
        return goEnemigo;
    }
}
