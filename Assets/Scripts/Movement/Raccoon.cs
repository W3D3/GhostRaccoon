using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

public class Raccoon : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private GamepadInput _inputHandler;
    private Animator _animator;
    private SoundEmitter _soundEmitter;

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

    /**
     * Prefab for the shockwave
     */
    public GameObject ShockwavePrefab;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<GamepadInput>();
        _soundEmitter = GetComponent<SoundEmitter>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ForceMovement)
            IsMovementActive = true;

        _velocity = new Vector3(_inputHandler.GetLeftHorizontalValue(), 0, _inputHandler.GetLeftVerticalValue())
                        .normalized * moveSpeed;

        if (!IsMovementActive)
            return;

        if (this.ToString().Contains("Raccoon1") && _inputHandler.IsChangeFeaturePressed()
            || this.ToString().Contains("Raccoon2") && _inputHandler.IsAltChangeFeaturePressed())
        {
            HandsOverMovement?.Invoke();
        }

        if (this.ToString().Contains("Raccoon1") && _inputHandler.IsShockwaveFeaturePressed()
            || this.ToString().Contains("Raccoon2") && _inputHandler.IsAltShockwaveFeaturePressed())
        {
            SoundManager.Instance.playRacNoise();
            _soundEmitter.makeSound();
            Instantiate(ShockwavePrefab, transform.position, Quaternion.identity);
        }
    }

    private void FixedUpdate()
    {
        if (!IsMovementActive)
            return;

        if (!_velocity.AlmostZero())
        {
            move();
        }
        else
        {
            stop();
        }
    }

    private void move()
    {
        transform.LookAt(_rigidbody.position + _velocity * Time.fixedDeltaTime);
        _animator.SetFloat("Speed", 5);
        _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.fixedDeltaTime);
    }

    private void stop()
    {
        _animator.SetFloat("Speed", 0);
    }

    public void Die()
    {
        // TODO kill for real
        Debug.Log("killed");
        _animator.SetTrigger("Dying");
    }
}
