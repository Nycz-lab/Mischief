using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSpawner : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if(Camera.main == null)
        {
            Instantiate(player, transform.position, Quaternion.identity);
        }
    }

}
