using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cell
{
    GameObject[] superposition;
    Vector3 pos;

    public cell(Vector3 pos)
    {
        this.pos = pos;
        loadPrefabs();
        Debug.Log("I am " + superposition[0].name + ", I am at " + pos);
    }

    private void loadPrefabs()
    {
        superposition = Resources.LoadAll<GameObject>("wfcPrefabs");
    }

    public void collapse()
    {
        
        Object.Instantiate(superposition[0], pos, Quaternion.identity);
        superposition = new GameObject[0];
    }

    public int getEntropy()
    {
        return superposition.Length;
    }
}
