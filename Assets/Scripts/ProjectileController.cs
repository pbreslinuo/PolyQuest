using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    int m_Colliding;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") m_Colliding++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") m_Colliding--;
    }

    public bool isColliding()
    {
        return (m_Colliding > 0);
    }
}
