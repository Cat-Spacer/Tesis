using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float jumpForce;
    [SerializeField] LayerMask mask;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((mask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            var entityRb = collision.gameObject.GetComponent<Rigidbody2D>();
            entityRb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
