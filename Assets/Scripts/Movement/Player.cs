using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private GamepadInput _inputHandler;
    
    private float moveSpeed = 6;
    private Vector3 _velocity;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<GamepadInput>();
    }

    // Update is called once per frame
    void Update()
    {
        _velocity = new Vector3(_inputHandler.GetLeftHorizontalValue(),0, _inputHandler.GetLeftVerticalValue()).normalized * moveSpeed;
    }
    
    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.fixedDeltaTime);
    }
}
