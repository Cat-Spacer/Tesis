using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ItemType
{
    Flower,
    Statue
}
public class Item : MonoBehaviour
{
    public ItemType type;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rb;
    public virtual void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }
    
    public virtual void PickUp(PlayerCharacter player, bool pickUp)
    {
        player.PickUp(this);
        var parent = player.GetInventoryTransform();
        transform.position = parent.position;
        transform.parent = parent;
        HasPhysics(false);
    }
    public virtual void Drop(Vector2 dir, float dropForce)
    {
        Debug.Log("Drop");
        transform.parent = null;
        HasPhysics(true);
        _rb.AddForce(dir * dropForce);
    }

    public void HasPhysics(bool has)
    {
        _boxCollider.enabled = has;
        _rb.simulated = has;
    }
    public ItemType Type()
    {
        return type;
    }
}
