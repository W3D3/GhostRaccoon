using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    public List<Vector3> waypoints;
    private int index;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        transform.position = waypoints[0];
        transform.LookAt(waypoints[1 % waypoints.Count]);
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var waypoint in waypoints)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(waypoint, 0.3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(agent);
        if (agent.remainingDistance == 0f)
        {
            Debug.DrawLine(transform.position, waypoints[index % waypoints.Count]);
            if (waypoints != null)
            {
                agent.SetDestination(waypoints[index % waypoints.Count]);
                index++;
            }
        }
    }

    void alert(Transform emitterPos)
    {
        
    }
}
