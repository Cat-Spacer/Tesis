using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBot : MonoBehaviour
{
    [SerializeField] private Transform otherBot;
    [SerializeField] private float distanceToOtherBot = 1f, distanceToPlayer = 0.5f, speed = 4f, lerpTime = 0.5f;
    [SerializeField] private Transform target = null;
    private Animator animator = null;
    private Vector3 _velocity = Vector3.zero;
    [SerializeField] private float maxForce = 5f, maxSpeed;
    private HamsterChar _hamster = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
        _hamster = GameManager.Instance.GetHamster().GetComponent<HamsterChar>();
    }

    Vector3 Seek(Vector3 targetPos)
    {
        Vector3 desired = targetPos - transform.position;
        desired.Normalize();
        desired *= speed;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;
    }

    Vector3 Separation()
    {
        Vector3 desired = new Vector3();
        int nearbyBoids = 0;

        Vector3 dist = otherBot.position - transform.position;

        if (dist.magnitude < distanceToOtherBot)
        {
            //desired += dist;
            desired.x += dist.x;
            desired.z += dist.z;
            nearbyBoids++;
        }

        if (nearbyBoids == 0) return desired;
        desired /= nearbyBoids;
        desired.Normalize();
        desired *= maxSpeed;
        desired = -desired;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);
        //steering.y = 0;
        return steering;
    }

    void ApplyForce(Vector3 force)
    {
        _velocity += force;
        _velocity = Vector3.ClampMagnitude(_velocity, speed);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.position) <= distanceToPlayer) return;
        ApplyForce(Seek(target.position));
        if (_hamster.IsInTube && target.GetComponent<HamsterChar>()) transform.position += _velocity * (1.75f * Time.deltaTime);
        else  transform.position += _velocity * Time.deltaTime;
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