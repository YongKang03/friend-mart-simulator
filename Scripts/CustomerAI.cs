using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    private enum CustomerState
    {
        Idle,
        Wander,
        GrabItem,
        Queue,
        Checkout,
        Leave
    }
    private CustomerState currentCustomerState = CustomerState.GrabItem;
    private Animator animator;

    NavMeshAgent agent;
    CustomerOrderSystem customerOrderSystem;
    Item currentTargetItem;
    QueueSystem queueSystem;
    CheckoutSystem checkoutSystem;
    CustomerSpawnPoint customerSpawnPoint;

    public Dictionary<string, int> customerOrder = new Dictionary<string, int>();
    private Dictionary<string, int> pickedItems = new Dictionary<string, int>();
    public List<GameObject> pickedItemGameObjects = new List<GameObject>();

    private float grabDistance = 2.0f;
    private float timerToGrab = 0.0f;
    private float grabTimer = 30.0f;

    private Vector3 targetQueueSpot;
    private bool isCustomerInQueue = false;
    private bool isOrderSent = false;
    private bool isCheckoutCompleted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        customerOrderSystem = GameObject.Find("CustomerOrderSystem").GetComponent<CustomerOrderSystem>();
        queueSystem = GameObject.Find("QueueSystem").GetComponent<QueueSystem>();
        checkoutSystem = GameObject.Find("CheckoutSystem").GetComponent<CheckoutSystem>();
        customerSpawnPoint = GameObject.Find("CustomerSpawnPoint").GetComponent<CustomerSpawnPoint>();
        animator = GetComponent<Animator>();

        if (customerOrderSystem != null)
        {
            customerOrder = customerOrderSystem.GenerateCustomerOrder();
            Debug.Log("Customer's order:");
            foreach (var item in customerOrder)
            {
                Debug.Log($"{item.Key} x {item.Value}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentCustomerState)
        {
            case CustomerState.Idle:
                break;

            case CustomerState.GrabItem:
                if (currentTargetItem == null)
                {
                    currentTargetItem = FindNextItem();

                    if (currentTargetItem != null)
                    {
                        agent.SetDestination(currentTargetItem.transform.position);
                    }
                    else if (IsAllItemsGrabbed())
                    {
                        currentCustomerState = CustomerState.Queue;
                        Debug.Log("Items grabbed. Proceed to counter.");
                    }
                }
                else
                {
                    if (!currentTargetItem.gameObject.activeInHierarchy || !currentTargetItem.enabled || !currentTargetItem ||
                        currentTargetItem.GetComponent<Item>().GetReserveStatus())
                    {
                        currentTargetItem = null;
                        return;
                    }

                    agent.SetDestination(currentTargetItem.transform.position);

                    if (Vector3.Distance(transform.position, currentTargetItem.transform.position) < grabDistance)
                    {
                        GrabItem(currentTargetItem);
                        StartCoroutine(WaitAfterGrabbing(3f));
                        currentCustomerState = CustomerState.Idle;
                        currentTargetItem = null;
                    }

                    if (timerToGrab >= grabTimer + Time.time)
                    {
                        currentTargetItem = null;
                    }
                }
                break;

            case CustomerState.Queue:
                if (!isCustomerInQueue)
                {
                    queueSystem.RegisterCustomer(this);
                    isCustomerInQueue = true;
                }

                if (queueSystem.IsCustomerAtCounter(this))
                {
                    if (Vector3.Distance(transform.position, targetQueueSpot) < 1f && !isOrderSent)
                    {
                        SendCustomerOrderToCheckout();
                        isOrderSent = true;
                        currentCustomerState = CustomerState.Checkout;
                    }
                }
                break;

            case CustomerState.Checkout:
                if (isCheckoutCompleted)
                {
                    currentCustomerState = CustomerState.Leave;
                }
                break;

            case CustomerState.Leave:
                queueSystem.UnregisterCustomer(this);
                agent.SetDestination(customerSpawnPoint.spawnPoint.position);
                if (Vector3.Distance(transform.position, customerSpawnPoint.spawnPoint.position) < 1f)
                {
                    Destroy(this.gameObject);
                }
                break;
        }
        UpdateAnimation();
    }

    Item FindNextItem()
    {
        foreach (var itemName in customerOrder.Keys)
        {
            int needed = customerOrder[itemName];
            int picked = pickedItems.ContainsKey(itemName) ? pickedItems[itemName] : 0;

            if (picked < needed)
            {
                Item[] allItems = Object.FindObjectsByType<Item>(FindObjectsSortMode.None);
                foreach (Item item in allItems)
                {
                    if (item != null && item.isActiveAndEnabled && item.itemName == itemName && !item.GetReserveStatus())
                    {
                        timerToGrab = Time.time;
                        Debug.Log("Trying to grab: " + itemName);
                        return item;
                    }
                }
            }
        }
        return null;
    }

    void GrabItem(Item item)
    {
        string name = item.itemName;

        if (!pickedItems.ContainsKey(name))
            pickedItems[name] = 0;

        pickedItems[name]++;
        pickedItemGameObjects.Add(item.gameObject);
        item.ReverseItem(this.gameObject);
        item.gameObject.SetActive(false);
    }

    IEnumerator WaitAfterGrabbing(float delay)
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(delay);
        agent.isStopped = false;
        currentCustomerState = CustomerState.GrabItem;
    }

    bool IsAllItemsGrabbed()
    {
        foreach (var entry in customerOrder)
        {
            if (!pickedItems.ContainsKey(entry.Key) || pickedItems[entry.Key] < entry.Value)
                return false;
        }
        return true;
    }

    public void MoveToQueueSpot(Vector3 spot)
    {
        targetQueueSpot = spot;
        agent.SetDestination(targetQueueSpot);
        agent.isStopped = false;
    }

    void SendCustomerOrderToCheckout()
    {
        if (checkoutSystem != null)
        {
            checkoutSystem.ReceiveCustomerOrder(this, customerOrder);
        }
    }
    
    public void CompleteCheckout()
    {
        isCheckoutCompleted = true;
    }

    void UpdateAnimation()
    {
        if (agent.velocity.magnitude > 0.25f && !agent.isStopped)
        {
            animator.Play("walk");
        }
        else
        {
            animator.Play("idle1");
        }
    }
}
