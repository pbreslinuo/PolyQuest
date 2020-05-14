using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWaypoint : MonoBehaviour
{
    public Transform[] destinations;
    public float moveSpeed = 1.0f;
    private float stoppingDist;

    private float dist;
    private float distCovered;
    private float fracJourney;
    private float startTime;
    private Vector3 startingPos;
    private Vector3 nextWaypoint;
    private int waypointCounter;
    public bool move;

    void Start()
    {
        move = true;
        stoppingDist = 0.25f;
        waypointCounter = -1;
        SetupNextWaypoint();
    }

    void Update()
    {
        if (move)
        {
            distCovered = (Time.time - startTime) * moveSpeed;
            fracJourney = distCovered / dist;
            transform.position = Vector3.Lerp(startingPos, nextWaypoint, fracJourney);
            if (Vector3.Distance(transform.position, nextWaypoint) < stoppingDist)
            {
                SetupNextWaypoint();
            }
        }
    }

    void SetupNextWaypoint()
    {
        waypointCounter = (waypointCounter + 1) % destinations.Length;
        SetPositions();
    }

    public void SetPositions()
    {
        startTime = Time.time;
        startingPos = transform.position;
        nextWaypoint = destinations[waypointCounter].position;
        dist = Vector3.Distance(startingPos, nextWaypoint);
    }
}