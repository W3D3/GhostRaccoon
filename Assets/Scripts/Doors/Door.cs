using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{


private    enum DoorDirection
    {
        Open,
        Close
    }


    private Animator _animator;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        _animator.SetTrigger(DoorDirection.Open.ToString());
    }

    public void Close()
    {
        _animator.SetTrigger(DoorDirection.Close.ToString());    
    }
}
