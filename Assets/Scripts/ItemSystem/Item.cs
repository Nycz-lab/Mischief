using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Item : MonoBehaviour, IInteractable
{

    public string itemName;
    public int itemPrice;
    public Sprite itemIcon;
    public int maxStack;

    protected Transform infoBubble;

    

    //public string itemName { get; set; }

    public virtual void Awake()
    {
        this.itemName = transform.name;
    }


    public virtual void UsePrimary()
    { 
        // Code for using the item

    }

    public virtual void UseSecondary()
    {
        // Code for using the item

    }

    public virtual void showInfo()
    {

        gameObject.GetComponent<Renderer>().material.SetFloat("_Outline_Width", 30);

        if (infoBubble == null)
        {
            infoBubble = ChatBubble.Create(transform.transform, Camera.main.transform.forward * -1, this.itemName);
        }
        else
        {
            infoBubble.LookAt(Camera.main.transform);
            infoBubble.localPosition = Camera.main.transform.forward * -1;
        }

    }

    public virtual void hideInfo()
    {
        Renderer r;
        if(gameObject.TryGetComponent<Renderer>(out r)) r.material.SetFloat("_Outline_Width", 0);


        if (infoBubble != null)
        {
            Destroy(infoBubble.gameObject);
        }
    }

    public virtual void Interact()
    {
        gameObject.SetActive(false);
    }

    public virtual void drop(Vector3 playerPos)
    {
        //transform.position = playerPos + Vector3.up;

        transform.rotation = Camera.main.transform.rotation;
        transform.position = Camera.main.transform.position;
        GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 3;


        gameObject.SetActive(true);
    }

}
