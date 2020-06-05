using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatWaypoint : EnemyWaypoint
{
    void Start()
    {
        move = true;
        stoppingDist = 0.1f;
        waypointCounter = -1;
        SetupNextWaypoint();
        stallTime = 0;
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
}