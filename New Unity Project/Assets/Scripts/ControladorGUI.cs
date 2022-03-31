using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ControladorGUI : MonoBehaviour
{
    public GameObject MenuPrincipal;
    public void OnClickBtnCliente()
    {
        NetworkManager.Singleton.StartClient();
        MenuPrincipal.SetActive(false);
    }

    public void OnClickBtnHost()
    {
        NetworkManager.Singleton.StartHost();
        MenuPrincipal.SetActive(false);
    }

    public void OnClickBtnServidor()
    {
        NetworkManager.Singleton.StartServer();
        MenuPrincipal.SetActive(false);
    }

    public void OnClickEquipo1()
    {
        NetworkObject playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        Jugador jugador = playerObject.GetComponent<Jugador>();
        jugador.PosicionAleatoriaJugadorEquipo1ServerRpc();
    }
    public void OnClickEquipo2()
    {
        NetworkObject playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        Jugador jugador = playerObject.GetComponent<Jugador>();
        jugador.PosicionAleatoriaJugadorEquipo2ServerRpc();
    }
    public void OnClickEquipoSinEquipo()
    {
        NetworkObject playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        Jugador jugador = playerObject.GetComponent<Jugador>();
        jugador.PosicionAleatoriaJugadorSinEquipoServerRpc();
    }
}
