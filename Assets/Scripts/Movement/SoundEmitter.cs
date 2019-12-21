using System.Collections;
using System.Collections.Generic;
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
        
    }

    void makeSound()
    {
        Collider[] targetsInSoundRadius = Physics.OverlapSphere(transform.position, soundRadius, guardMask);

        foreach (var target in targetsInSoundRadius)
        {
            var guard = target.GetComponent<Guard>();
        }
    }
}
