using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tube : MonoBehaviour, IInteract
{
    [SerializeField] TubeSystem tubeSystem;
    [SerializeField] private bool playerInTube = false;
    private Action moveAction;
    List<Tube> tubes = new List<Tube>();
    [SerializeField] private bool isCheckpoint;
    [SerializeField] private bool isEntry;
    [SerializeField] Tube rightTube;
    [SerializeField] Tube leftTube;
    [SerializeField] Tube upTube;
    [SerializeField] Tube downTube;
    InteractEnum _interactEnum;
    public bool IsCheckpoint(){return isCheckpoint;}

    private void Start()
    {
        moveAction = delegate { };
        tubes.Add(rightTube);
        tubes.Add(leftTube);
        tubes.Add(upTube);
        tubes.Add(downTube);
    }

    private void Update()
    {
        moveAction();
    }

    public Tube MoveRight()
    {
        return rightTube;
    }
    public Tube MoveLeft()
    {
        return leftTube;
    }
    public Tube MoveUp()
    {
        return upTube;
    }
    public Tube MoveDown()
    {
        return downTube;
    }
    public void Interact(params object[] param)
    {
        var obj = (GameObject)param[0];
        var player = obj.GetComponent<HamsterChar>();
        if(!tubeSystem.IsPlayerOnTube())
        {
            Debug.Log("Player Enter Tube");
            tubeSystem.EnterTubeSystem(true);
            player.GetInTube(transform.position, this);
        }
        else
        {
            Debug.Log("Player Exit Tube");
            tubeSystem.EnterTubeSystem(false);
            player.GetOutOfTube(transform.position);
        }
    }

    public void ShowInteract(bool showInteractState)
    {
        
    }

    public InteractEnum GetInteractType()
    {
        return _interactEnum;
    }

    public Vector2 GetCenter()
    {
        return transform.position;
    }

    public Tube GetNextPath(Tube lastTube)
    {
        return tubes.FirstOrDefault(x => x != lastTube && x != null);
    }

    public void OnTube(bool state)
    {
        playerInTube = state;
    }
    public bool HasRightPath() { return rightTube != null;}
    public bool HasLeftPath() { return leftTube != null;}
    public bool HasUpPath() { return upTube != null;}
    public bool HasDownPath() { return downTube != null;}
}
