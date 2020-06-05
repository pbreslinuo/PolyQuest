using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWaypoint : MonoBehaviour
{
    public Transform[] destinations;
    public float moveSpeed = 1.0f;
    public float stoppingDist;
    public float dist;
    public float distCovered;
    public float fracJourney;
    public float startTime;
    public Vector3 startingPos;
    public Vector3 nextWaypoint;
    public int waypointCounter;
    public bool move; // turns off when the level rotates and back on when it's done
    public float stallTime;
    public float endWait;

    void Start()
    {
        move = true;
        stoppingDist = 0.1f;
        waypointCounter = -1;
        SetupNextWaypoint();
    }

    public void SetupNextWaypoint()
    {
        waypointCounter = (waypointCounter + 1) % destinations.Length;
        SetPositions();
    }

    public void SetPositions()
    {
        if (destinations.Length > 0)
        {
            startTime = Time.time;
            startingPos = transform.position;
            nextWaypoint = destinations[waypointCounter].position;
            dist = Vector3.Distance(startingPos, nextWaypoint);
        }
    }
}