using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class CheckoutScreenManager : MonoBehaviour
{
    public GameObject screenUI;
    public TextMeshProUGUI itemListText;
    public TextMeshProUGUI totalText;
    public TextMeshProUGUI messageText;

    private List<string> scannedItems = new List<string>();
    private List<float> scannedPrices = new List<float>();

    private float paymentTimeout = 10f;
    private float paymentTimer = 0f;
    private bool waitingForPayment = false;

    public Color successColor = Color.green;
    public Color failureColor = Color.red;

    void Update()
    {
        if (waitingForPayment)
        {
            paymentTimer += Time.deltaTime;
            if (paymentTimer >= paymentTimeout)
            {
                ShowPaymentResult(false);
                waitingForPayment = false;
            }
        }
    }

    //when scan item call this function
    public void ScanItem(string itemName, float price)
    {
        if (!screenUI.activeSelf)
            screenUI.SetActive(true);

        scannedItems.Add(itemName);
        scannedPrices.Add(price);
        UpdateCheckoutDisplay();
    }

    private void UpdateCheckoutDisplay()
    {
        itemListText.text = "";
        float total = 0f;

        for (int i = 0; i < scannedItems.Count; i++)
        {
            itemListText.text += $"{scannedItems[i]} ............. ${scannedPrices[i]:0.00}\n";
            total += scannedPrices[i];
        }

        totalText.text = $"Total: ${total:0.00}";
        messageText.text = "Please Tap Card / Cash";
    }

    public void CancelCheckout()
    {
        waitingForPayment = false;
        paymentTimer = 0f;
        scannedItems.Clear();
        scannedPrices.Clear();
        Hide();
    }

    public void ConfirmCheckout()
    {
        waitingForPayment = true;
        paymentTimer = 0f;
        messageText.text = "Waiting for Payment...";
    }

    public void OnCardTapped()
    {
        if (waitingForPayment)
        {
            ShowPaymentResult(true);
            waitingForPayment = false;
        }
    }

    public void ShowPaymentResult(bool success)
    {
        messageText.text = success ? "? Payment Successful!" : "? Payment Failed!";
        messageText.color = success ? successColor : failureColor;

        waitingForPayment = false;
        paymentTimer = 0f;
        StartCoroutine(AutoClearScreenAfter(3f));
    }

    IEnumerator AutoClearScreenAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        Hide();
        scannedItems.Clear();
        scannedPrices.Clear();
    }

    public void Hide()
    {
        screenUI.SetActive(false);
    }
}
