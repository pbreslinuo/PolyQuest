using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelAnimate : MonoBehaviour
{
    public EnemyWaypoint[] enemies;

    private Vector3 rotatePoint;
    private Vector3 rotateAxis;
    private float rotateAmount;
    private int numRotates;
    private int numRotatesLeft; // to account for literal edge cases (haha)
    private bool finished;

    private void Start()
    {
        finished = false;
        numRotates = 18;
        numRotatesLeft = 0;
    }

    private void Update()
    {
        if (numRotatesLeft > 0)
        {
            transform.RotateAround(rotatePoint, rotateAxis, rotateAmount);
            numRotatesLeft--;
            if (numRotatesLeft == 0)
            {
                finished = true;
            }
        }
        if (finished)
        {
            foreach (EnemyWaypoint enemy in enemies)
            {
                enemy.move = true;
                enemy.SetPositions();
            }
            finished = false;
        }
    }

    public void RotateLevelAnimation(Vector3 point, Vector3 axis, float deg)
    {
        rotatePoint = point;
        rotateAxis = axis;
        rotateAmount = deg / numRotates;
        numRotatesLeft = numRotates - numRotatesLeft;
        foreach (EnemyWaypoint enemy in enemies)
        {
            enemy.move = false;
        }
    }
}