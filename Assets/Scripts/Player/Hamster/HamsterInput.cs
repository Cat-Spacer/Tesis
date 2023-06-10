using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterInput : MonoBehaviour
{
    public CameraBirdCageFollow cam;
    [SerializeField] float _radius;
    Vector3 targetPosition;
    IMouseOver lastInteraction;
    [SerializeField] LayerMask interactLayerMask;

    //public HamsterInput(Hamster ham)
    //{
    //    _hamster = ham;
    //}

    private void Update()
    {
        Control();
    }
    //public void OnUpdate()
    //{
    //    Control();
    //}

    void Control()
    {
        //screenPosition = Input.mousePosition;
        //targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        targetPosition = cam.MousePosition();
        var coll = Physics2D.OverlapCircle(targetPosition, _radius, interactLayerMask);
        if (!coll) return;
        var interact = coll.GetComponent<IMouseOver>();
        if (interact != null)
        {
            interact.MouseOver();
            lastInteraction = interact;
        }
        else
        {
            Debug.Log("Sali");
            if (lastInteraction != null)
            {
                Debug.Log(lastInteraction);
                lastInteraction.MouseExit();
            }
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            interact.Interact();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetPosition, _radius);
    }
}
