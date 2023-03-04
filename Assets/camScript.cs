using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class camScript : MonoBehaviour
{
    public Transform headBone;
    public float smoothing = 5.0f;
    public float interactionDistance = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, headBone.position, smoothing * Time.deltaTime);


        // TODO track rotation
        //Quaternion targetRot = Quaternion.Lerp(transform.rotation, characterAnim.GetBoneTransform(HumanBodyBones.Neck).rotation, smoothing);
        //Debug.Log("target " + targetRot.eulerAngles);
        //Debug.Log("Cam " + transform.eulerAngles);
        //transform.rotation = Quaternion.Euler(targetRot.eulerAngles.x, targetRot.eulerAngles.y + 90, targetRot.eulerAngles.z + 90);
    }

    // Update is called once per frame
    void Update()
    {
        checkLookingAtItem();
    }

    private void checkLookingAtItem()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance))
        {
            Item item = hit.collider.GetComponent<Item>();

            if(item != null)
            {
                Debug.Log("Looking at Item " + item.itemName);
                item.showInfo();
            }
            else
            {
                // TODO this works for now but change this later
                GameObject.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = "";
            }

        }
        else
        {
            // TODO this works for now but change this later
            GameObject.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}
