using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public GridManager gridManager1;
    public GridManager gridManager2;

    private void Start()
    {
        var player1OccupiedCells = GameManager.Instance.Player1OccupiedCells;
        gridManager1.FillOccupiedCells(player1OccupiedCells);

        var player2OccupiedCells = GameManager.Instance.Player2OccupiedCells;
        Debug.Log(player2OccupiedCells.Count);
        gridManager2.FillOccupiedCells(player2OccupiedCells);
    }

    public void LogCellStatus(CellManager cellManager)
    {
        if (SceneManager.GetActiveScene().name == "BattleScene")
        {
            Debug.Log($"Cell coordinates: {cellManager.cell.PosX}, {cellManager.cell.PosY}, isEmpty: {cellManager.cell.IsEmpty}");

            GameObject krestObject = cellManager.transform.Find("KREST").gameObject;
            GameObject tochkaObject = cellManager.transform.Find("TOCHKA").gameObject;

            if (!cellManager.cell.IsEmpty)
            {
                cellManager.cell._isKilled = true;
                krestObject.SetActive(true);

                GridManager gridManager = cellManager.GetComponentInParent<GridManager>();
                bool hasAdjacentOccupiedCell = gridManager.HasAdjacentOccupiedCell(cellManager.cell);
                Debug.Log($"Has adjacent occupied cell: {hasAdjacentOccupiedCell}");

                if (!hasAdjacentOccupiedCell)
                {
                    ActivateTochkaInRadius(cellManager.cell, gridManager);
                }
            }
            else
            {
                tochkaObject.SetActive(true);
            }
        }
    }

    private void ActivateTochkaInRadius(Cell cell, GridManager gridManager)
    {
        int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
        int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };

        for (int i = 0; i < dx.Length; i++)
        {
            int newX = cell.PosX + dx[i];
            int newY = cell.PosY + dy[i];

            if (newX >= 0 && newX < 10 && newY >= 0 && newY < 10)
            {
                Cell adjacentCell = gridManager.cellList.Find(c => c.PosX == newX && c.PosY == newY);
                if (adjacentCell != null)
                {
                    CellManager adjacentCellManager = FindCellManager(newX, newY, gridManager);
                    if (adjacentCellManager != null)
                    {
                        adjacentCellManager.ActivateTochka();
                    }
                }
            }
        }
    }

    private CellManager FindCellManager(int x, int y, GridManager gridManager)
    {
        foreach (var cellManager in FindObjectsOfType<CellManager>())
        {
            if (cellManager.cell.PosX == x && cellManager.cell.PosY == y && cellManager.GetComponentInParent<GridManager>() == gridManager)
            {
                return cellManager;
            }
        }
        return null;
    }
}
