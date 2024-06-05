using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [ClientRpc]
    public void RpcLoadNextScene()
    {
        // Устанавливаем состояние перехода через RPC
        GameManager.Instance.SetTransitionThroughRpc(true);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
