using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Jugador : NetworkBehaviour
{
    NetworkVariable<Vector3> posicionJugador = new NetworkVariable<Vector3>(); 

    void Start()
    {
        this.posicionJugador.OnValueChanged += OnCambioPosicionJugador;
        if (IsOwner){
            PosicionAleatoriaJugadorInicioServerRpc();
        }
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
    }
    [ServerRpc]
    public void PosicionAleatoriaJugadorEquipo2ServerRpc(ServerRpcParams parametros = default){
        Vector3 posicionAleatoriaDerecha = new Vector3(Random.Range(-5, 5), 0, Random.Range(2, 5));
        this.posicionJugador.Value = posicionAleatoriaDerecha;
    }
    private Vector3 DevolverPosicionAleatoriaCentroPlano(){
        return new Vector3(Random.Range(-5, 5), 0, Random.Range(-1, 1));
    }
}
