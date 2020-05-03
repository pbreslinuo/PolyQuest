using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    public GameObject edges;
    public GameObject player;
    public GameLevelAnimate animatedLevel;
    public float cubeWidth;        // set this publically where cubeWidth = cube's scale 
    private float halfWidth;
    private Vector3 rotatePoint;

    private void Start()
    {
        cubeWidth += 2;          // assume that player is one unit away from background cube
        halfWidth = cubeWidth / 2;
    }

    private void Update()
    {
        if (player.transform.position.x > edges.transform.position.x + halfWidth)
        {
            DoRotation(Vector3.right, Vector3.up);
        }
        if (player.transform.position.x < edges.transform.position.x - halfWidth)
        {
            DoRotation(Vector3.left, Vector3.down);
        }
        if (player.transform.position.y > edges.transform.position.y + halfWidth)
        {
            DoRotation(Vector3.up, Vector3.left);
        }
        if (player.transform.position.y < edges.transform.position.y - halfWidth)
        {
            DoRotation(Vector3.down, Vector3.right);
        }
    }

    private void DoRotation(Vector3 translateAxis, Vector3 rotateAxis)
    {
        rotatePoint = edges.transform.position + translateAxis * halfWidth;
        edges.transform.Translate(translateAxis * cubeWidth);
        transform.RotateAround(rotatePoint, rotateAxis, 90);
        animatedLevel.RotateLevelAnimation(rotatePoint, rotateAxis, 90);
    }
}