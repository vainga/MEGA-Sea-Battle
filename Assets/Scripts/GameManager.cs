using UnityEngine;
using System.Collections.Generic;
using Mirror;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    public Player player1;
    public Player player2;

    public List<Cell> Player1OccupiedCells { get; private set; }
    public List<Cell> Player2OccupiedCells { get; private set; }

    public bool IsPlayer1Ready { get; private set; }
    public bool IsPlayer2Ready { get; private set; }

    // Ќовое поле дл€ хранени€ состо€ни€ перехода
    public bool transitionThroughRpc { get; private set; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Player1OccupiedCells = new List<Cell>();
            Player2OccupiedCells = new List<Cell>();
            transitionThroughRpc = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Ќовый метод дл€ установки состо€ни€ перехода
    public void SetTransitionThroughRpc(bool state)
    {
        transitionThroughRpc = state;
    }

    public void SetPlayerOccupiedCells(int player, List<Cell> occupiedCells)
    {
        List<Cell> copiedCells = new List<Cell>();
        foreach (Cell cell in occupiedCells)
        {
            copiedCells.Add(new Cell(cell.PosX, cell.PosY) { IsEmpty = cell.IsEmpty });
        }

        if (player == 1)
        {
            Player1OccupiedCells = copiedCells;
        }
        else if (player == 2)
        {
            Player2OccupiedCells = copiedCells;
        }
    }

    public void SetPlayerReady(int player, bool isReady)
    {
        List<Cell> playerOccupiedCells = player == 1 ? Player1OccupiedCells : Player2OccupiedCells;

        // ѕодсчет количества клеток, зан€тых игроком
        int occupiedCount = playerOccupiedCells.Count(cell => !cell.IsEmpty);

        // ≈сли количество зан€тых клеток меньше 20, вывести сообщение об ошибке и не устанавливать готовность
        if (occupiedCount < 20)
        {
            Debug.LogError("Ќельз€ быть готовым, если зан€то меньше 20 клеток!");
            return;
        }

        // ”становка готовности игрока
        if (player == 1)
        {
            IsPlayer1Ready = isReady;
        }
        else if (player == 2)
        {
            IsPlayer2Ready = isReady;
        }

        // ≈сли оба игрока готовы, производим переход на сцену битвы
        if (IsPlayer1Ready && IsPlayer2Ready)
        {
            SetTransitionThroughRpc(true);
            NetworkManager.singleton.ServerChangeScene("BattleScene");
        }
    }
    //public void SetPlayerReady(bool isReady)
    //{
    //    if (NetworkServer.active)
    //    {
    //        IsPlayer1Ready = isReady;
    //    }
    //    else
    //    {
    //        IsPlayer2Ready = isReady;
    //    }

    //    if (IsPlayer1Ready && IsPlayer2Ready)
    //    {
    //        SetTransitionThroughRpc(true);
    //        NetworkManager.singleton.ServerChangeScene("BattleScene");
    //    }
    //}
}
