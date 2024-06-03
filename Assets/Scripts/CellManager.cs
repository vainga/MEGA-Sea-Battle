using UnityEngine;
using UnityEngine.EventSystems;

public class CellManager : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private SpriteRenderer spriteRenderer;
    private Color normalColor;
    private Color highlightedColor = new Color(0f, 1f, 0f, 0.6f);
    private Color highlightedRedColor = new Color(1f, 0f, 0f, 0.6f);

    public Cell cell;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalColor = spriteRenderer.color;
    }

    public void OnDrop(PointerEventData eventData) { }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //HighlightCell();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //ResetCellColor();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        LogCellCoordinates();
    }

    private void LogCellCoordinates()
    {
        Debug.Log($"Cell coordinates: {cell.PosX}, {cell.PosY}");
    }

    public void HighlightCell()
    {
        spriteRenderer.color = highlightedColor;
    }

    public void HighlightCellRed()
    {
        spriteRenderer.color = highlightedRedColor;
    }

    public void ResetCellColor()
    {
        spriteRenderer.color = normalColor;
    }
}
