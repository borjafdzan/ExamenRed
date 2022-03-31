using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Jugador : NetworkBehaviour
{
    Renderer renderizador;
    NetworkVariable<Vector3> posicionJugador = new NetworkVariable<Vector3>(); 
    NetworkVariable<Color> colorJugador = new NetworkVariable<Color>(new Color(1, 1, 1, 1));

    void Start()
    {
        this.renderizador = GetComponent<Renderer>();
        this.posicionJugador.OnValueChanged += OnCambioPosicionJugador;
        this.colorJugador.OnValueChanged += OnCambiarColorJugador;

        if (IsOwner){
            PosicionAleatoriaJugadorInicioServerRpc();
        } {
            this.renderizador.material.SetColor("_Color", colorJugador.Value); 
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
    private void PosicionAleatoriaJugadorInicioServerRpc(ServerRpcParams parametros = default){
        this.posicionJugador.Value = DevolverPosicionAleatoriaCentroPlano();
    }
    
    [ServerRpc]
    public void PosicionAleatoriaJugadorEquipo1ServerRpc(ServerRpcParams parametros = default){
        Vector3 posicionAleatoriaIzquierda = new Vector3(Random.Range(-5, 5), 0, Random.Range(-2, -5));
        this.posicionJugador.Value = posicionAleatoriaIzquierda;
        this.colorJugador.Value = Color.blue;
    }
    [ServerRpc]
    public void PosicionAleatoriaJugadorEquipo2ServerRpc(ServerRpcParams parametros = default){
        Vector3 posicionAleatoriaDerecha = new Vector3(Random.Range(-5, 5), 0, Random.Range(2, 5));
        this.posicionJugador.Value = posicionAleatoriaDerecha;
        this.colorJugador.Value = Color.red;
    }
    private Vector3 DevolverPosicionAleatoriaCentroPlano(){
        return new Vector3(Random.Range(-5, 5), 0, Random.Range(-1, 1));
    }
}
