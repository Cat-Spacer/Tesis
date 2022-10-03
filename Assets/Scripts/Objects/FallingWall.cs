using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingWall : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] LayerMask mask;
    [SerializeField] GameObject player;
    [SerializeField] float gravityScale;
    [SerializeField] RigidbodyConstraints2D constraints2D;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Activate()
    {
        rb.constraints = constraints2D;
        rb.gravityScale = gravityScale;
        player.transform.SetParent(transform);
    }    
    private void Deactivate()
    {
        //rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.gravityScale = 0;
        player.transform.parent = null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((mask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            player = collision.gameObject;          
            Activate();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((mask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            player = collision.gameObject;
            Deactivate();
        }
    }
}
