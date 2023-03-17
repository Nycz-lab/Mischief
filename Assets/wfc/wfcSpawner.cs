using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public struct cellPos
{
    public int x;
    public int y;

    public cellPos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return string.Format("{0}, {1}", x, y);
    }
}

public class impossibleLevelException : System.Exception
{
    public impossibleLevelException()
    {

    }
}

public class wfcSpawner : MonoBehaviour
{
    public int size = 10;

    public int cellSize = 15;

    cell[,] cellGrid;

    public int seed = 0;

    public float speed = 1;

    // Start is called before the first frame update
    void Awake()
    {
        if(seed != 0)
        {
            Random.InitState(seed);
        }
        else
        {
            seed = (int)System.DateTime.Now.Ticks;
            Random.InitState(seed);
        }
        Debug.Log("Current Seed is " + seed);

        wfc();
        
    }

    public void wfc()
    {
        createGrid();
        try
        {
            collapseLoop();
        }catch(impossibleLevelException e)
        {
            Debug.Log("Encountered impossible level design, trying again...");
            removeAllCells();
            wfc();
        }
    }

    public void collapseLoop()
    {
        cell[] lEC = getCellsWithLowestEntropy();
        while (collapseRandomCellInArray(lEC))
        {
            //Debug.Log(lEC.Length);
            lEC = getCellsWithLowestEntropy();

        }
    }

    public bool collapseRandomCellInArray(cell[] cellArr)
    {

        if (cellArr.Length <= 0) return false;
        cell c = cellArr[Random.Range(0, cellArr.Length)];
        cellConstraints cConstraints = c.collapse();
        propogateChanges(c, cConstraints);
        return true;

        
    }

    public void propogateChanges(cell c, cellConstraints cConstraints)
    {
        // loop through all array constraints and delete them from the cells that are next to the current one if that cell exists
        // delete everything thats NOT in the list

        cellPos pos = indexOf(c);

        if(pos.y + 1 < cellGrid.GetLength(1))  // another cell on the top
        {
            cell top = cellGrid[pos.x, pos.y +1];



            if (!top.collapsed)
            {
                List<GameObject> listA = new List<GameObject>(top.superposition);
                List<GameObject> listB = new List<GameObject>(cConstraints.top);

                List<GameObject> remainingPositions = listA.Intersect(listB).ToList();

                top.superposition = remainingPositions.ToArray();
            }
        }

        if(pos.y - 1 >= 0)   // another cell on the down
        {
            cell down = cellGrid[pos.x, pos.y-1];
            if (!down.collapsed)
            {
                List<GameObject> listA = new List<GameObject>(down.superposition);
                List<GameObject> listB = new List<GameObject>(cConstraints.down);

                List<GameObject> remainingPositions = listA.Intersect(listB).ToList();

                down.superposition = remainingPositions.ToArray();
            }
        }

        if(pos.x - 1 >= 0)   // another cell on the left
        {
            cell left = cellGrid[pos.x-1, pos.y];
            if (!left.collapsed)
            {
                List<GameObject> listA = new List<GameObject>(left.superposition);
                List<GameObject> listB = new List<GameObject>(cConstraints.left);

                List<GameObject> remainingPositions = listA.Intersect(listB).ToList();

                left.superposition = remainingPositions.ToArray();
            }
        }

        if(pos.x + 1 < cellGrid.GetLength(0)) // another cell on the right
        {
            cell right = cellGrid[pos.x+1, pos.y];
            if (!right.collapsed)
            {
                List<GameObject> listA = new List<GameObject>(right.superposition);
                List<GameObject> listB = new List<GameObject>(cConstraints.right);

                List<GameObject> remainingPositions = listA.Intersect(listB).ToList();

                right.superposition = remainingPositions.ToArray();
            }
        }


    }

    public cellPos indexOf(cell c)
    {
        int w = cellGrid.GetLength(0); // width
        int h = cellGrid.GetLength(1); // height

        for (int x = 0; x < w; ++x)
        {
            for (int y = 0; y < h; ++y)
            {
                if (cellGrid[x, y].Equals(c))
                    return new cellPos(x, y);
            }
        }

        return new cellPos(-1, -1);
    }

    public void removeAllCells()
    {
        GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("wfcObj");
        foreach(var obj in toDestroy)
        {
            Destroy(obj);
        }
    }

    public void collapseAllCells()
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
                cellGrid[i, j] = new cell(new Vector3(transform.position.x + (i * cellSize), transform.position.y, transform.position.z + (j * cellSize)));
            }
        }
        
    }

    /*  Returns an Array of uncollapsed Cells with the lowest Entropy
     */
    public cell[] getCellsWithLowestEntropy()
    {
        int entropy = -1;
        List<cell> lowEntropyCells = new List<cell>();
        
        foreach(var cell in cellGrid)
        {
            if(!cell.collapsed && ((cell.getEntropy() < entropy && cell.getEntropy() != 0) || entropy == -1))
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
