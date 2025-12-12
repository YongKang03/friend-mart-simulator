using UnityEngine;

public class PickupZone : MonoBehaviour
{
    private bool enablePickup = false;
    private bool isBagPickedUp = false;
    private bool isCardPickedUp = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnablePickup()
    {
        enablePickup = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enablePickup)
        {
            if (other.CompareTag("Bag"))
            {
                AudioManager.instance.Play("buttonclick");
                Destroy(other.gameObject);
                isBagPickedUp = true;
            }

            if (other.CompareTag("Card"))
            {
                AudioManager.instance.Play("buttonclick");
                Destroy(other.gameObject);
                isCardPickedUp = true;
            }
        }
    }

    public bool IsPickupCompleted()
    {
        return isBagPickedUp && isCardPickedUp;
    }

    public void CompletePickup()
    {
        enablePickup = false;
        isBagPickedUp = false;
        isCardPickedUp = false;
}
}
