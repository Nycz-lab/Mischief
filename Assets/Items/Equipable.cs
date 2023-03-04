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

    public virtual void drop(Vector3 playerPos)
    {
        Debug.Log("test");

        transform.SetParent(null, true);
        gameObject.SetActive(true);

        transform.rotation = Camera.main.transform.rotation;
        transform.position = Camera.main.transform.position;
        GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 3;

        

        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<BoxCollider>().enabled = true;
    }

    public virtual void pickUp(Transform equipSocket)
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().enabled = false;
        transform.position = equipSocket.position;
        gameObject.SetActive(false);
        transform.SetParent(equipSocket);
    }
}
