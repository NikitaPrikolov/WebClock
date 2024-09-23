using UnityEngine;

public class UIController : MonoBehaviour
{
    public TimeInputHandler timeInputHandler;
    public GameObject mainPanel;
    public GameObject changeTimePanel;

    public void OnSaveButtonClicked()
    {
        mainPanel.SetActive(true);
        timeInputHandler.SaveTime();
        changeTimePanel.SetActive(false);
    }
    public void OnChangeButtonClicked()
    {
        changeTimePanel.SetActive(true);
        mainPanel.SetActive(false);
    }
}
