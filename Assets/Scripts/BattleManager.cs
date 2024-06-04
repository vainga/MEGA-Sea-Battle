using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public GridManager gridManager1;
    public GridManager gridManager2;
    public GameObject winMenu;
    public GameObject darkOverlay;
    public TextMeshProUGUI menuText;

    private bool isPlayer1Turn = false;
    private int player1KilledCells = 0;
    private int player2KilledCells = 0;
    private const int cellsToWin = 20;



    private void Start()
    {
        var player1OccupiedCells = GameManager.Instance.Player1OccupiedCells;
        gridManager1.FillOccupiedCells(player1OccupiedCells);

        var player2OccupiedCells = GameManager.Instance.Player2OccupiedCells;
        Debug.Log(player2OccupiedCells.Count);
        gridManager2.FillOccupiedCells(player2OccupiedCells);

        UpdateGridInteractivity();
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

                if (isPlayer1Turn)
                {
                    player1KilledCells++;
                }
                else
                {
                    player2KilledCells++;
                }

                GridManager gridManager = cellManager.GetComponentInParent<GridManager>();
                bool hasAdjacentOccupiedCell = gridManager.HasAdjacentOccupiedCell(cellManager.cell);
                Debug.Log($"Has adjacent occupied cell: {hasAdjacentOccupiedCell}");

                var consecutiveOccupiedCells = FindConsecutiveOccupiedCells(cellManager.cell, gridManager);

                if (AllCellsKilled(consecutiveOccupiedCells))
                {
                    foreach (var occupiedCell in consecutiveOccupiedCells)
                    {
                        ActivateTochkaInRadius(occupiedCell, gridManager);
                    }
                }

                CheckForWin();
            }
            else
            {
                tochkaObject.SetActive(true);
                SwitchTurn(); // Переключаем ход только в случае промаха
            }

            UpdateGridInteractivity();
        }
    }

    private bool AllCellsKilled(List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            if (!cell._isKilled)
            {
                return false;
            }
        }
        return true;
    }

    private List<Cell> FindConsecutiveOccupiedCells(Cell startCell, GridManager gridManager)
    {
        List<Cell> consecutiveCells = new List<Cell> { startCell };
        int[] dx = { -1, 0, 1, 0 };
        int[] dy = { 0, -1, 0, 1 };

        foreach (var direction in new[] { 0, 1, 2, 3 })
        {
            int newX = startCell.PosX + dx[direction];
            int newY = startCell.PosY + dy[direction];

            while (newX >= 0 && newX < 10 && newY >= 0 && newY < 10)
            {
                Cell adjacentCell = gridManager.cellList.Find(c => c.PosX == newX && c.PosY == newY);
                if (adjacentCell != null && !adjacentCell.IsEmpty)
                {
                    consecutiveCells.Add(adjacentCell);
                    newX += dx[direction];
                    newY += dy[direction];
                }
                else
                {
                    break;
                }
            }
        }

        return consecutiveCells;
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
                    if (adjacentCellManager != null && adjacentCell.IsEmpty)
                    {
                        adjacentCellManager.ActivateTochka();
                        adjacentCellManager.isClicked = true;
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

    private void SwitchTurn()
    {
        isPlayer1Turn = !isPlayer1Turn;
    }

    private void UpdateGridInteractivity()
    {
        foreach (var cellManager in gridManager1.GetComponentsInChildren<CellManager>())
        {
            cellManager.SetInteractivity(isPlayer1Turn);
        }

        foreach (var cellManager in gridManager2.GetComponentsInChildren<CellManager>())
        {
            cellManager.SetInteractivity(!isPlayer1Turn);
        }
    }

    private void CheckForWin()
    {
        if (player1KilledCells >= cellsToWin || player2KilledCells >= cellsToWin)
        {
            menuText.gameObject.SetActive(true);
            if(player2KilledCells >= cellsToWin)
                menuText.text = "Победил " + GameManager.Instance.player1._playerName;
            else
                menuText.text = "Победил " + GameManager.Instance.player2._playerName;

            winMenu.SetActive(true);
            darkOverlay.SetActive(true);
        }
    }
}
