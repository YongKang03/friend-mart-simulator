using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckoutSystem : MonoBehaviour
{
    private enum CheckoutState
    {
        Idle,
        ScanningAndBagging,
        Payment,
        Pickup,
        Completed
    }
    private CheckoutState currentCheckoutState = CheckoutState.Idle;

    CustomerAI currentCustomer;
    public CounterSpawnZone counterSpawnZone;   
    public ScanZone scanZone;
    public BagSpawner bagSpawner;
    public BagSystem bagSystem;
    public CardSpawner cardSpawner;
    public PaymentZone paymentZone;
    public PickupZone pickupZone;
    public ScoreSystem scoreSystem;
    public UIManager uiManager;

    public Dictionary<string, int> customerOrder = new Dictionary<string, int>();
    private bool isCustomerOrderReceived = false;
    private float customerWaitingStartTime = 0.0f;
    private float customerWaitingTime = 0.0f;

    private int servedCustomer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        servedCustomer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentCheckoutState)
        {
            case CheckoutState.Idle:
                if (isCustomerOrderReceived && currentCustomer != null)
                {
                    scanZone.SetCustomerItems(customerOrder);
                    bagSpawner.SpawnBag();
                    counterSpawnZone.SpawnCustomerItems(currentCustomer.pickedItemGameObjects);
                    currentCheckoutState = CheckoutState.ScanningAndBagging;
                    Debug.Log("Customer order received.");
                    customerWaitingStartTime = Time.time;
                }
                break;

            case CheckoutState.ScanningAndBagging:
                if (scanZone.customerItems.Count == 0 && bagSystem.scannedCustomerItems.Count == 0)
                {
                    cardSpawner.SpawnCard();
                    paymentZone.EnablePayment();
                    currentCheckoutState = CheckoutState.Payment;
                    uiManager.DisplayPayment();
                    Debug.Log("Scanning and bagging done. Proceed to payment.");
                }
                break;

            case CheckoutState.Payment:
                if (paymentZone.IsPaymentCompleted())
                {
                    pickupZone.EnablePickup();
                    currentCheckoutState = CheckoutState.Pickup;
                    Debug.Log("Payment completed. Pickup enabled.");
                }
                break;

            case CheckoutState.Pickup:
                if (pickupZone.IsPickupCompleted())
                {
                    currentCheckoutState = CheckoutState.Completed;
                    Debug.Log("Checkout compeleted.");
                }
                break;

            case CheckoutState.Completed:
                customerWaitingTime = Time.time - customerWaitingStartTime;
                servedCustomer++;
                scoreSystem.AddCustomerServingScore(customerWaitingTime, customerOrder.Values.Sum());
                isCustomerOrderReceived = false;
                customerOrder.Clear();
                scanZone.ClearCustomerItems();
                bagSystem.ClearScannedCustomerItems();
                paymentZone.CompletePayment();
                pickupZone.CompletePickup();
                currentCustomer.CompleteCheckout();
                uiManager.ClearOrder();
                uiManager.ClearPaymentPrompt();

                currentCheckoutState = CheckoutState.Idle;
                break;
        }
    }

    public void ReceiveCustomerOrder(CustomerAI customer, Dictionary<string, int> receivedCustomerOrder)
    {
        currentCustomer = customer;
        customerOrder = new Dictionary<string, int>(receivedCustomerOrder);
        Debug.Log("Received customer order:");
        foreach (var item in customerOrder)
        {
            Debug.Log($"{item.Key} x {item.Value}");
        }
        isCustomerOrderReceived = true;
    }

    public void ResetCheckout()
    {
        scanZone.SetCustomerItems(customerOrder);
        bagSystem.ClearScannedCustomerItems();
        currentCheckoutState = CheckoutState.ScanningAndBagging;
    }

    public int GetServedCustomer()
    {
        return servedCustomer;
    }
}
