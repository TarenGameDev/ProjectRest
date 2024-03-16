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

    [Header("Variables")]
    [SerializeField] float _heightThreshold = 0.3f;
    public float characterHeight = 2f;

    List<PathNode> _openNodes = new List<PathNode>();
    List<PathNode> _closedNodes = new List<PathNode>();
    PathNode _originNode;
    PathNode _destinationNode;
    PathNode _currentNode;

    public List<Vector3> GeneratePath(Vector3 origin, Vector3 destination)
    {
        Debug.Log("Beginning path generation.");

        //level out the y of both.
        float heightDistance = origin.y - destination.y;
        if(heightDistance < _heightThreshold)
        {
            destination.y = origin.y;
        }

        var list = new List<Vector3>();

        _originNode = new(origin);
        _destinationNode = new(destination);

        //FLAT MODE FIRST: Start simple boys
        _openNodes = CreateGrid(_originNode, _destinationNode);

        _currentNode = _originNode;

        //while( _openNodes.Count > 0 )
        //{
        //    if(_currentNode.position == _destinationNode.position)
        //    {
        //        //GOAL
        //        //break
        //    }

        //    break;
        //}

        //while nodesToSearch > 0 || current node != destination node
            //current node find neighbours
            //remove current from to search
            //add current to searched
            //var next =  lowest F Cost in the list of neighbours
            //set next node.previous = current node
            //next becomes new current node
            //If current node == destinationNode
                //THATS THE PATH!
                //go back through the previous nodes until you get back to origin, adding each to the list
                //reverse the list
            
        


        Debug.LogError("Not implemented.");
        return list;
    }

    List<PathNode> CreateGrid(PathNode origin, PathNode destination)
    {
        Debug.Log("Creating Grid.");
        var list = new List<PathNode>();

        //note the start and end
        list.Add(origin); list.Add(destination);

        //get row and column count
        int xDifference = (int) (origin.position.x - destination.position.x);
        int zDifference = (int) (origin.position.z - destination.position.z);

        bool left = false;
        int xStepCount = xDifference; 
        if (xStepCount < 0)
        {
            xStepCount *= -1;
            left = true;
        }

        bool forward = false;
        int zStepCount = zDifference; 
        if (zStepCount < 0)
        {
            zStepCount *= -1;
            forward = true;
        }

        Vector3 start = new();
        start.y = characterHeight;
        Debug.LogError("This doesnt work yet");
        for(int i = 0; i < xStepCount; i++)
        {

            for (int j = 0; j < zStepCount; j++)
            {
                //I HATE MYSELF

                Ray ray = new(start, Vector3.down);
                if(Physics.Raycast(ray, out RaycastHit hit, characterHeight + _heightThreshold, ~3))
                {
                    Debug.DrawRay(start, Vector3.down, Color.green, 2f);
                }
                else
                {
                    Debug.DrawRay(start, Vector3.down, Color.red, 1f);
                }
            }
        }
            

        Debug.Log("grid of x:" + xStepCount + ", z:" + zStepCount);

        
        
        //Shoot raycasts aimed at the origins height from above; gapped by 1 on the X and Z
        //Make the raycasts start x units above the point, and shoot down.
        //If hitpoint.y is within threshold && vector position != origin/destination
            //add point to searching list.
        //  else. ignore ray.

        //origin node g = 0
        //origin node h = CalculateDistance(origin, destination)
        Debug.LogError("Not implemented.");
        return list;
    }


    const int straightCost = 10;
    const int diagCost = 14;
    int CalculateDistance(Vector3 origin, Vector3 destination)
    {
        int OriginX = (int)origin.x * 10;
        int OriginZ = (int)origin.z * 10;
        int DestinationX = (int)origin.x * 10;
        int DestinationZ = (int)origin.z * 10;
        int x = Mathf.Abs(OriginX - DestinationX);
        int z = Mathf.Abs(OriginZ - DestinationZ);
        int remaining = Mathf.Abs(x - z);

        return diagCost * Mathf.Min(x, z) + straightCost * remaining;
    }

    PathNode LowestFCostNode(List<PathNode> list)
    {
        PathNode lowest = list[0];
        for(int i = 1; i< list.Count; i++) 
            if (list[i].fCost < lowest.fCost) lowest = list[i];    

        return lowest;
    }
}

[System.Serializable]
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

        

        Debug.LogError("Not implemented.");
        return list;
    }
}
