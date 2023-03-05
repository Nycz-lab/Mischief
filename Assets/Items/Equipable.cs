using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipable : Item
{
    public bool inInventory = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        regainBehaviour();
    }

    public virtual void regainBehaviour()   // without this the item will go flying to space because of collision
    {
        Collider itemCollider = GetComponent<Collider>();
        if (!inInventory && !itemCollider.enabled) // if the item is not equipped and not enabled
        {
            if (CheckIfNoPlayerCollider(Physics.OverlapSphere(transform.position, 0.5f)))
            {
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Collider>().enabled = true;
            }
        }
    }

    public virtual bool CheckIfNoPlayerCollider(Collider[] colliderArr) // checks if the object still collides with the player
    {
        foreach (Collider collider in colliderArr)
        {
            //Debug.Log("Colliding with " + collider);
            if (collider.CompareTag("Player"))
            {
                return false;
            }
        }
        return true;
    }

    public override void drop(Vector3 playerPos)
    {
        inInventory = false;

        transform.SetParent(null, true);
        gameObject.SetActive(true);

        transform.rotation = Camera.main.transform.rotation;
        transform.position = Camera.main.transform.position;
        GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 5;

        

        
    }

    public virtual void pickUp(Transform equipSocket)
    {
        inInventory = true;

        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Collider>().enabled = false;
        transform.position = equipSocket.position;
        gameObject.SetActive(false);
        transform.SetParent(equipSocket);
    }
}
