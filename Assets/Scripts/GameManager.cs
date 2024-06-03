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
}
