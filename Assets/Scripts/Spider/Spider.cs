using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [Header("Stats")]
    public float speed;
    public Hamster _target;

    [Header("Field of View")]
    public float viewRadius, viewAngle, followArea, attackArea = 0.25f, attackTimer = 0.5f;
    public LayerMask obstacleMask;
    public NodePoint homeNode;

    public List<NodePoint> areaNodes;

    [Header("WayPoints")]
    public List<NodePoint> wayPoints = new List<NodePoint>();
    [SerializeField] private float _nodeSearchRad = 0.1f;
    [SerializeField] private LayerMask _nodeLayer;
    public int _wayPointIndex = 0, current = 0;
    public bool attacked = false;


    [SerializeField] private GameObject _alertImage;
    [SerializeField] private SoundManager.Types _sound = SoundManager.Types.Spider;

    private void Start()
    {
        followArea = 3;
        obstacleMask = LayerMask.GetMask("Shield");
        obstacleMask += LayerMask.GetMask("HamsterEnemy");
        _target = FindObjectOfType<Hamster>();
        if (!homeNode) homeNode = Physics2D.OverlapCircle(transform.position, _nodeSearchRad, _nodeLayer).GetComponent<NodePoint>();
    }

    public void Alert(bool state_arg)
    {
        _alertImage.gameObject.SetActive(state_arg);
    }

    public void Attack()
    {
        SoundManager.instance.Play(_sound, false);
        _target.Die();
        attacked = true;
    }

    public List<NodePoint> ConstructPathAStar(Vector2 pos, NodePoint goalNode)
    {
        if (!goalNode) return new List<NodePoint>();

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

            // foreach (var next in current.neighbours)
            // {
            //     int newCost = costSoFar[current] + next.cost;
            //     float priority = newCost + Heuristic(next.transform.position, goalNode.transform.position);
            //     if (!costSoFar.ContainsKey(next))
            //     {
            //         frontier.Put(next, priority);
            //         costSoFar.Add(next, newCost);
            //         cameFrom.Add(next, current);
            //     }
            //     else if (costSoFar.ContainsKey(next) && newCost < costSoFar[next])
            //     {
            //         frontier.Put(next, priority);
            //         costSoFar[next] = newCost;
            //         cameFrom[next] = current;
            //     }
            // }
        }


        return default;
    }

    public void Move(List<NodePoint> path)
    {
        if (path.Count <= 0 || path == null) return;

        if (current < path.Count - 1 && Vector2.Distance(transform.position, path[current].transform.position) < 0.15f)
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
}
