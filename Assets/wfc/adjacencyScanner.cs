using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public struct adjacentStore
{
    public GameObject obj;
    public cellConstraints constraints;

    public adjacentStore(GameObject obj, cellConstraints constraints)
    {
        this.obj = obj;
        this.constraints = constraints;
    }
}

public class adjacencyScanner
{
    int space;
    Dictionary<string, adjacentStore> rules = new Dictionary<string, adjacentStore>();

    public adjacencyScanner(int space)
    {
        this.space = space;
    }


    int getVerticeHashCode(GameObject obj)
    {
        int hashCode = 0;
        MeshFilter mf = obj.GetComponent<MeshFilter>();
        if(mf != null)
        {
            foreach (var vertex in mf.mesh.vertices)
            {
                hashCode = hashCode ^ vertex.GetHashCode();
            }
        }

        //for (int i = 0; i < obj.transform.childCount; i++)
        //{
        //    GameObject childObj = obj.transform.GetChild(i).gameObject;
        //    int childHashCode = getVerticeHashCode(childObj);
        //    hashCode ^= childHashCode;
        //}

        //hashCode ^= obj.gameObject.name.GetHashCode();

        return hashCode;
    }

    string addIfNotExist(GameObject obj)
    {
        string tileId = obj.GetComponent<wfc_tile>().tileId;
        if (!rules.ContainsKey(tileId))
        {
            cellConstraints cC = new cellConstraints();
            cC.top = new GameObject[0];
            cC.down = new GameObject[0];
            cC.left = new GameObject[0];
            cC.right = new GameObject[0];

            rules.Add(tileId, new adjacentStore(obj, cC));
        }
        return tileId;
    }

    void calculateConstraints(GameObject obj)
    {
        cellConstraints curConstraints;
        List<GameObject> top;
        List<GameObject> down;
        List<GameObject> left;
        List<GameObject> right;

        string tileId = obj.GetComponent<wfc_tile>().tileId;
        bool selfNeighbour = obj.GetComponent<wfc_tile>().selfNeighbour;

        Debug.Log("YIKES " + tileId);

        if (rules.ContainsKey(tileId))
        {
            Debug.Log("found " + obj);
            curConstraints = rules[tileId].constraints;
            top = new List<GameObject>(curConstraints.top);
            down = new List<GameObject>(curConstraints.down);
            left = new List<GameObject>(curConstraints.left);
            right = new List<GameObject>(curConstraints.right);
        }
        else
        {

            curConstraints = new cellConstraints();
            top = new List<GameObject>();
            down = new List<GameObject>();
            left = new List<GameObject>();
            right = new List<GameObject>();
        }
        //  top
        Collider[] colliderArr = Physics.OverlapSphere(obj.transform.position + Vector3.forward * space, space / 2);
        foreach(var collider in colliderArr)
        {
            if (collider.tag != "wfcObj") continue;
            top.Add(rules[addIfNotExist(collider.gameObject)].obj);
        }
        //  down
        colliderArr = Physics.OverlapSphere(obj.transform.position + Vector3.back * space, space / 2);
        foreach (var collider in colliderArr)
        {
            if (collider.tag != "wfcObj") continue;
            down.Add(rules[addIfNotExist(collider.gameObject)].obj);
        }
        //  left
        colliderArr = Physics.OverlapSphere(obj.transform.position + Vector3.left * space, space / 2);
        foreach (var collider in colliderArr)
        {
            if (collider.tag != "wfcObj") continue;
            left.Add(rules[addIfNotExist(collider.gameObject)].obj);
        }
        //  right
        colliderArr = Physics.OverlapSphere(obj.transform.position + Vector3.right * space, space / 2);
        foreach (var collider in colliderArr)
        {
            if (collider.tag != "wfcObj") continue;
            right.Add(rules[addIfNotExist(collider.gameObject)].obj);
        }

        if (selfNeighbour)
        {
            top.Add(rules[addIfNotExist(obj)].obj);
            down.Add(rules[addIfNotExist(obj)].obj);
            left.Add(rules[addIfNotExist(obj)].obj);
            right.Add(rules[addIfNotExist(obj)].obj);
        }

        curConstraints.top = top.Distinct().ToArray();
        curConstraints.down = down.Distinct().ToArray();
        curConstraints.left = left.Distinct().ToArray();
        curConstraints.right = right.Distinct().ToArray();

        if (rules.ContainsKey(tileId))
        {
            Debug.Log("found " + obj);
            adjacentStore current = rules[tileId];
            current.constraints = curConstraints;
            rules[tileId] = current;
        }
        else
        {
            Debug.Log("adding " + obj + " to dict");
            
            rules.Add(tileId, new adjacentStore(obj, curConstraints));
        }
    }

    public GameObject[] getSuperpositions()
    {
        GameObject[] sceneObj = GameObject.FindGameObjectsWithTag("wfcObj");


        foreach(GameObject obj in sceneObj)
        {
            calculateConstraints(obj);
        }

        List<GameObject> tileSet = new List<GameObject>();

        foreach(var key in rules.Keys)
        {
            adjacentStore adjStore = rules[key];
            constraints currentConstraints = adjStore.obj.AddComponent<constraints>();
            currentConstraints.top = adjStore.constraints.top;
            currentConstraints.down = adjStore.constraints.down;
            currentConstraints.left = adjStore.constraints.left;
            currentConstraints.right = adjStore.constraints.right;
            tileSet.Add(adjStore.obj);
        }

        Debug.Log(string.Join(", ", rules.Keys));
        
        return tileSet.ToArray();
    }

}
