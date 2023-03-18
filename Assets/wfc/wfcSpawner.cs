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

    public float seconds = 0;

    public bool farPropagation = true;
    public int max_depth = 3;

    /// <summary>
    /// this calls the wfc function after setting the seed
    /// </summary>
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

    /// <summary>
    /// main function to start the logic
    /// upon a map failure event (impossibleLevel) this restarts the map generation
    /// </summary>
    public void wfc()
    {
        createGrid();
        try
        {
            if(seconds != 0)
            {
                StartCoroutine(collapseLoopSeconds());
            }
            else
            {

                collapseLoop();
            }
        }catch(impossibleLevelException e)
        {
            Debug.Log("Encountered impossible level design, trying again...");
            removeAllCells();
            wfc();
        }
    }

    /// <summary>
    /// This function starts the collapse of all cells in the map
    /// </summary>
    public void collapseLoop()
    {
        cell[] lEC = getCellsWithLowestEntropy();
        while (collapseRandomCellInArray(lEC))
        {
            lEC = getCellsWithLowestEntropy();

        }
    }

    /// <summary>
    /// Use this to generate the map slowly to visualize it
    /// </summary>
    /// <returns></returns>
    public IEnumerator collapseLoopSeconds()
    {

        cell[] lEC = getCellsWithLowestEntropy();
        Debug.Log("Found " + lEC.Length + " cells with entropy of " + lEC[0].superposition.Length);
        while (collapseRandomCellInArray(lEC))
        {
            lEC = getCellsWithLowestEntropy();
            if(lEC.Length > 0) Debug.Log("Found " + lEC.Length + " cells with entropy of " + lEC[0].superposition.Length);
            yield return new WaitForSeconds(seconds);

        }
    }


    /// <summary>
    /// This function randomly collapses a cell within a given array
    /// </summary>
    /// <param name="cellArr"> The array of which a cell is randomly collapsed
    /// (most commonly this is from an array of low entropy cells) </param>
    /// <returns>returns true if there are cells to collapse and false if all cells have been collapsed</returns>
    public bool collapseRandomCellInArray(cell[] cellArr)
    {

        if (cellArr.Length <= 0) return false;
        cell c = cellArr[Random.Range(0, cellArr.Length)];
        cellConstraints cConstraints = c.collapse();
        propagateChanges(c, cConstraints);
        return true;

        
    }

    public void oldpropagateChanges(cell c) // causes stackOverflow
    {
        cellPos pos = indexOf(c);
        Debug.Log(pos);

        if (pos.y + 1 < cellGrid.GetLength(1))  // another cell on the top
        {
            cell top = cellGrid[pos.x, pos.y + 1];
            oldpropagateChanges(top);
        }

        if (pos.y - 1 >= 0)   // another cell on the down
        {
            cell down = cellGrid[pos.x, pos.y - 1];
            oldpropagateChanges(down);
        }

        if (pos.x - 1 >= 0)   // another cell on the left
        {
            cell left = cellGrid[pos.x - 1, pos.y];
            oldpropagateChanges(left);
        }

        if (pos.x + 1 < cellGrid.GetLength(0)) // another cell on the right
        {
            cell right = cellGrid[pos.x + 1, pos.y];
            oldpropagateChanges(right);
        }
    }

    /// <summary>
    /// this propagates the cells in advance so that their state becomes more deterministic
    /// this helps create logic levels with less failures but is incredibly performance intensive the more depth is added
    /// </summary>
    /// <param name="c"></param>
    /// <param name="max_depth"></param>
    public void propagateChanges(cell c, int max_depth)
    {

        Queue<cell> queue = new Queue<cell>();
        bool[,] visited = new bool[cellGrid.GetLength(0), cellGrid.GetLength(1)];

        queue.Enqueue(c);

        while (queue.Count > 0)
        {
            cell current = queue.Dequeue();
            cellPos pos = indexOf(current);
            visited[pos.x, pos.y] = true;
            //Debug.Log(pos);

            if (pos.y + 1 < cellGrid.GetLength(1) && queue.Count < max_depth)  // another cell on the top
            {
                cell top = cellGrid[pos.x, pos.y + 1];
                if(!visited[pos.x, pos.y + 1] && !top.collapsed)
                {
                    HashSet<GameObject> superPos = new HashSet<GameObject>();
                    foreach (var cellPos in current.superposition)
                    {
                        foreach(var posi in cellPos.GetComponent<constraints>().top)
                        {
                            superPos.Add(posi);
                        }
                    }

                    //top.superposition = superPos.ToArray();
                    collapseDirection(top, superPos.ToArray());
                    queue.Enqueue(top);
                }
            }

            if (pos.y - 1 >= 0 && queue.Count < max_depth)   // another cell on the down
            {
                cell down = cellGrid[pos.x, pos.y - 1];
                if (!visited[pos.x, pos.y - 1] && !down.collapsed)
                {
                    HashSet<GameObject> superPos = new HashSet<GameObject>();
                    foreach (var cellPos in current.superposition)
                    {
                        foreach (var posi in cellPos.GetComponent<constraints>().down)
                        {
                            superPos.Add(posi);
                        }
                    }

                    //down.superposition = superPos.ToArray();
                    collapseDirection(down, superPos.ToArray());
                    queue.Enqueue(down);
                }
            }

            if (pos.x - 1 >= 0 && queue.Count < max_depth)   // another cell on the left
            {
                cell left = cellGrid[pos.x - 1, pos.y];
                if (!visited[pos.x -1, pos.y] && !left.collapsed)
                {
                    HashSet<GameObject> superPos = new HashSet<GameObject>();
                    foreach (var cellPos in current.superposition)
                    {
                        foreach (var posi in cellPos.GetComponent<constraints>().left)
                        {
                            superPos.Add(posi);
                        }
                    }

                    //left.superposition = superPos.ToArray();
                    collapseDirection(left, superPos.ToArray());
                    queue.Enqueue(left);
                }
            }

            if (pos.x + 1 < cellGrid.GetLength(0) && queue.Count < max_depth) // another cell on the right
            {
                cell right = cellGrid[pos.x + 1, pos.y];
                if (!visited[pos.x + 1, pos.y] && !right.collapsed)
                {
                    HashSet<GameObject> superPos = new HashSet<GameObject>();
                    foreach (var cellPos in current.superposition)
                    {
                        foreach (var posi in cellPos.GetComponent<constraints>().right)
                        {
                            superPos.Add(posi);
                        }
                    }

                    //right.superposition = superPos.ToArray();
                    collapseDirection(right, superPos.ToArray());
                    queue.Enqueue(right);
                }
            }
        }
    }

    /// <summary>
    /// this is the propagation function for direct neighbours of the cell
    /// </summary>
    /// <param name="c"></param>
    /// <param name="cConstraints"></param>
    public void propagateChanges(cell c, cellConstraints cConstraints)
    {
        // loop through all array constraints and delete them from the cells that are next to the current one if that cell exists
        // delete everything thats NOT in the list

        cellPos pos = indexOf(c);

        if(pos.y + 1 < cellGrid.GetLength(1))  // another cell on the top
        {
            cell top = cellGrid[pos.x, pos.y +1];
            if (!top.collapsed)
            {
                collapseDirection(top, cConstraints.top);
                
                if(farPropagation) propagateChanges(top, max_depth);
            }
        }

        if(pos.y - 1 >= 0)   // another cell on the down
        {
            cell down = cellGrid[pos.x, pos.y-1];
            if (!down.collapsed)
            {
                collapseDirection(down, cConstraints.down);
                if (farPropagation) propagateChanges(down, max_depth);
            }
        }

        if(pos.x - 1 >= 0)   // another cell on the left
        {
            cell left = cellGrid[pos.x-1, pos.y];
            if (!left.collapsed)
            {
                collapseDirection(left, cConstraints.left);
                if (farPropagation) propagateChanges(left, max_depth);
            }
        }

        if(pos.x + 1 < cellGrid.GetLength(0)) // another cell on the right
        {
            cell right = cellGrid[pos.x+1, pos.y];
            if (!right.collapsed)
            {
                collapseDirection(right, cConstraints.right);
                if (farPropagation) propagateChanges(right, max_depth);
            }
        }



    }

    /// <summary>
    /// helper function to set the left logical superpositions based on the intersect of both cells superpositions
    /// </summary>
    /// <param name="c"></param>
    /// <param name="direction"></param>
    public void collapseDirection(cell c, GameObject[] direction)
    {
        List<GameObject> listA = new List<GameObject>(c.superposition);
        List<GameObject> listB = new List<GameObject>(direction);

        List<GameObject> remainingPositions = listA.Intersect(listB).ToList();

        c.superposition = remainingPositions.ToArray();
    }

    /// <summary>
    /// helper function to find a cell in the 2d grid array
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
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

    /// <summary>
    /// remove all cells from the game world (used to regenerate the map for example)
    /// </summary>
    public void removeAllCells()
    {
        GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("wfcObj");
        foreach(var obj in toDestroy)
        {
            Destroy(obj);
        }
    }

    /// <summary>
    /// helper function to get random unlogical maps
    /// </summary>
    public void collapseAllCells()
    {
        foreach(var cell in cellGrid)
        {
            cell.collapse();
        }
    }

    /// <summary>
    /// create the cell grid through this helper function
    /// </summary>
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

    /// <summary>
    /// Returns an Array of uncollapsed Cells with the lowest Entropy
    /// </summary>
    /// <returns></returns>
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

}
