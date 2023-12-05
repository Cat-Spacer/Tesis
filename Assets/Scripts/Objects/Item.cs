using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ItemType
{
    Flower
}
public class Item : MonoBehaviour
{
    public ItemType type;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rb;
    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }
    
    public virtual void PickUp(PlayerCharacter player, bool pickUp)
    {
        if (pickUp)
        {
            player.PickUp(this);
            HasPhysics(false);
        }
    }
    public virtual void Drop(Vector2 dir, float dropForce)
    {
        HasPhysics(true);
        _rb.AddForce(dir * dropForce);
    }

    public void HasPhysics(bool has)
    {
        if (has)
        {
            _boxCollider.enabled = false;
            _rb.simulated = false;
        }
        else
        {
            _boxCollider.enabled = false;
            _rb.simulated = false;
        }

    }
    public ItemType Type()
    {
        return type;
    }
}
