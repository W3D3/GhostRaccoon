using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    /// <summary>
    /// The door which gets triggered.
    /// </summary>
    public Door Door;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Raccoon")) // not triggered by guard
            Door.Open();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Raccoon")) // not triggered by guard
            Door.Close();
    }
}