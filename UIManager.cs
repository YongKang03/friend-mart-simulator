using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public GameObject beforeShiftUI;
    public GameObject gameplayUI;
    public GameObject pauseMenuUI;
    public GameObject afterShiftUI;
    public GameObject settingUI;
    public GameObject ongoingShiftUI;
    public GameObject endShiftButton;
    public GameObject confirmRestartUI;
    public GameObject confirmExitUI;
    public GameObject scoreRatingUI;
    public GameObject tutorialUI;

    public Transform xrHead; 
    public InputActionReference pauseAction;

    private bool isPaused = false;
    private bool isShiftRunning = false;

    public GameObject CheckoutUI;
    public TextMeshProUGUI checkoutOrderText;
    public TextMeshProUGUI paymentText;

    public TextMeshProUGUI shiftStatusText;

    //aftershift
    public CanvasGroup afterShiftPanel;
    public TextMeshProUGUI customerServedText;
    public TextMeshProUGUI unsatisfiedText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI scoreRatingText;
    string scoreRating = "";

    public GameplaySystem gameplaySystem;
    public ScoreSystem scoreSystem;
    public CheckoutSystem checkoutSystem;

    void Start()
    {
        ShowBeforeShift();
        pauseMenuUI.SetActive(false);
        confirmExitUI.SetActive(false);
    }

    void Update()
    {
        if (isShiftRunning && Time.time - gameplaySystem.GetShiftStartTime() >= gameplaySystem.GetShiftDuration())
        {
            if (gameplaySystem.ReturnCustomer())
            {
                ShowEndingShiftText();
                ShowEndShiftButton();
            }
            else
            {
                ShowCustomerShiftText();
            }
        }

        if (pauseAction != null && pauseAction.action.WasPressedThisFrame())
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    //beforeshift
    public void ShowBeforeShift()
    {
        beforeShiftUI.SetActive(true);
        gameplayUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        afterShiftUI.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void StartShift()
    {
        if (isShiftRunning) return;

        beforeShiftUI.SetActive(false);
        gameplayUI.SetActive(true);
        CheckoutUI.SetActive(true);
        ongoingShiftUI.SetActive(true);
        isShiftRunning = true;
    }

    public void ShowOngoingShiftText()
    {
        shiftStatusText.text = "The shift is ongoing!";
    }

    public void ShowCustomerShiftText()
    {
        shiftStatusText.text = "There are remaining customers in mart!";
    }

    public void ShowEndingShiftText()
    {
        shiftStatusText.text = "The shift has ended!";
    }

    public void ShowEndShiftButton()
    {
        endShiftButton.SetActive(true);
    }

    //aftershift
    public void EndShift()
    {
        isShiftRunning = false;
        gameplayUI.SetActive(false);
        ongoingShiftUI.SetActive(false);
        CheckoutUI.SetActive(false);
        afterShiftUI.SetActive(true);
        ShowResults(checkoutSystem.GetServedCustomer(), gameplaySystem.GetTotalCustomer() - checkoutSystem.GetServedCustomer(), scoreSystem.GetTotalScore());
    }

    //when shift complete,call this funtion
    public void ShowResults(int served, int unsatisfied, int score)
    {
        afterShiftUI.SetActive(true); 
        customerServedText.text = $"Customer(s) Served : {served}";
        unsatisfiedText.text = $"Unsatisfied Customer(s) : {unsatisfied}";
        finalScoreText.text = $"Final Score : {score}";
        scoreRatingText.text = $"Score Rating : {GetScoreRating(served, unsatisfied, score)}";
        afterShiftPanel.alpha = 1;
        afterShiftPanel.interactable = true;
        afterShiftPanel.blocksRaycasts = true;
    }

    public void ShowScoreRating()
    {
        scoreRatingUI.SetActive(true);
    }

    public void HideScoreRating()
    {
        scoreRatingUI.SetActive(false);
    }

    string GetScoreRating(int served, int unsatisfied, int score)
    {
        int totalCustomer = served + unsatisfied;
        int baseCustomerScore = 150;
        if (score >= totalCustomer * (baseCustomerScore + 50))
        {
            scoreRating = "Perfect";
        }
        if (score >= totalCustomer * (baseCustomerScore + 25))
        {
            scoreRating = "Good";
        }
        else if (score >= totalCustomer * baseCustomerScore)
        {
            scoreRating = "Average";
        }
        else if (score >= totalCustomer * (baseCustomerScore - 50))
        {
            scoreRating = "Bad";
        }
        else
        {
            scoreRating = "Don't come to work tomorrow";
        }
        return scoreRating;
    }

    //pausemenu
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        if (xrHead != null)
        {
            Vector3 forward = xrHead.forward;
            forward.y = 0;
            forward.Normalize();

            pauseMenuUI.transform.position = xrHead.position + forward * 0.75f + Vector3.up * -0.1f;
            pauseMenuUI.transform.rotation = Quaternion.LookRotation(forward);

        }
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
    }

    public void OpenSettingMenu()
    {
        settingUI.SetActive(true);
    }

    public void CloseSettingMenu()
    {
        settingUI.SetActive(false);

    }
    public void ConfirmRestartPage()
    {
        confirmRestartUI.SetActive(true);
    }

    public void NoConfirmRestart()
    {
        confirmRestartUI.SetActive(false);
    }

    public void confirmExitPage()
    {
        confirmExitUI.SetActive(true);
    }

    public void NoConfirmExit()
    {
        confirmExitUI.SetActive(false);
    }

    public void DisplayOrder(Dictionary<string, int> order)
    {
        string displayText = "";
        foreach (var pair in order)
        {
            displayText += $"{pair.Key} x {pair.Value}\n";
        }
        checkoutOrderText.text = displayText;
    }

    public void ClearOrder()
    {
        checkoutOrderText.text = "";
    }

    public void DisplayPayment()
    {
        paymentText.text = "Pending payment.";
    }

    public void DisplayPaymentPrompt(bool isPaymentSuccess)
    {
        if (isPaymentSuccess)
        {
            paymentText.text = "Payment successful.";
        }
        else
        {
            paymentText.text = "Invalid payment.";
        }
    }

    public void ClearPaymentPrompt()
    {
        paymentText.text = "";
    }

    public void ToggleTutorialUI()
    {
        tutorialUI.SetActive(!tutorialUI.activeSelf);
    }
}
