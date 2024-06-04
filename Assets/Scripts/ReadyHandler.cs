using Mirror;
using UnityEngine;

public class ReadyHandler : NetworkBehaviour
{
    public override void OnStartServer()
    {
        NetworkServer.RegisterHandler<ClientManager.ReadyMessage>(OnClientReadyMessage);
    }

    private void OnClientReadyMessage(NetworkConnectionToClient conn, ClientManager.ReadyMessage msg)
    {
        Debug.Log("Client ready: " + conn.connectionId);

        CheckAllClientsReady();
    }

    private void CheckAllClientsReady()
    {
        bool allClientsReady = true; 

        if (allClientsReady)
        {
            NetworkManager.singleton.ServerChangeScene("BattleScene");
        }
    }
}
