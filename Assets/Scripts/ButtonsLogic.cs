using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonsLogic : MonoBehaviour
{
    public TMP_InputField infoText;
    private bool isPlayer1Turn = true;

    public TextMeshProUGUI errorText;
    //private bool isError = false;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        UpdateInfoText();
    }

    private void UpdateInfoText()
    {
        if (infoText != null)
        {
            if (isPlayer1Turn)
            {
                infoText.text = "Player 1";
            }
            else
            {
                infoText.text = "Player 2";
            }
        }
        else
        {
            Debug.LogWarning("infoText is not assigned!");
        }
    }

    public void OnNextButtonPressed()
    {
        GridManager gridManager = FindObjectOfType<GridManager>();

        if (isPlayer1Turn)
        {
            List<Cell> player1OccupiedCells = GetPlayer1OccupiedCells(gridManager);
            if (player1OccupiedCells.Count < 20)
            {
                errorText.gameObject.SetActive(true);
                return;
            }
            GameManager.Instance.SetPlayerOccupiedCells(1, new List<Cell>(player1OccupiedCells));

            foreach (Cell cell in player1OccupiedCells)
            {
                Debug.Log($"Player 1 occupied cell: {cell.PosX}, {cell.PosY}");
            }

            ResetShips(gridManager);
            isPlayer1Turn = false;
            UpdateInfoText();
            errorText.gameObject.SetActive(false);
        }
        else
        {
            List<Cell> player2OccupiedCells = GetPlayer2OccupiedCells(gridManager);
            if (player2OccupiedCells.Count < 20)
            {
                errorText.gameObject.SetActive(true);
                return;
            }
            GameManager.Instance.SetPlayerOccupiedCells(2, new List<Cell>(player2OccupiedCells));

            foreach (Cell cell in player2OccupiedCells)
            {
                Debug.Log($"Player 2 occupied cell: {cell.PosX}, {cell.PosY}");
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }


    private List<Cell> GetPlayer1OccupiedCells(GridManager gridManager)
    {
        List<Cell> occupiedCells = new List<Cell>();
        foreach (Cell cell in gridManager.grid.OccupiedCells)
        {
            if (!cell.IsEmpty)
            {
                occupiedCells.Add(cell);
            }
        }
        return occupiedCells;
    }

    private List<Cell> GetPlayer2OccupiedCells(GridManager gridManager)
    {
        List<Cell> occupiedCells = new List<Cell>();
        foreach (Cell cell in gridManager.grid.OccupiedCells)
        {
            if (!cell.IsEmpty)
            {
                occupiedCells.Add(cell);
            }
        }
        return occupiedCells;
    }

    private void ResetShips(GridManager gridManager)
    {
        foreach (var draggable in FindObjectsOfType<Draggable>())
        {
            draggable.ship.ClearOccupiedCells();
            gridManager.DeleteOldCells();

            draggable.transform.position = draggable.initialPosition;
            if(draggable.isRotated == true)
            {
                draggable.rectTransform.Rotate(0, 0, -90);
            }
            draggable.isRotated = false;
        }
    }
}
