using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientManager : MonoBehaviour
{
    public TMP_InputField ipInputField;
    private NetworkManager networkManager;
    private static ClientManager instance;

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
        NetworkClient.RegisterHandler<SceneMessage>(OnSceneMessageReceived);
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

    private void OnSceneMessageReceived(SceneMessage msg)
    {
        Debug.Log("Scene message received: " + msg.sceneName);
        SceneManager.LoadScene(msg.sceneName);
    }

    public struct ReadyMessage : NetworkMessage { }

    public struct SceneMessage : NetworkMessage
    {
        public string sceneName;
    }
}
