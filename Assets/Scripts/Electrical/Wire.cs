using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    [SerializeField] Wire _UpWire, _RightWire, _DownWire, _LeftWire;
    Vector3 center;
    [SerializeField] LayerMask _wireMask;
    private void Start()
    {
        center = transform.position;
        CheckWires();       
    }

    void CheckWires()
    {
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 1, _wireMask);
        if (hitUp) _UpWire = hitUp.transform.gameObject.GetComponent<Wire>();
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 1, _wireMask);
        if (hitRight) _RightWire = hitRight.transform.gameObject.GetComponent<Wire>();
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 1, _wireMask);
        if (hitDown) _DownWire = hitDown.transform.gameObject.GetComponent<Wire>();
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 1, _wireMask);
        if (hitLeft) _LeftWire = hitLeft.transform.gameObject.GetComponent<Wire>();
    }
}
