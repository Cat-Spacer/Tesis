using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndingScene : MonoBehaviour
{
    [SerializeField] private PlayableDirector _director;
    void Start()
    {
        _director = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Play()
    {
        _director.Play();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Play();
    }
}
