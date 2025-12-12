using UnityEngine;

public class PaymentZone : MonoBehaviour
{
    Item item;
    UIManager uiManager;
    private bool enablePayment = false;
    private bool isPaymentCompleted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnablePayment()
    {
        enablePayment = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enablePayment)
        {
            if (other.CompareTag("Card"))
            {
                AudioManager.instance.Play("cardSound");
                isPaymentCompleted = true;
                uiManager.DisplayPaymentPrompt(true);
                Debug.Log("Payment successful.");
            }
            else
            {
                Debug.Log("Not a card!");
                uiManager.DisplayPaymentPrompt(false);
            }
        }
    }
    
    public bool IsPaymentCompleted()
    {
        return isPaymentCompleted;
    }

    public void CompletePayment()
    {
        enablePayment = false;
        isPaymentCompleted = false;
    }
}
