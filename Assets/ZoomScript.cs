using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomScript : MonoBehaviour
{
    public float Speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var scaleValue = Mathf.Sin(Time.time * Speed);

        var remappedScale = Remap(scaleValue, -1, 1, 1, 1.2f);

        transform.localScale = new Vector3(remappedScale, remappedScale, 0);
    }

    private float Remap(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
