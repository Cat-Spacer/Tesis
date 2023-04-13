using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovingPlatform : MonoBehaviour, IElectric
{
    Action _MoveAction = delegate { };

    public GameObject platform;
    [Header("Waypoints")]
    [SerializeField] List<Transform> waypoitns;
    public int currentWaypoint = 0;
    [Header("Data")]
    [SerializeField] float speed;
    [SerializeField] float baseSpeed;
    public float arriveMag;
    public int maxLenght = 0;
    int senseDir = 1;
    private void Start()
    {
        maxLenght = waypoitns.Count - 1;
        speed = baseSpeed;
    }
    private void Update()
    {
        _MoveAction();
    }
    void Movement()
    {
        float dist = Vector3.Distance(platform.transform.position, waypoitns[currentWaypoint].position);
        Vector3 dir = waypoitns[currentWaypoint].position - platform.transform.position;
        dir.Normalize();
        if (dist < 0.1)
        {
            if (currentWaypoint == maxLenght)
            {
                if (senseDir == 1)
                {
                    senseDir = -1;
                    maxLenght = 0;
                }
                else
                {
                    senseDir = 1;
                    maxLenght = waypoitns.Count - 1;
                }
            }
            currentWaypoint += senseDir;
        }  
        if (dist < arriveMag)
        {
            platform.transform.position += dir * speed * dist * Time.deltaTime;           
        }
        else
        {
            platform.transform.position += dir * speed * Time.deltaTime;
        }
    }

    public void TurnOn()
    {
        _MoveAction = Movement;
    }

    public void TurnOff()
    {
        _MoveAction = delegate { };
    }
}
