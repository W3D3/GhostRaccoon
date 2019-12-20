using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private GamepadInput _inputHandler;
    
    private float moveSpeed = 6;
    private Vector3 _velocity;

    public UnityEvent HandsOverMovement;

    /// <summary>
    /// Forces movement independent of xor movement.
    /// </summary>
    public bool ForceMovement;

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
        if (ForceMovement)
            IsMovementActive = true;
        
        _velocity = new Vector3(_inputHandler.GetLeftHorizontalValue(),0, _inputHandler.GetLeftVerticalValue()).normalized * moveSpeed;
        
        // movement
        if (!IsMovementActive)
            return;

        // TODO remove if not using 2 keyboards
        if (this.ToString().Contains("Raccoon1") && _inputHandler.IsChangeFeaturePressed()
            || this.ToString().Contains("Raccoon2") && _inputHandler.IsAltChangeFeaturePressed())
        {
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
