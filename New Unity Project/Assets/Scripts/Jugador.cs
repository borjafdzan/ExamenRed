using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Jugador : NetworkBehaviour
{
    Renderer renderizador;
    public static int numeroJugadoresEquipo1 = 0;
    public static int numeroJugadoresEquipo2 = 0;
    NetworkVariable<Vector3> posicionJugador = new NetworkVariable<Vector3>();
    NetworkVariable<Color> colorJugador = new NetworkVariable<Color>(new Color(1, 1, 1, 1));
    //Este entero representa al equipo al que pertenece el jugador 
    //0 el jugador no tiene equipo
    //1 el jugador es del equipo1
    //2 el jugador es del equipo2
    NetworkVariable<int> equipo = new NetworkVariable<int>(-1);

    void Start()
    {
        this.renderizador = GetComponent<Renderer>();
        this.posicionJugador.OnValueChanged += OnCambioPosicionJugador;
        this.colorJugador.OnValueChanged += OnCambiarColorJugador;
        this.equipo.OnValueChanged += OnCambioEquipo;

        if (IsOwner)
        {
            PosicionAleatoriaJugadorSinEquipoServerRpc();
        }
        {
            this.renderizador.material.SetColor("_Color", colorJugador.Value);
        }
    }

    private void OnCambioEquipo(int valorAnterior, int nuevoEquipo)
    {
        //Comprobamos qeu el jugador antes estaba dentro de uno de los dos equipos si el valor 
        //anterior es 1 o 2 significa que estaba antes en un equipo por lo tanto tenemos
        //que restarle al contador numero de jugadores
        if (valorAnterior == 1 || valorAnterior == 2)
        {
            if (valorAnterior == 1)
            {
                numeroJugadoresEquipo1--;
            }
            else if (valorAnterior == 2)
            {
                numeroJugadoresEquipo2--;
            }
        }
        //Incrementamos la variable estatica de los jugadores por equipo
        if (nuevoEquipo == 1)
        {
            numeroJugadoresEquipo1++;
        }
        else if (nuevoEquipo == 2)
        {
            numeroJugadoresEquipo2++;
        }
    }

    private void OnCambiarColorJugador(Color colorAnterior, Color nuevoColor)
    {
        this.renderizador.material.SetColor("_Color", nuevoColor);
    }

    private void OnCambioPosicionJugador(Vector3 posicionAnterior, Vector3 nuevaPosicion)
    {
        this.transform.position = nuevaPosicion;
    }


    [ServerRpc]
    public void PosicionAleatoriaJugadorEquipo1ServerRpc(ServerRpcParams parametros = default)
    {
        if (Jugador.numeroJugadoresEquipo1 < 2)
        {
            Vector3 posicionAleatoriaIzquierda = new Vector3(Random.Range(-5, 5), 0, Random.Range(-2, -5));
            this.posicionJugador.Value = posicionAleatoriaIzquierda;
            this.colorJugador.Value = Color.blue;
            this.equipo.Value = 1;
        }
        else
        {
            Debug.Log("No se puede asignar el jugador al equipo 1 porque esta lleno");
        }
    }
    [ServerRpc]
    public void PosicionAleatoriaJugadorEquipo2ServerRpc(ServerRpcParams parametros = default)
    {
        if (Jugador.numeroJugadoresEquipo2 < 2)
        {
            Vector3 posicionAleatoriaDerecha = new Vector3(Random.Range(-5, 5), 0, Random.Range(2, 5));
            this.posicionJugador.Value = posicionAleatoriaDerecha;
            this.colorJugador.Value = Color.red;
            this.equipo.Value = 2;
        }
        else
        {
            Debug.Log("no se puede asignar el jugador al equipo 2 porque esta lleno");
        }
    }
    [ServerRpc]
    public void PosicionAleatoriaJugadorSinEquipoServerRpc(ServerRpcParams parametros = default)
    {
        Vector3 posicionAleatoriaCentral = new Vector3(Random.Range(-5, 5), 0, Random.Range(-1, 1));
        this.posicionJugador.Value = posicionAleatoriaCentral;
        this.colorJugador.Value = Color.white;
        //Hacemos esto porque si el jugador esta en un equipo y cambia a sin equipo tenemos que restarle
        //el valor a la variable estatica
        this.equipo.Value = 0;
    }
}
