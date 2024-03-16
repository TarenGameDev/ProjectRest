using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public static Pathfinder instance;

    private void Awake()
    {
        if(instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    List<PathNode> nodesToSearch = new List<PathNode>();
    List<PathNode> nodesSearched = new List<PathNode>();
    PathNode originNode;
    PathNode destinationNode;
    PathNode currentNode;

    public List<Vector3> GeneratePath(Vector3 origin, Vector3 destination)
    {
        Debug.Log("Beginning path generation.");
        var list = new List<Vector3>();

        originNode = new(Vector3.zero);
        destinationNode = new(destination - origin);

        //FLAT MODE FIRST: Start simple boys
        nodesToSearch = CreateGrid(origin);
        nodesToSearch.Add(originNode);
        nodesToSearch.Add(destinationNode);

        currentNode = originNode;
        //while nodesToSearch > 0
            //current node find neighbours
            //remove (!)current & (!)neighbours from to search
            //add (!)current & (!)neighbours to searched
            //find lowest F Cost in the list
            //lowest F Cost node.previous = current node
            //lowest F Cost becomes new current node
            //If current node == destinationNode
                //THATS THE PATH!
                //go back through the previous nodes until you get back to origin, adding each to the list
                //reverse the list
            
        


        Debug.LogError("Not implemented.");
        return list;
    }

    List<PathNode> CreateGrid(Vector3 offset)
    {
        Debug.Log("Creating Grid.");
        var list = new List<PathNode>();
        //Treat origin as Vector3.zero

        //Shoot raycasts aimed at the origins height from above; gapped by 1 on the X and Z
        //Make the raycasts start x units above the point, and shoot down.
        //Subtract the offset Vector3 from the point to allow for Vector3.zero change.
        //If hitpoint.y is within threshold && vector position != origin/destination
            //add point to searching list.
        //  else. ignore ray.

        Debug.LogError("Not implemented.");
        return list;
    }
}

public class PathNode
{
    public Vector3 position {  get; private set; }
    public PathNode previous { get; private set; }

    int gCost;
    int hCost;
    public int fCost { get {  return gCost + hCost; } }

    public PathNode(Vector3 pos)
    {
        position = pos;
    }

    public List<PathNode> FindNeighbours()
    {
        Debug.Log("finding neighbours for node " + position);
        var list = new List<PathNode>();

        CalculateCosts();

        Debug.LogError("Not implemented.");
        return list;
    }

    void CalculateCosts()
    {
        Debug.LogError("Not implemented");
    }
}
