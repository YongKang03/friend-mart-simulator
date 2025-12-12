using UnityEngine;

public class CustomerSpawnPoint : MonoBehaviour
{
    public GameObject[] customerWithOrderPrefabs;
    public GameObject[] wanderCustomerPrefabs;
    public Transform spawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnCustomerWithOrder()
    {
        if (customerWithOrderPrefabs.Length == 0) return;

        int index = Random.Range(0, customerWithOrderPrefabs.Length);

        GameObject customer = Instantiate(customerWithOrderPrefabs[index], spawnPoint.position, Quaternion.identity);
    }

    public void SpawnWanderCustomer()
    {
        if (wanderCustomerPrefabs.Length == 0) return;

        int index = Random.Range(0, wanderCustomerPrefabs.Length);

        GameObject customer = Instantiate(wanderCustomerPrefabs[index], spawnPoint.position, Quaternion.identity);
    }
}
