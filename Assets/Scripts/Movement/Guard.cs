using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

/**
 * This is terrible code
 * please don't look at it.
 */
public class Guard : MonoBehaviour
{
    private enum Animations
    {
        Idle = 0,
        Walking = 1,
        Shooting = 2
    }
    
    public List<Vector3> waypoints;
    public float speed = 3;
    private int index = 0;
    private NavMeshAgent agent;
    private bool isAlerted = false;
    private Vector3 currentTarget;
    private Animator _animator;
    private FieldOfView _fieldOfView;
    private bool isSearching = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        transform.position = waypoints[0];
        transform.LookAt(waypoints[1 % waypoints.Count]);
        currentTarget = waypoints[index];
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

        if (currentTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(currentTarget, 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if any raccoons can be shot
        foreach (var target in _fieldOfView.visibleTargets)
        {
            Raccoon raccoon = target.GetComponent<Raccoon>();
            if (raccoon != null && !raccoon.IsDead && !raccoon.HiddenInTrash)
            {
                agent.isStopped = true;
                transform.DOLookAt(raccoon.transform.position, 2f, AxisConstraint.Y);
                _animator.SetInteger("State", (int)Animations.Shooting);
                
                SoundManager.Instance.playGunSound();

                raccoon.Die();
                Invoke("resumeNormal", 4);
            }
        }

        // Movement code
        bool closeToCurrentTarget = Mathf.Abs(transform.position.x - currentTarget.x) +
                                    Mathf.Abs(transform.position.z - currentTarget.z) <= 0.01;
        if (closeToCurrentTarget)
        {
            if (isAlerted)
            {
                agent.isStopped = true;
                isAlerted = false;
                _animator.SetInteger("State", (int)Animations.Idle);
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

    /// <summary>
    /// Takes next waypoint from list
    /// </summary>
    private void setNextDestination()
    {
        index = (index + 1) % waypoints.Count;
        Debug.Log(this.name + " is walking to pos " + waypoints[index] + " with index " + index);
        resumeNormal();
        currentTarget = waypoints[index];
        agent.SetDestination(currentTarget);
    }

    private void resumeNormal()
    {
        if(isAlerted) return;
        StopAlertedAnimation();

        agent.isStopped = false;
        _animator.SetInteger("State", (int) Animations.Walking);
    }

    public void alertGuard(Vector3 emitterPos)
    {
        // if (isAlerted) 
        //     return;
        isAlerted = true;

        AlertedAnimation();
        
        Debug.DrawRay(transform.position, emitterPos, Color.yellow, 5f);
        currentTarget = emitterPos;
        agent.SetDestination(currentTarget);
    }

    private void AlertedAnimation()
    {
        var text = GetComponentInChildren<LookAtCameraScript>(true);
        text.gameObject.SetActive(true);

        SoundManager.Instance.playDetect();
    }

    private void StopAlertedAnimation()
    {
        var text = GetComponentInChildren<LookAtCameraScript>(true);
        text.gameObject.SetActive(false);
    }
}