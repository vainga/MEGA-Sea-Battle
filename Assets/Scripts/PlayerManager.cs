using UnityEngine;

using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player player;

    void Start()
    {
        player = new Player("PlayerName");

        Debug.Log("Player name: " + player._playerName);

    }
}

