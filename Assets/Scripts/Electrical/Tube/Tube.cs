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
    public bool IsCheckpoint(){return isCheckpoint;}
    public bool IsEntry(){return isEntry;}

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
            player.GetInTube(transform.position, this);
        }
        else
        {
            player.GetOutOfTube(transform.position, this);
        }
    }

    public void OnPlayerEnter(bool playerInTube)
    {
        tubeSystem.EnterTubeSystem(playerInTube);
    }
    public void ShowInteract(bool showInteractState)
    {
        
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

    public bool IsOnTube() { return tubeSystem.IsPlayerOnTube();}
    public bool HasRightPath() { return rightTube != null;}
    public bool HasLeftPath() { return leftTube != null;}
    public bool HasUpPath() { return upTube != null;}
    public bool HasDownPath() { return downTube != null;}
}
