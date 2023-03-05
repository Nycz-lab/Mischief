using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun_railgun : Equipable
{

    public float shootRange = 50f;
    public int dmgPoints = 20;
    public override void showInfo()
    {


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

    public override void hideInfo()
    {

        if (infoBubble != null)
        {
            Destroy(infoBubble.gameObject);
        }
    }

    public override void pickUp(Transform equipSocket)
    {
        base.pickUp(equipSocket);
        transform.Rotate(new Vector3(-50f, -100f, 0f));
    }

    public override void UsePrimary()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootRange))
        {
            simpleTarget target;
            if(hit.collider.gameObject.TryGetComponent<simpleTarget>(out target))
            {
                target.damage(dmgPoints);
            }
        }

        GameObject.Find("Railgun/muzzle").GetComponent<ParticleSystem>().Play();
    }
}
