using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ChangeCamInTutorial : MonoBehaviour
{
    public GameObject CamGroup;
    /// <summary>
    /// The new target for the camera target list.
    /// </summary>
    public GameObject NextCamTarget;
    /// <summary>
    /// The index to place the new target.
    /// </summary>
    public int InsteadOfIdx;
    
    private CinemachineTargetGroup targets;
    
    
    // Start is called before the first frame update
    void Start()
    {
      targets = CamGroup.GetComponent<CinemachineTargetGroup>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Raccoon"))
        {
            CinemachineTargetGroup.Target t = new CinemachineTargetGroup.Target();
            t.radius = 0;
            t.weight = 1;
            t.target = NextCamTarget.transform;
            targets.m_Targets[InsteadOfIdx] = t;
        }
    }
}
