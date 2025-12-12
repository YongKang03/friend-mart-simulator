using System.Collections.Generic;
using UnityEngine;

public class BagSystem : MonoBehaviour
{
    public Dictionary<string, int> scannedCustomerItems = new Dictionary<string, int>();
    Item item;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBagZoneDetect(GameObject detectedItem)
    {
        if (detectedItem.gameObject.CompareTag("Item"))
        {
            item = detectedItem.gameObject.GetComponent<Item>();
            if (item != null)
            {
                string itemName = item.itemName;

                if (scannedCustomerItems.ContainsKey(itemName) && scannedCustomerItems[itemName] > 0)
                {
                    scannedCustomerItems[itemName]--;
                    Debug.Log("Bagged: " + itemName);
                    Destroy(item.gameObject);
                    AudioManager.instance.Play("plasticBag");

                    if (scannedCustomerItems[itemName] == 0)
                    {
                        scannedCustomerItems.Remove(itemName);
                    }
                }
                else
                {
                    Debug.Log("Invalid or extra item bagged: " + itemName);
                }
            }
            else
            {
                Debug.Log("Invalid item to be bagged.");
            }
        }
    }

    public void ClearScannedCustomerItems()
    {
        scannedCustomerItems.Clear();
    }
}
