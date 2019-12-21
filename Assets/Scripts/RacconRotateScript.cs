using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacconRotateScript : MonoBehaviour
{
    public float RotationSpeed = 200f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.one * Time.deltaTime * RotationSpeed);
    }
}
