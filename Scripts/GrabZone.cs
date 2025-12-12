using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GrabZone : MonoBehaviour
{
    public GameObject[] itemPrefabPool;
    private int selectedIndex = 0;
    public Transform spawnPoint;

    private Dictionary<XRNode, bool> wasGripping = new Dictionary<XRNode, bool>()
    {
        { XRNode.LeftHand, false },
        { XRNode.RightHand, false }
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("LeftHand") || other.CompareTag("RightHand"))
        {
            //For unity editor test
            if (Application.isEditor && Input.GetKeyDown(KeyCode.G))
            {
                SpawnItem();
                Debug.Log("Grabbed with XR interactive device.");
            }
        }

        //For vr controller
        if (other.CompareTag("LeftHand") || other.CompareTag("RightHand"))
        {
            // 2) Pick the right XRNode
            XRNode node = other.CompareTag("LeftHand")
                          ? XRNode.LeftHand
                          : XRNode.RightHand;

            InputDevice device = InputDevices.GetDeviceAtXRNode(node);
            if (!device.isValid) return;

            // Grip detection
            if (device.TryGetFeatureValue(CommonUsages.gripButton, out bool isGripping))
            {
                bool wasPreviouslyGripping = wasGripping[node];

                // Only spawn when grip is newly pressed
                if (isGripping && !wasPreviouslyGripping)
                {
                    SpawnItem();
                    Debug.Log($"Spawned item once with grip on {node}");
                }

                // Update grip state for next frame
                wasGripping[node] = isGripping;
            }
        }
    }

    void SpawnItem()
    {
        Instantiate(itemPrefabPool[selectedIndex], spawnPoint.position, spawnPoint.rotation);
    }
}