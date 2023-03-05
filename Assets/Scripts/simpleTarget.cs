using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleTarget : MonoBehaviour
{
    public int hitPoints = 100;

    public void damage(int dmgPoints)
    {
        hitPoints -= dmgPoints;
        if(hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
