using System;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;

public class PeaceFlower : Item, IInteract, IDamageable
{
    private Vector3 _startPos;
    private Rigidbody2D rb;
    private Action DetectAction = delegate {  };
    [SerializeField] private Vector2 size;
    private LayerMask itemLayerMask;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject arrow;
    private bool isPutDown;
    private bool onFloor = true;
    public override void Start()
    {
        base.Start();
        itemLayerMask = gameObject.layer;
        _startPos = transform.position;
        arrow.SetActive(false);
    }

    private void Update()
    {
        DetectAction();
        
        if (_target == null || isPutDown || onFloor) return;
        var dir = _target.position - transform.position;
        if (dir.magnitude < 2)
        {
            arrow.SetActive(false);
        }
        else
        {
            arrow.SetActive(true);
            arrow.transform.up = _target.position - transform.position;
        }
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
        onFloor = false;
        arrow.SetActive(true);
        DetectAction = delegate {  };
    }

    public override void Drop(Vector2 dir, float dropForce)
    {
        base.Drop(dir, dropForce);
        onFloor = true;
        arrow.SetActive(false);
        transform.localScale = Vector3.one;
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
                if (statue != null && !statue.HasFlower())
                {
                    isPutDown = true;
                    arrow.SetActive(false);
                    statue.PutFlower(this);
                }
            }

        }
    }

    public void GetDamage()
    {
        transform.position = _startPos;
    }
}
