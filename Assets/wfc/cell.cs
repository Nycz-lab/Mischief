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
    public cellPos gridPos;

    public GameObject finalPosition;

    public cell(Vector3 pos, cellPos gridPos, GameObject[] superposition)
    {
        this.pos = pos;
        this.gridPos = gridPos;
        this.superposition = superposition;
        //loadPrefabs();
    }

    private void loadPrefabs()
    {
        //superposition = Resources.LoadAll<GameObject>("wfcPrefabs");
        
    }

    private GameObject[] genWeights()
    {
        List<GameObject> tempList = new List<GameObject>(superposition);
        foreach(var pos in superposition)
        {
            for(int i = 0; i < pos.GetComponent<wfc_tile>().weight; i++)
            {
                tempList.Add(pos);
            }
        }
        return tempList.ToArray();
    }

    public void collapse()
    {
        collapsed = true;
        if (superposition.Length <= 0) throw new impossibleLevelException();
        var weightedSuperposition = genWeights();
        GameObject position = weightedSuperposition[Random.Range(0, weightedSuperposition.Length)];
        //GameObject position = superposition[Random.Range(0, superposition.Length)];
        Object.Instantiate(position, pos, Quaternion.identity).SetActive(true);
        superposition = new GameObject[0];
        finalPosition = position;


    }

    public void collapseSpawn()
    {
        collapsed = true;
        foreach(var supo in superposition)
        {
            if (supo.GetComponent<wfc_tile>().spawnTile)
            {
                GameObject position = supo;
                Object.Instantiate(position, pos, Quaternion.identity).SetActive(true);
                superposition = new GameObject[0];
                finalPosition = position;
                return;
            }
        }
        collapse();
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
