using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleTarget : MonoBehaviour
{
    public float hitPoints = 100.0f;

    public void damage(float dmgPoints)
    {
        hitPoints -= dmgPoints;
        if(hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
