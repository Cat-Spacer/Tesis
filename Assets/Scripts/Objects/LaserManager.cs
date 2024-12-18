using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LaserManager : MonoBehaviour
{
    private Action activateAction = delegate { };
    [SerializeField] List<Laser> lasers = new List<Laser>();
    [SerializeField] private float laserLifeTime;
    [SerializeField] private float laserDelayTime;
    private float currentTime;

    private int current;

    private void Start()
    {
        activateAction = DelayTimeCountdown;
        current = -1;
    }

    private void Update()
    {
        activateAction();
    }

    void Open()
    {
        current++;
        if (current > lasers.Count - 1) current = 0;
        
        foreach (var laser in lasers[current].lasers)
        {
            laser.Activate();
        }

    }

    void Close()
    {
        foreach (var laser in lasers[current].lasers)
        {
            laser.Desactivate();
        }
    }
    void LifeTimeCountdown()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= laserLifeTime)
        {
            Close();
            currentTime = 0;
            activateAction = DelayTimeCountdown;
        }
    }

    void DelayTimeCountdown()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= laserDelayTime)
        {
            Open();
            currentTime = 0;
            activateAction = LifeTimeCountdown;
        }
    }
}

[Serializable]
public class Laser
{
    [SerializeField] public List<LaserTrap> lasers = new List<LaserTrap>();
}