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
    }

}
