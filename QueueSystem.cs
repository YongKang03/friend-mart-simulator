using System.Collections.Generic;
using UnityEngine;

public class QueueSystem : MonoBehaviour
{
    public Transform baseQueuePosition;
    public Vector3 queueOffset = new Vector3(1.0f, 0, 0);
    private List<CustomerAI> customerQueue = new List<CustomerAI>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterCustomer(CustomerAI customer)
    {
        customerQueue.Add(customer);
        UpdateQueuePositions();
    }

    public void UnregisterCustomer(CustomerAI customer)
    {
        if (customerQueue.Contains(customer))
        {
            customerQueue.Remove(customer);
            UpdateQueuePositions();
        }
    }

    private void UpdateQueuePositions()
    {
        for (int i = 0; i < customerQueue.Count; i++)
        {
            Vector3 position = baseQueuePosition.position + (queueOffset * i);
            customerQueue[i].MoveToQueueSpot(position);
        }
    }

    public bool IsCustomerAtCounter(CustomerAI customer)
    {
        return customerQueue.Count > 0 && customerQueue[0] == customer;
    }
}
