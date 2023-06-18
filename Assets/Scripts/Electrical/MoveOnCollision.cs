using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnCollision : MonoBehaviour
{
    [SerializeField] private LayerMask _colMask;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.layer & (1 << _colMask.value)) > 0)
            collision.transform.SetParent(transform);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.layer & (1 << _colMask.value)) > 0)
            collision.transform.SetParent(GameManager.Instance.GetConfig.mainGame);
    }
}
