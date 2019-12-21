using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/**
 * Script for GameObjects with a transform
 * call makeSound to emit a sound and alert all guards in the specified radius
 */
public class SoundEmitter : MonoBehaviour
{
    public LayerMask guardMask;

    public float soundRadius;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            makeSound();
        }
        
    }
    
    private void OnDrawGizmosSelected()
    {
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, soundRadius);
    }

    void makeSound()
    {
        Collider[] targetsInSoundRadius = Physics.OverlapSphere(transform.position, soundRadius, guardMask);

        foreach (var target in targetsInSoundRadius)
        {
            Guard guard = target.GetComponent<Guard>();
            if (guard != null)
            {
                guard.alertGuard(transform.position);
            }
        }
    }
}
