using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Item : MonoBehaviour
{

    public string itemName;
    public int itemPrice;
    public Sprite itemIcon;
    public int maxStack;
    

    //public string itemName { get; set; }

    public virtual void Awake()
    {
        this.itemName = transform.name;
    }

    public virtual void Use()
    { 
        // Code for using the item
    }

    public virtual void showInfo()
    {
        //    if (gameObject.GetComponent<TextMesh>() != null) return;
        //    TextMesh tm = gameObject.AddComponent<TextMesh>();
        //    tm.text = this.itemName;

        //    tm.color = Color.white;

        // TODO this works for now but change this later
        GameObject.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = this.itemName;
        gameObject.GetComponent<Renderer>().material.SetFloat("_Outline_Width", 30);


    }

    public virtual void hideInfo()
    {
        // TODO this works for now but change this later
        GameObject.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = "";
        gameObject.GetComponent<Renderer>().material.SetFloat("_Outline_Width", 0);
    }

    public virtual void pickUp()
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
