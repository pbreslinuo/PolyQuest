using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private int m_Colliding;
    private float initTime;

    private void Start()
    {
        initTime = Time.timeSinceLevelLoad;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") m_Colliding++;
        if (other.tag == "Enemy")
        {
            other.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") m_Colliding--;
    }

    public bool IsColliding()
    {
        return (m_Colliding > 0);
    }

    public bool TimeOver()
    {
        return (Time.timeSinceLevelLoad - initTime > 2);
    }
}
