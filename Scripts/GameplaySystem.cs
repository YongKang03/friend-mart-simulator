using UnityEngine;

public class GameplaySystem : MonoBehaviour
{
    private float shiftStartTime = 0.0f;
    public float shiftDuration = 360.0f;
    private bool isShiftActive = false;

    public CustomerSpawnPoint customerSpawnPoint;
    public int totalCustomer = 5;
    private int spawnedCustomer = 0;
    private float nextCustomerSpawnTime = 0.0f;
    public float customerSpawnMinInterval = 10.0f;
    public float customerSpawnMaxInterval = 30.0f;
    GameObject[] customers;

    int type = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isShiftActive && Time.time - shiftStartTime >= shiftDuration)
        {
            EndShift();
        }

        if (isShiftActive && Time.time >= nextCustomerSpawnTime)
        {
            if (spawnedCustomer < totalCustomer)
            {
                type = Random.Range(0, 2);
            }
            else
            {
                type = 1;
            }

            if (type == 0)
            {
                customerSpawnPoint.SpawnCustomerWithOrder();
                spawnedCustomer++;
            }
            else if (type == 1)
            {
                customerSpawnPoint.SpawnWanderCustomer();
            }

            ScheduleNextCustomerSpawn();
        }
    }

    public void StartShift()
    {
        shiftStartTime = Time.time;
        isShiftActive = true;
        AudioManager.instance.Play("ding");
        ScheduleNextCustomerSpawn();
        Debug.Log("Shift started!");
    }

    public void EndShift()
    {
        isShiftActive = false;
        AudioManager.instance.Play("ding");
        Debug.Log("Shift ended.");
    }

    void ScheduleNextCustomerSpawn()
    {
        nextCustomerSpawnTime = Time.time + (Random.Range(customerSpawnMinInterval, customerSpawnMaxInterval));
        Debug.Log("Next customer comes in: " + (nextCustomerSpawnTime - Time.time));
    }

    public float GetShiftStartTime()
    {
        return shiftStartTime;
    }

    public float GetShiftDuration()
    {
        return shiftDuration;
    }

    public int GetTotalCustomer()
    {
        return totalCustomer;
    }

    public bool ReturnCustomer()
    {
        customers = GameObject.FindGameObjectsWithTag("NPC");
        return customers.Length == 0;
    }

    public bool IsShiftActive()
    {
        return isShiftActive;
    }
}
