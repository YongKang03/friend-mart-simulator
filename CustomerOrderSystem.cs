using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderSystem : MonoBehaviour
{
    public List<string> itemPool = new List<string>();
    public int minItemCount = 1;
    public int maxItemCount = 5;
    public int maxItemQuantityCount = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Dictionary<string, int> GenerateCustomerOrder()
    {
        var newOrder = new Dictionary<string, int>();

        int itemCount = Random.Range(minItemCount, maxItemCount);
        for (int i = 0; i < itemCount; i++)
        {
            string item = itemPool[Random.Range(0, itemPool.Count)];
            int quantity = Random.Range(1, maxItemQuantityCount);

            if (newOrder.ContainsKey(item))
                newOrder[item] += quantity;
            else
                newOrder.Add(item, quantity);
        }

        return newOrder;
    }
}
