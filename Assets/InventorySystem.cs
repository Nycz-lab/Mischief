using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventorySystem : MonoBehaviour
{
    private StarterAssetsInputs _input; // input

    public GameObject equipSocket;

    public List<GameObject> inventory = new List<GameObject>();
    public int selectionIndex = 0;

    public float interactionDistance = 2.0f;    // distance for interacting
    private Item focussedItem;

    public int maxItems = 30;

    public GameObject activeObj;

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        checkLookingAtItem();
        checkDropItem();
        processEquip();
        //Debug.Log(_input.scrollIdx);
    }

    private void processEquip() // TODO change this wasted resources
    {
        if (inventory.Count > 0)    // this makes sure we dont access out of bounds in the inventory
        {
            selectionIndex = Mathf.Clamp(_input.scrollIdx, 0, inventory.Count - 1);
            _input.scrollIdx = selectionIndex;
        }

        if (inventory.Count <= 0) return;

        if(inventory[selectionIndex] != activeObj && activeObj != null)
        {
            if (inventory.IndexOf(activeObj) != -1)
            {
                activeObj.SetActive(false); // if the previous Equipment is still part of the Inventory then we want to hide it
            }
            Debug.Log("changed equp");
        }
        activeObj = inventory[selectionIndex];

        Item activeItem = activeObj.GetComponent<Item>();
        
        if(activeItem is Equipable)
        {
            activeObj.SetActive(true);  // Equipment is visible
        }

    }

    private void checkLookingAtItem()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance))
        {
            focussedItem = hit.collider.GetComponent<Item>();

            if (focussedItem != null)
            {
                //Debug.Log("Looking at Item " + focussedItem.itemName);
                focussedItem.showInfo();

                if (_input.interact)
                {
                    inventory.Add(hit.collider.gameObject);
                    if(focussedItem is Equipable)
                    {
                        Equipable focussedItem = hit.collider.GetComponent<Equipable>();
                        focussedItem.pickUp(equipSocket.transform);
                    }
                    else
                    {
                        focussedItem.pickUp();
                    }
                    

                    _input.interact = false;
                }
                
            }

        }
        else
        {
            if (focussedItem != null) focussedItem.hideInfo();
        }
        _input.interact = false;
    }

    private void checkDropItem()
    {
        if (!_input.dropItem) return;
        if (inventory.Count <= 0)
        {
            _input.dropItem = false;
            return;
        }

        Item item = inventory[selectionIndex].GetComponent<Item>();

        item.drop(transform.position);
        
        inventory.RemoveAt(selectionIndex);


        _input.dropItem = false;
    }
}
