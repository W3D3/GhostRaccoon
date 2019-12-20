using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("Update");
            _rigidbody.AddForce(1, 0, 0);
        }
        else  if (Input.GetKey(KeyCode.S))
            _rigidbody.AddForce(-1, 0, 0);
        else  if (Input.GetKey(KeyCode.A))
            _rigidbody.AddForce(0, 0, 1);
        else if (Input.GetKey(KeyCode.D))
            _rigidbody.AddForce(0, 0, -1);
     
    }
}
