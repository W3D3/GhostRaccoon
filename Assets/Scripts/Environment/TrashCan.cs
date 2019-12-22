using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Raccoon"))
        {
            other.gameObject.GetComponent<Raccoon>().HideInTrash();
            this.gameObject.GetComponentInChildren<TrashTail>().ShowTail();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Raccoon"))
        {
            other.gameObject.GetComponent<Raccoon>().ShowFromTrash();
            other.gameObject.SetActive(true);
            this.gameObject.GetComponentInChildren<TrashTail>().HideTail();
        }
    }
}
