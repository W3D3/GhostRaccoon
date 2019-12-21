using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaccoonUnityEvent : UnityEvent<Raccoon>
{
}

public class Soul : MonoBehaviour
{
    /// <summary>
    /// The speed of the soul.
    /// </summary>
    public float Speed = 5f;

    /// <summary>
    /// Triggered when the soul reached the goal raccoon.
    /// </summary>
    public UnityEvent<Raccoon> MovementFinished = new RaccoonUnityEvent();

    private Raccoon _fromRaccoon = null,
        _toRaccoon = null;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (_fromRaccoon == null)
            return;

        var direction = Vector3.Lerp(this.transform.position, _toRaccoon.transform.position,
            Speed * Time.deltaTime);
        this.transform.position = direction;
    }

    public void Move(Raccoon from, Raccoon to)
    {
        _fromRaccoon = from;
        _toRaccoon = to;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Raccoon>() == _toRaccoon)
        {
            MovementFinished?.Invoke(_toRaccoon);
            Destroy(this.gameObject);
        }
    }
}