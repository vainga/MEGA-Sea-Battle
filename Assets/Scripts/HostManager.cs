using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostManager : NetworkManager
{
    public void StartHostNetwork()
    {
        if (NetworkServer.active || NetworkClient.active)
            return;

        StartHost();
        Debug.Log("Host IP: " + networkAddress);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        if (NetworkServer.connections.Count == 2)
        {
            PlayerController playerController = FindObjectOfType<PlayerController>();
            if (playerController != null)
            {
                playerController.RpcLoadNextScene();
            }
        }
    }
}
