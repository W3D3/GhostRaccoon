using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Cinemachine.Utility;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

public class Raccoon : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private GamepadInput _inputHandler;
    private Animator _animator;
    private SoundEmitter _soundEmitter;
    private bool _hiddenInTrash = false;
    private Vector3 _velocity;

    public UnityEvent HandsOverMovement;
    public UnityEvent<Raccoon> Died = new RaccoonUnityEvent();
    public UnityEvent Won;

    public float MoveSpeed = 5;

    /// <summary>
    /// Forces movement independent of xor movement.
    /// </summary>
    public bool ForceMovement;

    /// <summary>
    /// Boolean if the movement is active for this raccoon.
    /// </summary>
    public bool IsMovementActive { get; set; }

    /// <summary>
    /// Flag if raccoon is dead.
    /// </summary>
    public bool IsDead;

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
        
        var forward = Camera.main.transform.forward;
        var right = Camera.main.transform.right;
        
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        
        _velocity = (forward * _inputHandler.GetLeftVerticalValue() + right * _inputHandler.GetLeftHorizontalValue()).normalized * MoveSpeed;

        if (IsMovementActive && !_hiddenInTrash)
            HandleMovement();
    }

    private void HandleMovement()
    {
        _inputHandler.IsRegularFireReleased();
        _inputHandler.IsSpecialFireReleased();
        
        if (this.ToString().Contains("Raccoon1") && _inputHandler.IsChangeFeaturePressed()
          || this.ToString().Contains("Raccoon2") && _inputHandler.IsAltChangeFeaturePressed()
          || _inputHandler.IsSpecialFirePressed())
        {
            HandsOverMovement?.Invoke();
        }

        if (this.ToString().Contains("Raccoon1") && _inputHandler.IsShockwaveFeaturePressed()
            || this.ToString().Contains("Raccoon2") && _inputHandler.IsAltShockwaveFeaturePressed()
            || _inputHandler.IsRegularFirePressed())
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
            StopMoving();
        }
    }

    private void move()
    {
        transform.LookAt(_rigidbody.position + _velocity * Time.fixedDeltaTime);
        _animator.SetFloat("Speed", 5);
        _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.fixedDeltaTime);
    }

    public void StopMoving()
    {
        _animator.SetFloat("Speed", 0);
    }

    public async void Die()
    {
        IsDead = true;
        IsMovementActive = false;
        
        StartCoroutine(Wait5SecondsAsync());
        CapsuleCollider raccoonCollider = this.GetComponent<CapsuleCollider>();
        if (raccoonCollider != null)
        {
            raccoonCollider.isTrigger = true;
        }
        Rigidbody rigidbody = this.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.isKinematic = true;
        }
        
        Died?.Invoke(this);
    }

    private IEnumerator Wait5SecondsAsync()
    {
        _animator.SetTrigger("Dying");
        yield return new WaitForSeconds(.7f);
        SoundManager.Instance.playDeath();
    }

    public void Win()
    {
        this.gameObject.SetActive(false);

        Won?.Invoke();
    }

    public void HideInTrash()
    {
        _hiddenInTrash = true;
        GetComponentInChildren<SkinnedMeshRenderer>().enabled=false;
    }

    public void ShowFromTrash()
    {
        _hiddenInTrash = false;
        GetComponentInChildren<SkinnedMeshRenderer>().enabled=true;
    }
}
