using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var thisCollider = GetComponent<Collider>();

        if (!thisCollider)
            return;

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Buyer has collided with " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hello, I am a buyer. I want to buy something.");
        }
    }
}
