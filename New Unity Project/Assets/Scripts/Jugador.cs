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
    NetworkVariable<Color> colorJugador = new NetworkVariable<Color>();
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
            PosicionAleatoriaJugadorInicioServerRpc();
        }
        {
            this.renderizador.material.SetColor("_Color", colorJugador.Value);
        }
    }

    private void OnCambioEquipo(int valorAnterior, int nuevoEquipo)
    {
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
    public void PosicionAleatoriaJugadorInicioServerRpc(ServerRpcParams parametros = default)
    {
        this.posicionJugador.Value = DevolverPosicionAleatoriaCentroPlano();
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
        } else {
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
        Vector3 posicionAleatoriaCentral = new Vector3(Random.Range(-5, 5), 0, Random.Range(1.5f, 1.5f));
        this.posicionJugador.Value = posicionAleatoriaCentral;
        this.colorJugador.Value = Color.white;
    }
    private Vector3 DevolverPosicionAleatoriaCentroPlano()
    {
        return new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
    }

}
