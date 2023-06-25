using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterInput : MonoBehaviour
{
    public CameraBirdCageFollow cam;
    [SerializeField] float _radius;
    Vector3 targetPosition;
    IMouseOver lastInteraction;
    [SerializeField] GameObject lastInteraction2;
    [SerializeField] LayerMask interactLayerMask;
    [SerializeField] Collider2D coll;

    //public HamsterInput(Hamster ham)
    //{
    //    _hamster = ham;
    //}

    private void Update()
    {
        Control();
    }

    void Control()
    {
        targetPosition = cam.MousePosition();
        coll = Physics2D.OverlapCircle(targetPosition, _radius, interactLayerMask);
        if (!coll)
        {
            if (lastInteraction != null)
            {
                lastInteraction.MouseExit();
            }
            return;
        }
        var interact = coll.GetComponent<IMouseOver>();
        if (interact != null) //Tiene
        {
            interact.MouseOver();
            lastInteraction = interact;
            lastInteraction2 = coll.gameObject;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) interact.Interact();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetPosition, _radius);
    }
}
