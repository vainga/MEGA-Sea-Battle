using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CellManager : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private SpriteRenderer spriteRenderer;
    private Color normalColor;
    private Color highlightedColor = new Color(0f, 1f, 0f, 0.6f);
    private Color highlightedRedColor = new Color(1f, 0f, 0f, 0.6f);
    private Color disabledColor = new Color(1f, 1f, 1f, 0.3f); // Полупрозрачный цвет для неактивных ячеек

    public Cell cell;
    private BattleManager battleManager;
    private bool isInteractive = true; // Флаг для отслеживания интерактивности ячейки
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
            // Проверяем, активен ли текущий игрок для клика
            if ((battleManager.isPlayer1Turn && transform.parent == battleManager.gridManager2.transform) ||
                (!battleManager.isPlayer1Turn && transform.parent == battleManager.gridManager1.transform))
            {
                Debug.Log("Этот игрок не может кликать по этой сетке в данный момент.");
                return;
            }

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
        spriteRenderer.color = isInteractive ? normalColor : disabledColor; // Устанавливаем цвет в зависимости от интерактивности
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
