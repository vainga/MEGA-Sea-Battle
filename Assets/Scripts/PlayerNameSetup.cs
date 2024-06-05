using UnityEngine;
using Mirror;
using TMPro;

public class PlayerNameSetup : NetworkBehaviour
{
    public TMP_InputField nameInputField;

    [SyncVar(hook = nameof(OnPlayerNameChanged))]
    public string playerName;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        if (isLocalPlayer)
        {
            playerName = "Player 1"; 
            if (isServer)
            {
                playerName = "Player 2"; 
            }
        }
    }

    public void SetPlayerName()
    {
        if (isLocalPlayer)
        {
            CmdSetPlayerName(nameInputField.text);
        }
    }

    [Command]
    public void CmdSetPlayerName(string name)
    {
        playerName = name;
    }

    void OnPlayerNameChanged(string oldName, string newName)
    {
        nameInputField.text = newName;
    }
}
