using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleControllerScript : MonoBehaviour
{
    private float moveSpeed = 6;
    private Rigidbody rigidbody;
    private Camera viewCamera;
    private Vector3 velocity;
    // private GamepadInput inputHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        // inputHandler = GetComponent<GamepadInput>();
        viewCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            viewCamera.transform.position.y));
        transform.LookAt(mousePos + Vector3.up * transform.position.y);
        // velocity = new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
        // velocity = new Vector3(inputHandler.GetLeftHorizontalValue(),0, inputHandler.GetLeftVerticalValue()).normalized * moveSpeed;
    }

    private void FixedUpdate()
    {
        // rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
    }
}
