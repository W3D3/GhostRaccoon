﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    public List<Vector3> waypoints;
    public float speed = 3;
    public int index = 1;
    private NavMeshAgent agent;
    private bool isAlerted = false;
    public Vector3 alertTarget;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        transform.position = waypoints[0];
        transform.LookAt(waypoints[1 % waypoints.Count]);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var waypoint in waypoints)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(waypoint, 0.3f);
        }

        if (alertTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(alertTarget, 0.5f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (agent.remainingDistance == 0f)
        {
            if (isAlerted)
            {
                agent.isStopped = true;
                isAlerted = false;
                // transform.DORotate(new Vector3(0, 90, 0), 1f)
                //     .OnComplete(resumeNormal);
                Invoke("resumeNormal", 4);
                return;
            }
            
            if (waypoints != null && !agent.isStopped)
            {
                // _animator.SetInteger("State", 1);
                agent.isStopped = true;
                try
                {
                    transform.DOLookAt(waypoints[index], 2f, AxisConstraint.Y);
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
                Invoke("setNextDestination", 2);
            }
        }
    }

    private void setNextDestination()
    {
        agent.isStopped = false;
        agent.SetDestination(waypoints[index]);
        index = (index + 1) % waypoints.Count;
    }

    private void resumeNormal()
    {
        agent.isStopped = false;
    }

    public void alertGuard(Vector3 emitterPos)
    {
        if(isAlerted) return;
        isAlerted = true;
        Debug.Log("alerted! going towards " + emitterPos.ToString());
        RaycastHit hitInfo;
        Physics.Raycast(transform.position, emitterPos, out hitInfo);
        Debug.DrawRay(transform.position, emitterPos, Color.yellow, 5f);
        alertTarget = emitterPos;
        agent.SetDestination(emitterPos);
        // alertTarget = hitInfo.point;
        // agent.SetDestination(hitInfo.point);
        
        index = (index + waypoints.Count - 1) % waypoints.Count;
    }
    
    
}
