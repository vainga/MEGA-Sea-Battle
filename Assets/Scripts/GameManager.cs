using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    public List<Cell> Player1OccupiedCells { get; private set; }
    public List<Cell> Player2OccupiedCells { get; private set; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Player1OccupiedCells = new List<Cell>();
            Player2OccupiedCells = new List<Cell>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerOccupiedCells(int player, List<Cell> occupiedCells)
    {
        if (player == 1)
        {
            Player1OccupiedCells = occupiedCells;
        }
        else if (player == 2)
        {
            Player2OccupiedCells = occupiedCells;
        }
    }
}
