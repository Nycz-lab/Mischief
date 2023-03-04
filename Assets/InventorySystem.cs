using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventorySystem : MonoBehaviour
{
    private StarterAssetsInputs _input; // input

    public GameObject equipSocket;

    List<GameObject> inventory = new List<GameObject>();
    int selectionIndex = 0;

    public float interactionDistance = 2.0f;
    private Item focussedItem;

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
    }

    private void processEquip() // TODO change this wasted resources
    {
        if (inventory.Count <= 0) return;
        GameObject activeObj = inventory[selectionIndex];
        Debug.Log(activeObj);
        Item activeItem = activeObj.GetComponent<Item>();
        if(activeItem is Equipable)
        {
            activeObj.SetActive(true);
            Debug.Log("kek");
        }

    }

    private void checkLookingAtItem()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance))
        {
            focussedItem = hit.collider.GetComponent<Item>();

            if (focussedItem != null)
            {
                Debug.Log("Looking at Item " + focussedItem.itemName);
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

        inventory[selectionIndex].GetComponent<Item>().drop(transform.position);
        inventory.RemoveAt(selectionIndex);


        _input.dropItem = false;
    }
}
