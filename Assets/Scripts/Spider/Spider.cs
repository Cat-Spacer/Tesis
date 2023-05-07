using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [Header("Stats")]
    public float speed;
    private Vector3 _velocity;
    public float maxSpeed;
    public float maxForce;


    public GameObject _target;

    [Header("Field of View")]
    public float viewRadius;
    public float viewAngle;
    public LayerMask obstacleMask;

    private NodePoint _startingNode;
    private NodePoint _goalNode;

    public List<NodePoint> pathNodes;

    [Header("WayPoints")]
    public List<NodePoint> wayPoints = new List<NodePoint>();
    public int _wayPointIndex = 0;


    public int current = 0;

    private void Update()
    {
        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;
    }
    public void Attack()
    {
        Debug.Log("spider attack");
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

        Vector3 desired = path[current].transform.position - transform.position;


        if (desired.magnitude < 0.15f)
        {
            current++;
        }

        desired.Normalize();
        desired *= maxSpeed;

        Vector3 steering = Vector3.ClampMagnitude(desired - _velocity, maxForce);

        ApplyForce(steering);
    }
    public float Heuristic(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b);
    }

    public NodePoint CheckNearestStart(Vector3 pos)
    {
        PriorityQueue e = new PriorityQueue();
        foreach (var item in pathNodes)
        {
            if (InSight(transform.position, item.transform.position)) e.Put(item, Heuristic(item.transform.position, pos));
        }
        return e.Get();
    }
    public bool InSight(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;
        if (!Physics.Raycast(start, dir, dir.magnitude, obstacleMask)) return true;
        else return false;
    }
    public void ApplyForce(Vector3 force)
    {
        _velocity += force;
        _velocity = Vector3.ClampMagnitude(_velocity, maxSpeed);
    }
}
