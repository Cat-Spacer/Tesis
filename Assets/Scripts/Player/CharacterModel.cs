using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleType
{
    Land,
    Jump,
    Run,
    Hit,
    Special,
    Die,
    Revive,
    Damage,
    Grow,
    Shrink,
    Tube,
    InTube,
    Attack,
    Error,
    ExtraScore
}
public class CharacterModel : MonoBehaviour
{
    public Animator _anim;
    private CharacterData _data;
    private string _currentState;
    public SpriteRenderer spRenderer;
    private Material _mat;
    List<ParticleSystem> _particles = new List<ParticleSystem>();
    [SerializeField] private ParticleSystem _landParticle;
    [SerializeField] private ParticleSystem _jumpParticle;
    [SerializeField] private ParticleSystem _runParticle;
    [SerializeField] private ParticleSystem _hitParticle;
    [SerializeField] private ParticleSystem _specialParticle;
    [SerializeField] private ParticleSystem _dieParticle;
    [SerializeField] private ParticleSystem _reviveParticle;
    [SerializeField] private ParticleSystem _bloodParticle;
    [SerializeField] private ParticleSystem _growParticle;
    [SerializeField] private ParticleSystem _shrinkParticle;
    [SerializeField] private ParticleSystem _tubeParticle;
    [SerializeField] private ParticleSystem _inTubeParticle;
    [SerializeField] private ParticleSystem[] _attackParticle;
    [SerializeField] private ParticleSystem[] _errorParticle;
    [SerializeField] private ParticleSystem _extraScoreParticle;
    [Header("Imgs")]
    public GameObject stunIcon;

    [SerializeField] private GameObject canvas;

    private void Start()
    {
        _data = GetComponent<CharacterData>();
        _mat = spRenderer.material;
        _particles.Add(_landParticle);
        _particles.Add(_jumpParticle);
        _particles.Add(_runParticle);
        _particles.Add(_hitParticle);
        _particles.Add(_specialParticle);
        _particles.Add(_dieParticle);
        _particles.Add(_reviveParticle);
        _particles.Add(_bloodParticle);
        _particles.Add(_growParticle);
        _particles.Add(_shrinkParticle);
    }

    private void OnEnable()
    {
        if (EventManager.Instance == null) return;
        EventManager.Instance.Subscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Subscribe(EventType.OnLoseGame, OnFinishGame);
    }
    private void OnFinishGame(object[] obj)
    {
        Debug.Log("StopCanvas");
        canvas.SetActive(false);
    }

    public void PlayParticle(ParticleType type)
    {
        switch (type)
        {
            case ParticleType.Land:
                _landParticle.Play();
                break;
            case ParticleType.Jump:
                _jumpParticle.Play();
                break;
            case ParticleType.Run:
                if(!_runParticle.isPlaying) _runParticle.Play();
                break;
            case ParticleType.Hit:
                if(!_hitParticle.isPlaying) _hitParticle.Play();
                break;
            case ParticleType.Special:
                if(!_specialParticle.isPlaying) _specialParticle.Play();
                break;
            case ParticleType.Die:
                if(!_dieParticle.isPlaying) _dieParticle.Play();
                break;
            case ParticleType.Revive:
                if(!_reviveParticle.isPlaying) _reviveParticle.Play();
                break;
            case ParticleType.Damage:
                _bloodParticle.Play();
                break;
            case ParticleType.Grow:
                _growParticle.Play();
                break;
            case ParticleType.Shrink:
                _shrinkParticle.Play();
                break;
            case ParticleType.Tube:
                _tubeParticle.Play();
                break;
            case ParticleType.InTube:
                _inTubeParticle.Play();
                break;
            case ParticleType.Attack:
                if (_attackParticle != null)
                { foreach (var item in _attackParticle)
                    { item.Play(); }
                } 
                break;
            case ParticleType.Error:
                if (_errorParticle != null)
                {
                    foreach (var item in _errorParticle)
                    { item.Play(); }
                }
                break;
            case ParticleType.ExtraScore:
                if (_extraScoreParticle != null)
                    _extraScoreParticle.Play();
                break;
        }
    }
    public void StopParticle(ParticleType type)
    {
        switch (type)
        {
            case ParticleType.Land:
                _landParticle.Stop();
                break;
            case ParticleType.Jump:
                _jumpParticle.Stop();
                break;
            case ParticleType.Run:
                _runParticle.Stop();
                break;
            case ParticleType.Hit:
                _hitParticle.Stop();
                break;
            case ParticleType.Special:
                _specialParticle.Stop();
                break;
            case ParticleType.Die:
                if(!_dieParticle.isPlaying) _dieParticle.Stop();
                break;
            case ParticleType.Revive:
                if(!_reviveParticle.isPlaying) _reviveParticle.Stop();
                break;
            case ParticleType.InTube:
                _inTubeParticle.Stop();
                break;
        }
    }
    public void StopAllParticles()
    {
        foreach (var particle in _particles)
        {
            particle.Stop();
        }
    }


    public void ChangeAnimationState(string newState)
    {
        if (_currentState == newState) return;
        _anim.Play(newState);
        
        _currentState = newState;
    }
    public void FaceDirection(int direction)
    {
        //if (_data.faceDirection == direction) return;
        _data.faceDirection = direction;
        if (_data.faceDirection > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (_data.faceDirection < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }
    public void Teletransport()
    {
        _mat.SetInteger("Tp_Bool", 1);
    }

    public void GetSmash()
    {
        ChangeAnimationState("GetSmash");
    }
    public void GetStun(bool isStun)
    {
        stunIcon.SetActive(isStun);
    }
    public float GetFaceDirection()
    {
        return _data.faceDirection;
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Unsubscribe(EventType.OnLoseGame, OnFinishGame);
    }
}
