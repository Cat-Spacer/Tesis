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
        EventManager.Instance.Subscribe(EventType.OnChangePeace, OnChangePeace);
    }

    private void OnChangePeace(object[] obj)
    {
        Debug.Log("OnChangePeace");
        if (isShowing == false)
        {
            isShowing = true;
            animator.Play("ShowCrowd");
        }
    }

    void StopShowing()
    {
        isShowing = false;
    }
}
