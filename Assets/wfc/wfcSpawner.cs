using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class wfcSpawner : MonoBehaviour
{
    public int size = 10;

    public int cellSize = 15;

    cell[,] cellGrid;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("test");
        createGrid();
        //spawnAllCells();

        cellGrid[1, 1].collapse();

        cell[] lEC = getCellsWithLowestEntropy();
        Debug.Log(lEC.Length);

    }

    //void OnValidate()
    //{
    //    //removeAllCells();
    //    Debug.Log("yikes");
    //    createGrid();

    //    spawnAllCells();
    //}

    public void removeAllCells()
    {
        GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("wfcObj");
        Debug.Log(toDestroy.Length);
        foreach(var obj in toDestroy)
        {
            Debug.Log("destroying");
            Destroy(obj);
        }
    }

    public void spawnAllCells()
    {
        foreach(var cell in cellGrid)
        {
            cell.collapse();
        }
    }

    public void createGrid()
    {
        cellGrid = new cell[size, size];
        for(int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                cellGrid[i, j] = new cell(new Vector3(transform.position.x + (i * cellSize), 0, transform.position.z + (j * cellSize)));
            }
        }
        
    }

    public cell[] getCellsWithLowestEntropy()
    {
        int entropy = -1;
        List<cell> lowEntropyCells = new List<cell>();
        
        foreach(var cell in cellGrid)
        {
            if((cell.getEntropy() < entropy && cell.getEntropy() != 0) || entropy == -1)
            {
                lowEntropyCells.Clear();
                entropy = cell.getEntropy();
            }

            if(cell.getEntropy() == entropy)
            {
                lowEntropyCells.Add(cell);
            }
        }

        return lowEntropyCells.ToArray();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
