using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ShiftStatusScreen : MonoBehaviour
{
    public TextMeshProUGUI shiftStatusText;
    public GameObject endShiftButton;

    public void ShowOngoingShift()
    {
        shiftStatusText.text = "The shift is ongoing!";
        shiftStatusText.color = Color.green;
        endShiftButton.SetActive(false);
    }

    public void ShowEndedShift()
    {
        shiftStatusText.text = "The shift has ended!";
        shiftStatusText.color = Color.red;
        endShiftButton.SetActive(true);
    }

    public void OnEndShiftButtonPressed()
    {
        Debug.Log("End Shift button clicked!");
    }
}
