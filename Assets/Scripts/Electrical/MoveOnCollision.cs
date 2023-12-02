using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnCollision : MonoBehaviour
{
    [SerializeField] private LayerMask _colMask;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((_colMask.value & (1 << collision.gameObject.layer)) > 0) collision.transform.SetParent(transform);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //if ((_colMask.value & (1 << collision.gameObject.layer)) > 0) collision.transform.SetParent(GameManager.Instance.GetConfig.mainGame);
    }
}