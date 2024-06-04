using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CellManager : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private SpriteRenderer spriteRenderer;
    private Color normalColor;
    private Color highlightedColor = new Color(0f, 1f, 0f, 0.6f);
    private Color highlightedRedColor = new Color(1f, 0f, 0f, 0.6f);
    private Color disabledColor = new Color(1f, 1f, 1f, 0.3f); // ѕолупрозрачный цвет дл€ неактивных €чеек

    public Cell cell;
    private BattleManager battleManager;
    private bool isInteractive = true; // ‘лаг дл€ отслеживани€ интерактивности €чейки
    public bool isClicked = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalColor = spriteRenderer.color;
    }

    private void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
        if (battleManager == null)
        {
            Debug.LogError("BattleManager not found in the scene.");
        }
    }

    public void OnDrop(PointerEventData eventData) { }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isInteractive && battleManager != null && !isClicked)
        {
            HighlightCell();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isInteractive)
        {
            ResetCellColor();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isInteractive && battleManager != null && !isClicked)
        {
            isClicked = true;
            battleManager.LogCellStatus(this);
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
        spriteRenderer.color = isInteractive ? normalColor : disabledColor; // ”станавливаем цвет в зависимости от интерактивности
    }

    public void ActivateTochka()
    {
        GameObject tochkaObject = transform.Find("TOCHKA").gameObject;
        tochkaObject.SetActive(true);
    }

    public void SetInteractivity(bool interactive)
    {
        isInteractive = interactive;
        ResetCellColor();
    }
}
