using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelAnimate : MonoBehaviour
{
    private Vector3 rotatePoint;
    private Vector3 rotateAxis;
    private float rotateAmount;
    private int numRotates;
    private int numRotatesLeft; // to account for literal edge cases (haha)

    private void Start()
    {
        numRotates = 18;
        numRotatesLeft = 0;
    }

    private void Update()
    {
        if (numRotatesLeft > 0)
        {
            transform.RotateAround(rotatePoint, rotateAxis, rotateAmount);
            numRotatesLeft--;
        }
    }

    public void RotateLevelAnimation(Vector3 point, Vector3 axis, float deg)
    {
        rotatePoint = point;
        rotateAxis = axis;
        rotateAmount = deg / numRotates;
        numRotatesLeft = numRotates - numRotatesLeft;
    }
}