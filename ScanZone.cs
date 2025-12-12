using System.Collections.Generic;
using UnityEngine;

public class ScanZone : MonoBehaviour
{
    public Dictionary<string, int> customerItems = new Dictionary<string, int>();
    public Dictionary<string, int> scannedCustomerItemsForUI = new Dictionary<string, int>();
    Item item;
    BagSystem bagSystem;
    UIManager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bagSystem = GameObject.Find("BagSystem").GetComponentInChildren<BagSystem>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if (bagSystem == null)
        {
            Debug.LogError("Stupid");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCustomerItems(Dictionary<string, int> customerItemsFromCheckoutSsytem)
    {
        customerItems = new Dictionary<string, int>(customerItemsFromCheckoutSsytem);
        Debug.Log("Received customer order:");
        foreach (var item in customerItems)
        {
            Debug.Log($"{item.Key} x {item.Value}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Item"))
        //{
            item = other.GetComponent<Item>();
            if (item != null)
            {
                string itemName = item.itemName;

                if (customerItems.ContainsKey(itemName) && customerItems[itemName] > 0)
                {
                    customerItems[itemName]--;

                    if (!bagSystem.scannedCustomerItems.ContainsKey(itemName))
                    {
                        bagSystem.scannedCustomerItems.Add(itemName, 1);
                        scannedCustomerItemsForUI.Add(itemName, 1);
                    }
                    else
                    {
                        bagSystem.scannedCustomerItems[itemName]++;
                        scannedCustomerItemsForUI[itemName]++;
                    }

                    Debug.Log("Scanned and ready to put into bag: " + itemName);

                    uiManager.DisplayOrder(scannedCustomerItemsForUI);

                    AudioManager.instance.Play("itemScan");

                    if (customerItems[itemName] == 0)
                    {
                        customerItems.Remove(itemName);
                    }

                    if (customerItems.Count == 0)
                    {
                        Debug.Log("All items scanned!");
                    }
                }
                else
                {
                    Debug.Log("Invalid or extra item scanned: " + itemName);
                }
            }
            else
            {
                Debug.Log("Invalid item!");
            }
        }
    //}

    public void ClearCustomerItems()
    {
        customerItems.Clear();
        scannedCustomerItemsForUI.Clear();
    }
}
