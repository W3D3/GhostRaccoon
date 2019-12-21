using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashTail : MonoBehaviour
{
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ShowTail()
    {
        var renderer = GetComponent<SkinnedMeshRenderer>();
        renderer.enabled = true;
        
        _animator.SetBool("Wiggle", true);
    }
    
    public void HideTail()
    {
        var renderer = GetComponent<SkinnedMeshRenderer>();
        renderer.enabled = false;
        
        _animator.SetBool("Wiggle", false);
    }
}
