using System.Collections.Generic;
using UnityEngine;

public class CounterSpawnZone : MonoBehaviour
{
    public Vector3 spawnAreaSize = new Vector3();
    public float spacing = 0.25f;
    public int maxAttempts = 30;
    private List<Vector3> usedPositions = new List<Vector3>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnCustomerItems(List<GameObject> items)
    {
        usedPositions.Clear();

        foreach (GameObject item in items)
        {
            Vector3 spawnPos;
            int attempts = 0;

            do
            {
                float x = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
                float z = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);
                spawnPos = transform.position + new Vector3(x, 0, z);

                attempts++;
            } while (!IsFarEnough(spawnPos) && attempts < maxAttempts);

            usedPositions.Add(spawnPos);

            item.transform.position = spawnPos;
            item.SetActive(true);
        }
    }

    private bool IsFarEnough(Vector3 pos)
    {
        foreach (var used in usedPositions)
        {
            if (Vector3.Distance(pos, used) < spacing)
                return false;
        }
        return true;
    }
}
