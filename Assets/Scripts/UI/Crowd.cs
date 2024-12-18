using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    Animator animator;
    private bool isShowing;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        if(GameManager.Instance == null) return;
        EventManager.Instance.Subscribe(EventType.OnChangePeace, OnChangePeace);
    }
    private void OnChangePeace(object[] obj)
    {
        Debug.Log("OnChangePeace");
        if (isShowing == false)
        {
            if(SoundManager.instance != null) SoundManager.instance.Play(SoundsTypes.CrowdSurprised, gameObject);
            isShowing = true;
            animator.Play("ShowCrowd");
        }
    }

    void StopShowing()
    {
        isShowing = false;
    }
}
