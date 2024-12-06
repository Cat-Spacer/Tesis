using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBot : MonoBehaviour
{
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private float speed;
    [SerializeField] private float lerpTime;
    [SerializeField] private Transform target;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        var distance = (transform.position - target.transform.position).magnitude;
        if (distance > distanceToPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }

        if (transform.position.y < target.position.y)
        {
            transform.position += speed * Time.deltaTime * Vector3.up;
        }
    }

    public void P1ChangeAnimation(TutorialBoxType type)
    {
        if (animator == null) return;
        switch (type)
        {
            case TutorialBoxType.None:
                animator.Play("Happy");
                break;
            case TutorialBoxType.Movement:
                animator.Play("P1MoveKeys");
                break;
            case TutorialBoxType.Jump:
                animator.Play("P1Jump");
                break;
            case TutorialBoxType.Interact:
                animator.Play("P1Interact");
                break;
            case TutorialBoxType.Special:
                animator.Play("P1Special");
                break;
            case TutorialBoxType.Punch:
                animator.Play("P1Punch");
                break;
            case TutorialBoxType.DoubleJump:
                animator.Play("P1DoubleJump");
                break;
            case TutorialBoxType.TubesMovement:
                animator.Play("P1TubesMovement");
                break;
            case TutorialBoxType.CatFace:
                animator.Play("CatFace");
                break;
            case TutorialBoxType.HamsterFace:
                animator.Play("HamsterFace");
                break;
        }        
    }
    public void P2ChangeAnimation(TutorialBoxType type)
    {
        if (animator == null) return;
        switch (type)
        {
            case TutorialBoxType.None:
                animator.Play("Happy");
                break;
            case TutorialBoxType.Movement:
                animator.Play("P2MoveKeys");
                break;
            case TutorialBoxType.Jump:
                animator.Play("P2Jump");
                break;
            case TutorialBoxType.Interact:
                animator.Play("P2Interact");
                break;
            case TutorialBoxType.Special:
                animator.Play("P2Special");
                break;
            case TutorialBoxType.Punch:
                animator.Play("P2Punch");
                break;
            case TutorialBoxType.DoubleJump:
                animator.Play("P2DoubleJump");
                break;
            case TutorialBoxType.TubesMovement:
                animator.Play("P2TubesMovement");
                break;
            case TutorialBoxType.CatFace:
                animator.Play("CatFace");
                break;
            case TutorialBoxType.HamsterFace:
                animator.Play("HamsterFace");
                break;
        }        
    }
}
