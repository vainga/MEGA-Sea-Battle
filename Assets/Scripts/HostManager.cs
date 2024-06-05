using Mirror;
using UnityEngine;

public class HostManager : NetworkManager
{
    private static HostManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartHostNetwork()
    {
        if (NetworkServer.active || NetworkClient.active)
            return;

        StartHost();
        Debug.Log("Host IP: " + networkAddress);
        string localIPAddress = IPManager.GetLocalIPAddress();
        Debug.Log("Local IP address: " + localIPAddress);
    }

    public void StopHostNetwork()
    {
        if (NetworkServer.active)
        {
            StopHost();
            Debug.Log("Server stopped.");
        }
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
