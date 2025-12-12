using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BagZone : MonoBehaviour
{
    BagSystem bagSystem;
    //public AudioSource baggingSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bagSystem = GameObject.Find("BagSystem").GetComponentInChildren<BagSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        bagSystem.OnBagZoneDetect(other.gameObject);
    }

    public void PlayBaggingSound()
    {
        //baggingSound.Play();
    }
}
