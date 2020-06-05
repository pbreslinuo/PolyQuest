using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnightWaypoint : EnemyWaypoint
{
    private Animator m_Animator;
    private bool standingStill;
    public bool anyMovement;

    void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
        stallTime = 3; // seconds
        endWait = 0;
        move = true;
        stoppingDist = 0.1f;
        waypointCounter = -1;

        if (destinations.Length == 0)
        {
            anyMovement = false;
        }
        else
        {
            anyMovement = true;
            SetupNextWaypoint();
        }
    }

    void Update()
    {
        if (!anyMovement)
        { // if the Knight has no waypoints
            m_Animator.SetBool("IsWalking", false);
        }
        else
        { // if the Knight has waypoints
            DecideIfStandingStill(); // Knights wait for a while when they reach a waypoint
            if (move && !standingStill)
            {
                m_Animator.SetBool("IsWalking", true);
                distCovered = (Time.time - startTime) * moveSpeed;
                fracJourney = distCovered / dist;
                transform.position = Vector3.Lerp(startingPos, nextWaypoint, fracJourney);
                if (Vector3.Distance(transform.position, nextWaypoint) < stoppingDist)
                {
                    SetupNextWaypoint();
                    endWait = Time.time + stallTime;
                    startTime += stallTime;
                }
            }
            else
            {
                m_Animator.SetBool("IsWalking", false);
            }
        }
    }

    private void DecideIfStandingStill()
    {
        if (Time.time < endWait)
        {
            standingStill = true;
        }
        else
        {
            standingStill = false;
        }
    }
}