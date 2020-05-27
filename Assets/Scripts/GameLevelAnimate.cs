using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelAnimate : MonoBehaviour
{
    public EnemyWaypoint[] enemies;
    public GameObject gate;
    public GameObject gateEnd;
    public GameObject[] texts;
    public GameObject panel;
    public GameObject gameSwitch;
    public GameObject winText;

    private Vector3 rotatePoint;
    private Vector3 rotateAxis;
    private float rotateAmount;
    private int numRotates;
    private int numRotatesLeft; // to account for literal edge cases (haha)
    private bool finished;
    private bool displayText;
    private int textNum;

    // the following is for gate raising animation
    public float gateSpeed;
    private float dist;
    private float distCovered;
    private float fracJourney;
    private float startTime;
    private Vector3 startingPos;
    private Vector3 endingPos;
    public bool moveGate;

    private void Start()
    {
        finished = false;
        moveGate = false;
        displayText = true;
        numRotates = 18;
        numRotatesLeft = 0;
        textNum = 0;
        winText.SetActive(false);
        if (texts.Length > 0)
        {
            ChangeText();
        }
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
            if (moveGate)
            { // if level starts rotating when the gate is moving, just delete the gate animation
                gate.SetActive(false);
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
        if (moveGate) // gate raising animation with Lerp
        {
            distCovered = (Time.time - startTime) * gateSpeed;
            fracJourney = distCovered / dist;
            gate.transform.position = Vector3.Lerp(startingPos, endingPos, fracJourney);
            if (fracJourney > 1)
            {
                moveGate = false;
                gate.SetActive(false);
            }
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
        if (texts.Length > 0)
        {
            ChangeText();
        }
    }

    public void AnimateGate()
    { // used in levels with gates
        moveGate = true;
        startTime = Time.time;
        startingPos = gate.transform.position;
        endingPos = gateEnd.transform.position;
        dist = Vector3.Distance(startingPos, endingPos);

        // the following doesn't work well for all switches, only the one in the tutorial
        gameSwitch.transform.Rotate(0, 180, 0);
    }

    private void ChangeText()
    { // used in the tutorial
        foreach (GameObject text in texts)
        {
            text.SetActive(false);
        }
        if (textNum >= texts.Length)
        {
            displayText = false;
            panel.SetActive(false);
        }
        if (displayText)
        {
            texts[textNum++].SetActive(true);
        }
    }

    public void DisplayWinText()
    {
        winText.SetActive(true);
    }
}