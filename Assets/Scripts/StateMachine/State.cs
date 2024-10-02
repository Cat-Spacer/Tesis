using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected CharacterModel model;
    protected Inputs input;
    protected CharacterData data;
    public bool isComplete { get; protected set; }
    protected float startTime;
    public float time => Time.time - startTime;
    
    public virtual void Enter() {}
    public virtual void Do() {}
    public virtual void FixedDo() {}
    public virtual void Exit() {}

    public void SetUp(Rigidbody2D _rb, CharacterModel _model, Inputs _input, CharacterData _data)
    {
        rb = _rb;
        model = _model;
        input = _input;
        data = _data;
    }
}
