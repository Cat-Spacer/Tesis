using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class LineCollision : MonoBehaviour
{
    [SerializeField] EdgeCollider2D _lineCollider;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] int _count = 0;
    public Action _GrowState = delegate { };
    void Start()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _lineCollider = gameObject.GetComponent<EdgeCollider2D>();
       _lineRenderer.SetPosition(0, transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        _GrowState();
    }

    public void AddPoint(int num_arg)
    {
        _lineRenderer.positionCount = num_arg + 1;
    }

    bool start = false;
    public void SetLight(Vector2 nextPos_arg, int count_arg)
    {

        count = count_arg;
        nextPos = nextPos_arg;

        Debug.Log(nextPos_arg);

        _GrowState = StartGrow;


           // Vector2 distance = nextPos_arg - (Vector2)_lineRenderer.GetPosition(count_arg - 1);
           //   Vector2 distanceSepareted = distance * 0.2f;


        //  }
    }
    Vector2 posUpdate;
    Vector2 nextPos;
    int count;
    void StartGrow()
    {
       posUpdate = _lineRenderer.GetPosition(count - 1);
        AddPoint(count);
        //posUpdate.y = nextPos.y;
        _GrowState = WhileGrow;
    }
    void WhileGrow()
    {
        if (posUpdate.x < nextPos.x)
            posUpdate.x += 1 * Time.deltaTime * 8;

        if (posUpdate.x > nextPos.x)
            posUpdate.x -= 1 * Time.deltaTime * 8;

        if (posUpdate.y < nextPos.y)
            posUpdate.y += 1 * Time.deltaTime * 8;

        if (posUpdate.y > nextPos.y)
            posUpdate.y -= 1 * Time.deltaTime * 8;
   
        _lineRenderer.SetPosition(0, transform.localPosition);
        _lineRenderer.SetPosition(count, posUpdate);

        if (posUpdate == nextPos)
            _GrowState = delegate { };
    }
    void EndGrow()
    {

    }



       
}
