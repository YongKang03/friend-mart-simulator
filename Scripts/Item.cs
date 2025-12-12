using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    private bool isReserved = false;
    private GameObject reservedBy = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetReserveStatus()
    {
        return isReserved;
    }

    public void ReverseItem(GameObject customer)
    {
        isReserved = true;
        reservedBy = customer;
    }

    public void UnreserveItem()
    {
        isReserved = false;
        reservedBy = null;
    }
}
