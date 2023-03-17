using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct cellConstraints
{
    public GameObject[] top;
    public GameObject[] down;
    public GameObject[] left;
    public GameObject[] right;

    public cellConstraints(GameObject[] top, GameObject[] down, GameObject[] left, GameObject[] right)
    {
        this.top = top;
        this.down = down;
        this.left = left;
        this.right = right;
    }
}



public class cell
{
    public GameObject[] superposition;
    public bool collapsed = false;
    Vector3 pos;

    public cell(Vector3 pos)
    {
        this.pos = pos;
        loadPrefabs();
        //Debug.Log("I am " + superposition[0].name + ", I am at " + pos);
    }

    private void loadPrefabs()
    {
        superposition = Resources.LoadAll<GameObject>("wfcPrefabs");
    }

    public cellConstraints collapse()
    {
        collapsed = true;
        if (superposition.Length <= 0) throw new impossibleLevelException();
        GameObject position = superposition[Random.Range(0, superposition.Length)];
        Object.Instantiate(position, pos, Quaternion.identity);
        superposition = new GameObject[0];

        return getCellConstraints(position);

    }

    public cellConstraints getCellConstraints(GameObject cellObj)
    {

        constraints cConstraints = cellObj.GetComponent<constraints>();
        return new cellConstraints(cConstraints.top, cConstraints.down, cConstraints.left, cConstraints.right);
    }

    public int getEntropy()
    {
        return superposition.Length;
    }
}
