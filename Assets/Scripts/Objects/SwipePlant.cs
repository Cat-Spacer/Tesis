using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipePlant : Obstacle
{
    public float _attackSpeed = 1.0f;
    [SerializeField] private Animator _swipeAnimator;

    void Start()
    {
        if (_swipeAnimator == null)
            _swipeAnimator = GetComponent<Animator>();
        if (_swipeAnimator == null)
            Debug.LogWarning($"No animator added to {name}.");
    }

    void Update()
    {
        
    }

    public void SetAnimationSpeed(float newSpeed = 1.0f)
    {
        _swipeAnimator.speed = newSpeed;
    }
}
