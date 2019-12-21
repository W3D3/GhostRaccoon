using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

/**
 * This is terrible code
 * please don't look at it.
 */
public class Guard : MonoBehaviour
{
    public List<Vector3> waypoints;
    public float speed = 3;
    private int index = 0;
    private NavMeshAgent agent;
    private bool isAlerted = false;
    private Vector3 alertTarget;
    private Animator _animator;
    private FieldOfView _fieldOfView;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        transform.position = waypoints[0];
        transform.LookAt(waypoints[1 % waypoints.Count]);
        agent = GetComponent<NavMeshAgent>();
        _fieldOfView = GetComponent<FieldOfView>();
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
    void Update()
    {
        if (agent.velocity.magnitude < 1)
        {
            _animator.SetInteger("State", 0);
        }
        else
        {
            _animator.SetInteger("State", 1);
        }
        // Check if any raccoons can be shot
        foreach (var target in _fieldOfView.visibleTargets)
        {
            Raccoon raccoon = target.GetComponent<Raccoon>();
            if (raccoon != null)
            {
                agent.isStopped = true;
                _animator.SetInteger("State", 2);
                // TODO SoundManager.Instance.PlayGunSound()
                raccoon.Die();
            }
        }
        // Movement code
        bool close = Mathf.Abs(transform.position.x - waypoints[index].x) +  Mathf.Abs(transform.position.z - waypoints[index].z) == 0;
        if (close || agent.isPathStale)
        {
            if (isAlerted)
            {
                agent.isStopped = true;
                isAlerted = false;
                _animator.SetInteger("State", 4);
                Invoke("resumeNormal", 4);
                return;
            }
            
            if (waypoints != null && !agent.isStopped)
            {
                agent.isStopped = true;
                _animator.SetInteger("State", 0);
                try
                {
                    transform.DOLookAt(waypoints[(index + 1) % waypoints.Count], 2f, AxisConstraint.Y);
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
        index = (index + 1) % waypoints.Count;
        Debug.Log(this.name + " is walking to pos " + waypoints[index] + " with index " + index);
        resumeNormal();
        agent.SetDestination(waypoints[index]);
        
    }

    private void resumeNormal()
    {
        agent.isStopped = false;
        _animator.SetInteger("State", 1);
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
