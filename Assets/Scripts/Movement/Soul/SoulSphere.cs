using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Forwards the OnTriggerEnter to the parent Soul script.
/// </summary>
public class SoulSphere : MonoBehaviour
{
    private Soul _soul;
    
    // Start is called before the first frame update
    void Start()
    {
        _soul = transform.parent.GetComponent<Soul>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        _soul.OnTriggerEnter(other);
    }
}
