using UnityEngine;
using UnityEngine.EventSystems;

public class CellManager : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private SpriteRenderer spriteRenderer;
    private Color normalColor;
    private Color highlightedColor = new Color(0f, 1f, 0f, 0.6f);
    private Color highlightedRedColor = new Color(1f, 0f, 0f, 0.6f);

    public Cell cell;
    private BattleManager battleManager;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalColor = spriteRenderer.color;
    }

    private void Start()
    {
        // Find the BattleManager instance in the scene
        battleManager = FindObjectOfType<BattleManager>();
        if (battleManager == null)
        {
            Debug.LogError("BattleManager not found in the scene.");
        }
    }

    public void OnDrop(PointerEventData eventData) { }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // HighlightCell();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ResetCellColor();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (battleManager != null)
        {
            battleManager.LogCellStatus(this);
        }
        else
        {
            Debug.LogError("BattleManager is not assigned.");
        }
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

    public void ActivateTochka()
    {
        GameObject tochkaObject = transform.Find("TOCHKA").gameObject;
        tochkaObject.SetActive(true);
    }
}
