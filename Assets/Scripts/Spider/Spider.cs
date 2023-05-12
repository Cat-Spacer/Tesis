using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [Header("Stats")]
    public float speed;
  //  private Vector2 _velocity;
  //  public float maxSpeed;
   // public float maxForce;


    public Hamster _target;

    [Header("Field of View")]
    public float viewRadius;
    public float viewAngle;
    public LayerMask obstacleMask;
    public float followArea;
    //private NodePoint _startingNode;
    // private NodePoint _goalNode;
    public NodePoint homeNode;

    public List<NodePoint> areaNodes;

    [Header("WayPoints")]
    public List<NodePoint> wayPoints = new List<NodePoint>();
    public int _wayPointIndex = 0;


    public int current = 0;

    private void Start()
    {
        followArea = 3;
        speed = 1;
        obstacleMask = LayerMask.GetMask("Shield");
        _target = FindObjectOfType<Hamster>();
    }

    /*  private void Update()
      {
          transform.position += _velocity * Time.deltaTime;
          transform.forward = _velocity;
      }*/

   public  bool attacked = false;
    public void Attack()
    {
        _target.HamsterCatched();
        attacked = true;
        Debug.Log("_spider attack");
    }

    public List<NodePoint> ConstructPathAStar(Vector2 pos, NodePoint goalNode)
    {
        NodePoint startingNode = CheckNearestStart(pos);


        if (startingNode == null && goalNode == null) return default;

        PriorityQueue frontier = new PriorityQueue();
        frontier.Put(startingNode, 0);

        Dictionary<NodePoint, NodePoint> cameFrom = new Dictionary<NodePoint, NodePoint>();
        Dictionary<NodePoint, int> costSoFar = new Dictionary<NodePoint, int>();
        cameFrom.Add(startingNode, null);
        costSoFar.Add(startingNode, 0);

        while (frontier.Count() > 0)
        {
            NodePoint current = frontier.Get();

            if (current == goalNode)
            {
                List<NodePoint> path = new List<NodePoint>();

                NodePoint nodeToAdd = current;
                while (nodeToAdd != null)
                {
                    path.Add(nodeToAdd);
                    nodeToAdd = cameFrom[nodeToAdd];
                }

                path.Reverse();

                return path;
            }

            foreach (var next in current.neighbours)
            {
                int newCost = costSoFar[current] + next.cost;
                float priority = newCost + Heuristic(next.transform.position, goalNode.transform.position);
                if (!costSoFar.ContainsKey(next))
                {
                    frontier.Put(next, priority);
                    costSoFar.Add(next, newCost);
                    cameFrom.Add(next, current);
                }
                else if (costSoFar.ContainsKey(next) && newCost < costSoFar[next])
                {
                    frontier.Put(next, priority);
                    costSoFar[next] = newCost;
                    cameFrom[next] = current;
                }
            }
        }


        return default;
    }
    public void Move(List<NodePoint> path)
    {
        if (path.Count == 0) return;

        if (current < path.Count-1 && Vector2.Distance(transform.position, path[current].transform.position) < 0.15f)
        {
            current++;
        }

        transform.position = Vector2.MoveTowards(transform.position, path[current].transform.position, speed * Time.deltaTime);

    }
    public float Heuristic(Vector2 a, Vector2 b)
    {
        return Vector2.Distance(a, b);
    }

    public NodePoint CheckNearestStart(Vector2 pos)
    {
        PriorityQueue e = new PriorityQueue();
        foreach (var item in areaNodes)
        {
            if (InSight(pos, item.transform.position)) 
                e.Put(item, Heuristic(item.transform.position, pos));
        }
        return e.Get();
    }
    public bool InSight(Vector2 start, Vector2 end)
    {
        Vector2 dir = end - start;
        if (!Physics2D.Raycast(start, dir, dir.magnitude, obstacleMask)) return true;
        else return false;
    }
   /* public void ApplyForce(Vector2 force)
    {
        _velocity += force;
        _velocity = Vector3.ClampMagnitude(_velocity, maxSpeed);
    }*/
}
