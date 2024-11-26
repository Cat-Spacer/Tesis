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
    Revive
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
    [Header("Imgs")]
    public GameObject stunIcon;

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
        }
    }
    public void StopAllParticles()
    {
        foreach (var particle in _particles)
        {
            particle.Stop();
        }
    }
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
}
