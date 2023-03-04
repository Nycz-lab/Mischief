using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipable : Item
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void pickUp(Transform equipSocket)
    {
        transform.position = equipSocket.position;
        gameObject.SetActive(false);
        transform.SetParent(equipSocket);
    }
}
