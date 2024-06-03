using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    private const int gridWidth = 10;
    private const int gridHeight = 10;
    private const float cellSize = 1f;

    public Grid grid;

    public float CellSize
    {
        get
        {
            return cellSize;
        }
    }

    public List<Cell> cellList = new List<Cell>();

    void Start()
    {
        grid = new Grid();
        GenerateGrid();
    }

    void GenerateGrid()
    {
        Vector3 centerPosition = transform.position;
        float gridWidthOffset = (gridWidth - 1) * cellSize / 2;
        float gridHeightOffset = (gridHeight - 1) * cellSize / 2;
        Vector3 startPos = centerPosition - new Vector3(gridWidthOffset, gridHeightOffset, 0f);
        startPos += new Vector3(0f, 9 * cellSize, 0f);

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Vector3 cellPos = startPos + new Vector3(x * cellSize, -y * cellSize, -0.1f);
                Cell cell = new Cell(x, y);
                cellList.Add(cell);

                GameObject newCell = Instantiate(cellPrefab, cellPos, Quaternion.identity, transform);
                CellManager cellManager = newCell.GetComponent<CellManager>();
                cellManager.cell = cell;
            }
        }
    }

    public void ClearGrid()
    {
        foreach (var cell in cellList)
        {
            cell.IsEmpty = true;
        }

        grid.OccupiedCells.Clear();

        foreach (var cellManager in FindObjectsOfType<CellManager>())
        {
            cellManager.ResetCellColor();
        }

        Debug.Log("Grid cleared for next player.");
    }

    public void DeleteOldCells()
    {
        foreach (var cell in cellList)
        {
            if (cell.IsEmpty)
            {
                grid.OccupiedCells.Remove(cell);
            }
        }
    }

    public void PrintOccupiedCells()
    {
        foreach (var cell in cellList)
        {
            if (!cell.IsEmpty)
            {
                Debug.Log($"Cell occupied at: {cell.PosX}, {cell.PosY}");

                if (!grid.OccupiedCells.Contains(cell))
                {
                    DeleteOldCells();
                    grid.AddOccupiedCells(cell);
                }
                Debug.Log(grid.OccupiedCells.Count);
            }
        }
    }
}
