﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private GamepadInput _inputHandler;
    
    private float moveSpeed = 6;
    private Vector3 _velocity;

    public UnityEvent HandsOverMovement; 

    /// <summary>
    /// Boolean if the movement is active for this raccoon.
    /// </summary>
    public bool IsMovementActive { get; set; }
    
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
        
        // movement
        if (!IsMovementActive)
            return;

        if (_inputHandler.IsChangeFeaturePressed())
        {
            Debug.Log("Invoke");
            HandsOverMovement?.Invoke();
        }
    }
    
    private void FixedUpdate()
    {
        if (!IsMovementActive)
            return;
        
        _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.fixedDeltaTime);
    }
}
