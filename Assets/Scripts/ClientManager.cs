using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour
{
    public TMP_InputField ipInputField;
    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = FindObjectOfType<NetworkManager>();
    }

    public void ConnectToHost()
    {
        if (NetworkClient.isConnected || NetworkServer.active)
            return;

        string ipAddress = ipInputField.text;
        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        NetworkClient.RegisterHandler<ReadyMessage>(OnReadyMessageReceived);
    }

    public void OnClientReady()
    {
        if (NetworkClient.isConnected)
        {
            ReadyMessage readyMessage = new ReadyMessage();
            NetworkClient.Send(readyMessage);
        }
    }

    private void OnReadyMessageReceived(ReadyMessage msg)
    {
        Debug.Log("Ready message received from server.");
    }

    public struct ReadyMessage : NetworkMessage { }
}
