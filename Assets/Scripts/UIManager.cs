using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI player1Name;
    public TextMeshProUGUI player2Name;

    public GameObject menu;
    public GameObject darkOverlay;

    void Start()
    {
        player1Name.text = GameManager.Instance.player1._playerName;
        player2Name.text = GameManager.Instance.player2._playerName;
    }

    public void MenuButton()
    {
        darkOverlay.SetActive(true);
        menu.SetActive(true);
    }

    public void BackButton()
    {
        darkOverlay.SetActive(false);
        menu.SetActive(false);
    }

    public void MainmenuButton()
    {
        SceneManager.LoadScene(0);
    }
    public void RevengeButton()
    {
        SceneManager.LoadScene(1);
    }

}
