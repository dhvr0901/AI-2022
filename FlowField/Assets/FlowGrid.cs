using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FlowGrid : MonoBehaviour
{
    //a list of nodes with extra functionality
    struct Path
    {
        public Path(Node toAdd)
        {
            order = new List<Node>();
            order.Add(toAdd);

            //used to account for diagonals being longer from center to corner
            directions = new List<int>();
            directions.Add(0);

            length = toAdd.GetWeight();
        }
        public Path(Node toAdd, int direction, Path original)
        {
            order = new List<Node>();
            foreach (Node element in original.order)
            {
                order.Add(element);
            }
            order.Add(toAdd);

            directions = new List<int>();
            foreach (int element in original.directions)
            {
                directions.Add(element);
            }
            directions.Add(direction);


            length = 0;
            for(int i = 0; i < order.Count; i++)
            {
                length += order[i].GetWeight() * (1 + directions[i] % 2 * 1);
            }
        }

        float getLength()
        {
            float total = 0;
            foreach(Node node in order)
            {
                total += node.GetWeight();
            }
            return total;
        }

        public List<Node> order;
        public List<int> directions;
        public float length;
    }

    //a list of paths with extra functionality
    struct PathList
    {
        public PathList(Path initial)
        {
            paths = new List<Path>();
            paths.Add(initial);
        }

        public List<Path> paths;

        //add a path based on length (not functional in this way)
        public void Add(Path toAdd)
        {
            Debug.Log("count is: " + paths.Count);
            int i = 0;
            if (paths.Count - 1 > i)
            {
                while (paths[i].length < toAdd.length && i < paths.Count -1)
                {
                    Debug.Log("iterator is: " + i);
                    i++;
                }
            }
            if (i == paths.Count)
            {
                paths.Add(toAdd);
            }
            else
            {
                paths.Insert(i, toAdd);
            }
        }

        //removes and returns the smallest (first) element
        public Path Pop()
        {
            Path temp = paths[0];
            paths.RemoveAt(0);
            return temp;
        }

    }

    public Node[,] graph;
    [SerializeField]
    private Vector2 size;
    [SerializeField]
    private GameObject baseNode;

    private Vector2 currentPoint;


    // Start is called before the first frame update
    void Start()
    {
        graph = new Node[(int)size.x, (int)size.y];
        PopulateGrid();
        ConnectGrid();
        EstablishGrid(new Vector2(0, 0));
    }


    //instantiate a grid of nodes
    private void PopulateGrid()
    {
        for (int i = 0; i < size.x; i++)
        {
            for(int c = 0; c < size.y; c++)
            {
                graph[i, c] = Instantiate(baseNode, this.transform).GetComponent<Node>();
                graph[i, c].transform.position = new Vector3(5 + c * 10, transform.position.y, 5 + i * 10);
            }
        }
    }

    //can be used to update changes to the grid
    private void ConnectGrid()
    {
        for (int i = 0; i < size.x - 1; i++)
        {
            for (int c = 0; c < size.y - 1; c++)
            {
                Node temp = graph[i, c];
                for(int j = 0; j < 8; j++)
                {
                    temp.SetConnection(j, null);
                }
            }
        }

                for (int i = 0; i < size.x - 1; i++)
        {
            for (int c = 0; c < size.y - 1; c++)
            {
                Node temp = graph[i, c];
                if(temp.GetAvailable())
                {
                    //plus x node
                    Node tempTwo = graph[i + 1, c];
                    if (tempTwo.GetAvailable())
                    {
                        temp.SetConnection(0, tempTwo);
                        tempTwo.SetConnection(4, temp);
                    }

                    //plus x/y node
                    tempTwo = graph[i + 1, c + 1];
                    if (tempTwo.GetAvailable())
                    {
                        temp.SetConnection(1, tempTwo);
                        tempTwo.SetConnection(5, temp);
                    }

                    //plus y node
                    tempTwo = graph[i, c + 1];
                    if (tempTwo.GetAvailable())
                    {
                        temp.SetConnection(2, tempTwo);
                        tempTwo.SetConnection(6, temp);
                    }

                    //plus y minus x node
                    if (i > 0)
                    {
                        tempTwo = graph[i - 1, c + 1];
                        if (tempTwo.GetAvailable())
                        {
                            temp.SetConnection(3, tempTwo);
                            tempTwo.SetConnection(7, temp);
                        }
                    }
                }
            }
        }
    }

    //set flow out from chosen node
    private void EstablishGrid(Vector2 flowPoint)
    {
        currentPoint = flowPoint;
        //nodes explored so far
        List<Node> explored = new List<Node>();

        //how many nodes are available
        int availableNow = GetAvailable();

        //start the open list with the point to flow to (this will be ordered from shortest to longest)
        PathList open = new PathList(new Path(graph[(int)flowPoint.x, (int)flowPoint.y]));

        //while not all nodes have been explored
        while (explored.Count < availableNow)
        {
            //smallest open path
            Path smallest = open.Pop();

            //last node in path
            Node last = smallest.order[smallest.order.Count - 1];

            //for every valid connection in last
            for (int i = 0; i < 8; i++)
            {
                if (last.GetConnection(i))
                {
                    Node temp = last.GetConnection(i);
                    if (!explored.Contains(temp))
                    {
                        open.Add(new Path(temp, i, smallest)); ///index out of range here for list get item
                        temp.SetFlow(new Vector3(last.transform.position.x - temp.transform.position.x, 0, last.transform.position.z - temp.transform.position.z));
                        explored.Add(temp);
                    }
                }
            }
        }
    }

    //count all available nodes (unavailable nodes act ass barriers)
    private int GetAvailable()
    {
        int count = 0;
        foreach(Node node in graph)
        {
            if (node.GetAvailable())
                count++;
        }
        return count;
    }

    public void RefreshGrid()
    {
        ConnectGrid();
        EstablishGrid(currentPoint);
    }

    public void NewFlow(Node point)
    {
        Vector2 temp = FindNode(point);
        if(temp.x > -1)
        {
            EstablishGrid(temp);
        }
    }

    //returns (-1, -1) if not found
    public Vector2 FindNode(Node point)
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int c = 0; c < size.y; c++)
            {
                if(graph[i, c] == point)
                {
                    return new Vector2(i, c);
                }
            }
        }
        return new Vector2(-1, -1);
    }
}
