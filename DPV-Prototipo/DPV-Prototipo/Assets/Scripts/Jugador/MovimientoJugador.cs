using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Tooltip(" Movimiento Vertical. ")]
    public float movVer = 0;
    [Tooltip(" Movimiento Horizontal. ")]
    public float movHor = 0;
    [Tooltip(" Suavizado de movimiento. ")]
    public float suavizado;

    [Tooltip(" Rigidbody del Jugador. ")]
    public Rigidbody rb;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        movHor = Input.GetAxisRaw("Horizontal") * Juego.controlador.RegresaVelocidadJugador();
        movVer = Input.GetAxisRaw("Vertical") * Juego.controlador.RegresaVelocidadJugador();
    }

    private void FixedUpdate()
    {
        mover(movHor * Time.deltaTime, movVer * Time.deltaTime);
    }

    private void mover(float mh, float mv)
    {
        /*
            Registra el movimiento del jugador, 
            en este punto la velocidad ya fué multiplicada 
            y se normalizo con Time.deltaTime.
        */

        Vector3 veli = new Vector3(mh, 0, mv);

        rb.velocity = Vector3.SmoothDamp(rb.velocity, veli, ref veli, suavizado);
    }
}

