using UnityEngine;
using UnityEngine.UI;

public class HideButtonOnTransition : MonoBehaviour
{
    public Button buttonToHide;
    public Button buttonToShow;

    private void Start()
    {
        // ��������� ��������� ��������
        if (GameManager.Instance.transitionThroughRpc)
        {
            buttonToHide.gameObject.SetActive(false);
            buttonToShow.gameObject.SetActive(true);
        }
    }
}
