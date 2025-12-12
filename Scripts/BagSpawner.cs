using UnityEngine;

public class BagSpawner : MonoBehaviour
{
    public GameObject bagPrefab;
    public Transform spawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBag()
    {
        GameObject card = Instantiate(bagPrefab, spawnPoint.position, Quaternion.identity);
    }
}
