using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Draggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Vector3 initialPosition;
    private Vector3 offset;
    private bool isDragging = false;

    private GridManager gridManager;
    private bool isInsideGrid = false;

    public Ships ship;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.position;
    }

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        offset = rectTransform.position - mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 newPosition = mousePosition + offset;

        isInsideGrid = IsInsideGrid(newPosition);

        if (isInsideGrid)
        {
            newPosition = GetSnappedPosition(newPosition);
        }

        rectTransform.position = newPosition;

        HighlightCells();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        ResetCellsColor();

        if (isInsideGrid)
        {
            PositionInMiddle();
        }
        else
        {
            rectTransform.position = initialPosition;
        }

        gridManager.PrintOccupiedCells();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isDragging)
        {
            // Log the health of the ship
            if (ship != null)
            {
                Debug.Log("Ship Health: " + ship.Health);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    private bool IsInsideGrid(Vector3 position)
    {
        Collider2D gridCollider = gridManager.GetComponent<Collider2D>();
        return gridCollider.OverlapPoint(position);
    }

    private Vector3 GetSnappedPosition(Vector3 position)
    {
        float minDistanceX = float.MaxValue;
        float nearestCellX = position.x;

        float minDistanceY = float.MaxValue;
        float nearestCellY = position.y;

        foreach (var cell in gridManager.cellList)
        {
            CellManager cellManager = FindCellManager(cell);
            if (cellManager != null)
            {
                float distanceX = Mathf.Abs(position.x - cellManager.transform.position.x);
                float distanceY = Mathf.Abs(position.y - cellManager.transform.position.y);

                if (distanceX < minDistanceX)
                {
                    minDistanceX = distanceX;
                    nearestCellX = cellManager.transform.position.x;
                }

                if (distanceY < minDistanceY)
                {
                    minDistanceY = distanceY;
                    nearestCellY = cellManager.transform.position.y;
                }
            }
        }

        return new Vector3(nearestCellX, nearestCellY, position.z);
    }

    private CellManager FindCellManager(Cell cell)
    {
        CellManager[] cellManagers = FindObjectsOfType<CellManager>();
        foreach (var cellManager in cellManagers)
        {
            if (cellManager.cell == cell)
            {
                return cellManager;
            }
        }
        return null;
    }

    void HighlightCells()
    {
        if (ship == null)
        {
            return;
        }

        Vector3[] worldCorners = new Vector3[4];
        rectTransform.GetWorldCorners(worldCorners);

        List<Vector3> raycastPoints = new List<Vector3>();

        for (int i = 0; i < 4; i++)
        {
            Vector3 start = worldCorners[i];
            Vector3 end = worldCorners[(i + 1) % 4];
            raycastPoints.Add(start);
            raycastPoints.Add(Vector3.Lerp(start, end, 1f / 8f)); 
            raycastPoints.Add(Vector3.Lerp(start, end, 2f / 8f)); 
            raycastPoints.Add(Vector3.Lerp(start, end, 3f / 8f)); 
            raycastPoints.Add(Vector3.Lerp(start, end, 4f / 8f)); 
            raycastPoints.Add(Vector3.Lerp(start, end, 5f / 8f)); 
            raycastPoints.Add(Vector3.Lerp(start, end, 6f / 8f)); 
            raycastPoints.Add(Vector3.Lerp(start, end, 7f / 8f)); 
            raycastPoints.Add(end);
        }

        List<CellManager> highlightedCells = new List<CellManager>();

        foreach (Vector3 point in raycastPoints)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(point, Vector2.zero);
            foreach (var hit in hits)
            {
                CellManager cellManager = hit.collider?.GetComponent<CellManager>();
                if (cellManager != null && !highlightedCells.Contains(cellManager))
                {
                    highlightedCells.Add(cellManager);
                }
            }
        }

        int count = 0;
        foreach (CellManager cellManager in highlightedCells)
        {
            if (count < ship.Health)
            {
                cellManager.HighlightCell();
                count++;
            }
            else
            {
                cellManager.ResetCellColor();
            }
        }

        foreach (CellManager cell in FindObjectsOfType<CellManager>())
        {
            if (!highlightedCells.Contains(cell))
            {
                cell.ResetCellColor();
            }
        }
    }

    void ResetCellsColor()
    {
        foreach (CellManager cell in FindObjectsOfType<CellManager>())
        {
            cell.ResetCellColor();
        }
    }

    void PositionInMiddle()
    {
        if (ship == null)
        {
            return;
        }

        Vector3[] worldCorners = new Vector3[4];
        rectTransform.GetWorldCorners(worldCorners);

        List<Vector3> raycastPoints = new List<Vector3>();

        for (int i = 0; i < 4; i++)
        {
            Vector3 start = worldCorners[i];
            Vector3 end = worldCorners[(i + 1) % 4];
            raycastPoints.Add(start);
            raycastPoints.Add(Vector3.Lerp(start, end, 1f / 8f));
            raycastPoints.Add(Vector3.Lerp(start, end, 2f / 8f));
            raycastPoints.Add(Vector3.Lerp(start, end, 3f / 8f));
            raycastPoints.Add(Vector3.Lerp(start, end, 4f / 8f));
            raycastPoints.Add(Vector3.Lerp(start, end, 5f / 8f));
            raycastPoints.Add(Vector3.Lerp(start, end, 6f / 8f));
            raycastPoints.Add(Vector3.Lerp(start, end, 7f / 8f));
            raycastPoints.Add(end);
        }

        List<CellManager> highlightedCells = new List<CellManager>();

        foreach (Vector3 point in raycastPoints)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(point, Vector2.zero);
            foreach (var hit in hits)
            {
                CellManager cellManager = hit.collider?.GetComponent<CellManager>();
                if (cellManager != null && !highlightedCells.Contains(cellManager))
                {
                    highlightedCells.Add(cellManager);
                }
            }
        }

        if (highlightedCells.Count >= ship.Health)
        {
            float averageX = 0f;
            float averageY = 0f;

            ship.ClearOccupiedCells();

            Cell[] newOccupiedCells = new Cell[ship.Health];

            for (int i = 0; i < ship.Health; i++)
            {
                averageX += highlightedCells[i].transform.position.x;
                averageY += highlightedCells[i].transform.position.y;
                highlightedCells[i].cell.IsEmpty = false;
                newOccupiedCells[i] = highlightedCells[i].cell;
            }

            averageX /= ship.Health;
            averageY /= ship.Health;

            Vector3 newPosition = new Vector3(averageX, averageY, rectTransform.position.z);
            rectTransform.position = newPosition;

            ship.SetOccupiedCells(newOccupiedCells);
        }
        else
        {
            rectTransform.position = initialPosition;
        }
    }
}
