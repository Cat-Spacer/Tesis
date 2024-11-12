using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LaserManager : MonoBehaviour
{
    [SerializeField] List<Laser> lasers = new List<Laser>();
    [SerializeField] private float onTime;
    [SerializeField] private float startDelay;
    [SerializeField] private float delayLoop;
    Coroutine loopCoroutine;

    private int current; 
    private void Start()
    {
        loopCoroutine = StartCoroutine(LoopTime());
    }
    void Open()
    {
        current++;
        if (current > lasers.Count - 1) current = 0;
        
        foreach (var laser in lasers[current].lasers)
        {
            laser.Activate();
        }
        loopCoroutine = StartCoroutine( OnTime());
    }

    void Close()
    {
        foreach (var laser in lasers[current].lasers)
        {
            laser.Desactivate();
        }
    }
    IEnumerator OnTime() //Lasers Lifetime
    {
        yield return new WaitForSeconds(onTime); 
        Close();
        loopCoroutine = StartCoroutine(DelayLoop());
    }
    IEnumerator DelayLoop() //Seconds of grace
    {
        yield return new WaitForSeconds(delayLoop);
        Open();
    }
    IEnumerator LoopTime()
    {
        yield return new WaitForSeconds(startDelay);
        Open();
    }
}

[Serializable]
public class Laser
{
    [SerializeField] public List<LaserTrap> lasers = new List<LaserTrap>();
}