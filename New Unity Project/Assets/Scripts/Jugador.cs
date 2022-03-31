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
    private Vector3 DevolverPosicionAleatoriaCentroPlano(){
        return new Vector3(Random.Range(-5, 5), 0, Random.Range(-1, 1));
    }
}
