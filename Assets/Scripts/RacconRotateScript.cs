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

        transform.parent.position += new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * 5f, 0, Input.GetAxis("Vertical") * Time.deltaTime * 5f);
    }
}
