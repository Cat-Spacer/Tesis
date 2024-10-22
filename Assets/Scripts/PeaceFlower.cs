using System;
using System.Linq;
using UnityEngine;

public class PeaceFlower : Item, IInteract
{
    private Rigidbody2D rb;
    private Action DetectAction = delegate {  };
    [SerializeField] private Vector2 size;
    private LayerMask itemLayerMask;
    public override void Start()
    {
        base.Start();
        itemLayerMask = gameObject.layer;
    }

    private void Update()
    {
        DetectAction();
    }

    public void Interact(params object[] param)
    {
        var newObj = (GameObject) param[0];
        var player = newObj.GetComponent<PlayerCharacter>();
        PickUp(player, true);
    }

    public void ShowInteract(bool showInteractState)
    {

    }

    public override void PickUp(PlayerCharacter player, bool pickUp)
    {
        base.PickUp(player, pickUp);
        DetectAction = delegate {  };
    }

    public override void Drop(Vector2 dir, float dropForce)
    {
        base.Drop(dir, dropForce);
        DetectAction = DetectSurroundings;
    }

    void DetectSurroundings()
    {
        var collision = Physics2D.OverlapBoxAll(transform.position, size, itemLayerMask);
        if (collision.Where(x => x.gameObject != gameObject) != null)
        {
            //var statue = collision.First(x => x.gameObject.GetComponent<FlowerStatue>() != null).GetComponent<FlowerStatue>();
            foreach (var col in collision)
            {
                var statue = col.gameObject.GetComponent<FlowerStatue>();
                if (statue != null)
                {
                    statue.PutFlower(this);
                }
            }

        }
    }
    public InteractEnum GetInteractType()
    {
        return default;
    }
}
