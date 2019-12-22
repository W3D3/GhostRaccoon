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
    public float TargetDistanceTreshold = 0.1f;

    private int index = 0;
    private NavMeshAgent agent;
    private Vector3 currentTarget;
    private Animator _animator;
    private FieldOfView _fieldOfView;

    private State currentState;

    private enum State
    {
        Patrolling = 0,
        Alerted = 1,
        Stopped = 2
    }

    private int NextWaypointIndex
    {
        get
        {
            index = (index + 1) % waypoints.Count;
            return index;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        _fieldOfView = GetComponent<FieldOfView>();

        // spawn at first waypoint
        transform.position = waypoints[0];

        var firstWaypointIdx = NextWaypointIndex;
        transform.LookAt(waypoints[firstWaypointIdx]);
        agent.speed = speed;

        setNextDestination(firstWaypointIdx);
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
        bool hitAnyRaccoon = false;
        foreach (var target in _fieldOfView.visibleTargets)
        {
            Raccoon raccoon = target.GetComponent<Raccoon>();
            if (raccoon != null && !raccoon.IsDead && !raccoon.HiddenInTrash)
            {
                agent.isStopped = true;
                transform.LookAt(raccoon.transform.position);

                _animator.SetInteger("State", (int) Animations.Shooting);
                SoundManager.Instance.playGunSound();

                raccoon.Die();
                hitAnyRaccoon = true;
            }
        }

        if (hitAnyRaccoon)
        {
            if (currentState == State.Alerted)
                StopAlertedAnimation();

            currentState = State.Patrolling;
            agent.isStopped = false;
        }

        // Movement code
        bool closeToCurrentTarget = Mathf.Abs(transform.position.x - currentTarget.x) +
                                    Mathf.Abs(transform.position.z - currentTarget.z) <= TargetDistanceTreshold;
        if (closeToCurrentTarget && currentState != State.Stopped)
        {
            if (currentState == State.Alerted)
                StopAlertedAnimation();

            SetupIdle();
            StartCoroutine(StartNextTarget());
        }
    }

    private IEnumerator StartNextTarget()
    {
        var nextIdx = NextWaypointIndex;
        try
        {
            transform.DOLookAt(waypoints[nextIdx], 2f, AxisConstraint.Y);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        yield return new WaitForSeconds(2f);

        setNextDestination(nextIdx);
    }

    /// <summary>
    /// Takes next waypoint from list
    /// </summary>
    private void setNextDestination(int nextIndex)
    {
        currentTarget = waypoints[nextIndex];
        agent.SetDestination(currentTarget);

        SetupMovement();
    }

    private void SetupMovement()
    {
        currentState = State.Patrolling;
        _animator.SetInteger("State", (int) Animations.Walking);
        agent.isStopped = false;
    }

    private void SetupIdle()
    {
        currentState = State.Stopped;
        _animator.SetInteger("State", (int) Animations.Idle);
        agent.isStopped = true;
    }

    // private void resumeNormal()
    // {
    //     if(isAlerted) return;
    //     StopAlertedAnimation();
    //
    //     agent.isStopped = false;
    //     _animator.SetInteger("State", (int) Animations.Walking);
    // }

    public void alertGuard(Vector3 emitterPos)
    {
        currentState = State.Alerted;

        AlertedAnimation();

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