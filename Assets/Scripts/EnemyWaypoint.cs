using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWaypoint : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] destinations;

    int currentWaypoint;

    void Start()
    {
        navMeshAgent.SetDestination(destinations[0].position);
    }

    void Update()
    {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            currentWaypoint = (currentWaypoint + 1) % destinations.Length;
            navMeshAgent.SetDestination(destinations[currentWaypoint].position);
        }
    }
}