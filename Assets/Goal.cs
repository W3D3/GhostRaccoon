﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
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
            this.GetComponentInChildren<GoalTail>().ShowTail();
            
            Debug.Log("win");
            
            // todo win
        }
    }
}
