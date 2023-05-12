using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class NodePoint : MonoBehaviour
{
    [Header("Stats")]

    public List<NodePoint> neighbours;

    public int cost = 1;

    private float viewRadius = 1.2f;

    public Renderer _meshRenderer;

    [Header("Layers")]
    public LayerMask obstacleMask = 27;
    public LayerMask nodesMask = 23;
    public bool showGizmos = true;

    Spider spider;
    private void Awake()
    {
        spider = FindObjectOfType<Spider>();

        spider.pathNodes.Add(this);
        obstacleMask = LayerMask.GetMask("Shield");
        nodesMask = LayerMask.GetMask("Tube");

        _meshRenderer = GetComponent<Renderer>();

        neighbours.Clear();
        
    }

    private void Start()
    {
        GetNeightbours();
    }

    void GetNeightbours()
    {
        Collider2D[] nodesInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius);

        foreach (var n in nodesInViewRadius)
        {
            var _node = n.GetComponentInChildren<NodePoint>();

            if (_node != null && _node != this && !neighbours.Contains(_node) && InSight(transform.position, n.gameObject.transform.position))
            {
                neighbours.Add(_node);
            }


            // && GameManager.instance.allNodes.Contains(_node)
        }

        foreach (var n in neighbours)
        {
            if (!InSight(transform.position, n.gameObject.transform.position))
                neighbours.Remove(n.GetComponent<NodePoint>());
        }
    }
    public bool InSight(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;
        if (!Physics2D.Raycast(start, dir, dir.magnitude, obstacleMask)) return true;
        else return false;
    }
    void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, viewRadius);

            Gizmos.color = Color.blue;
            foreach (var n in neighbours)
            {
                Vector3 direction = n.transform.position - transform.position;
                Gizmos.DrawRay(transform.position, direction);
            }
        }
    }
    /*public bool blocked;
    
    void SetBlocked(bool b)
    {
        blocked = b;
        //Color color = b ? Color.black : Color.white;
        //GameManager.instance.ChangeGameObjectColor(gameObject, color);
    }

   

    private void OnMouseOver()
    {
        if (Input.GetKey(KeyCode.UpArrow)) ChangeCost(cost + 1);
        if (Input.GetKey(KeyCode.DownArrow)) ChangeCost(cost - 1);
        if (Input.GetKey(KeyCode.R)) ChangeCost(1);
    }


    void ChangeCost(int c)
    {
        if (c < 1) c = 1;
        cost = c;

       // ChangeTextCost();
    }

    void ChangeTextCost()
    {
        textCost.enabled = cost > 1;
        textCost.text = cost.ToString();
    }

    void SetBlocked(bool b)
    {
        blocked = b;
        Color color = b ? Color.black : Color.white;
        //GameManager.instance.ChangeGameObjectColor(gameObject, color);
    }*/
}