using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entitySpawner : MonoBehaviour
{

    public GameObject[] entities;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, 2) == 0) return;
        Instantiate(entities[Random.Range(0, entities.Length)], transform.position, Quaternion.identity);

    }

}
