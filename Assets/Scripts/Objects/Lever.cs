using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Lever : MonoBehaviour, IInteract
{
    [SerializeField] private CharacterType _type;
    private Animator _anim;
    [SerializeField] private List<GameObject> _connectionObj;
    private List<IActivate> _connection = new List<IActivate>();
    [SerializeField] private bool _activated = false;
    [SerializeField] ParticleSystem[] _greenParticles;
    [SerializeField] ParticleSystem[] _loopGreenParticles;
    [SerializeField] ParticleSystem[] _redParticles;
    private string _activateAnimation;
    private string _desactivateAnimation;
    [SerializeField] InteractionColorsEnum _interactionColors;
    [SerializeField]private List<InteractionColorParticle> _colorIDParticle;

    [System.Serializable]
    public class InteractionColorParticle
    {
        public InteractionColorsEnum colorID;
        public ParticleSystem particleSystem;
    }
 
    void Start()
    {
        _anim = GetComponent<Animator>();
        foreach (var connection in _connectionObj)
        {
            var obj = connection.GetComponent<IActivate>();
            if (obj != null) _connection.Add(obj);
        } 
        SetColor();
        if (_activated) _anim.Play(_activateAnimation);
        else _anim.Play(_desactivateAnimation);
    }
    private void OnEnable()
    {
        if (_anim == null) return;
        SetColor();
        if (_activated) _anim.Play(_activateAnimation);
        else _anim.Play(_desactivateAnimation);
    }

    private void FilterParticles(InteractionColorsEnum colorId)
    {
        if (_colorIDParticle == null)
        { return; }

        var particleType = _colorIDParticle
            .Where(p => p.colorID == colorId)
            .Select(x=> x.particleSystem);

        foreach (var particle in particleType)
        {
            if (particle != null)  
                particle.playOnAwake = true;
            
        }
    }
    void SetColor()
    {
        switch (_interactionColors)
        {
            case InteractionColorsEnum.Orange:
                _activateAnimation = "ActivateOrange";
                _desactivateAnimation = "DeactivateOrange";
                FilterParticles(_interactionColors);
                break;
            case InteractionColorsEnum.Yellow:
                _activateAnimation = "ActivateYellow";
                _desactivateAnimation = "DeactivateYellow";
                FilterParticles(_interactionColors);
                break;
            case InteractionColorsEnum.BlackCyan:
                _activateAnimation = "ActivateBlackCyan";
                _desactivateAnimation = "DeactivateBlackCyan";
                FilterParticles(_interactionColors);
                break;
            case InteractionColorsEnum.Pink:
                _activateAnimation = "ActivatePink";
                _desactivateAnimation = "DeactivatePink";
                FilterParticles(_interactionColors);
                break;
            case InteractionColorsEnum.Grey:
                _activateAnimation = "ActivateGrey";
                _desactivateAnimation = "DeactivateGrey";
                FilterParticles(_interactionColors);
                break;
        }
    }


    void PressLever()
    {
        if (!_activated)
        {
            foreach (var connection in _connection)
            {
                if(connection != null) connection.Activate();
            }
            foreach (var item in _greenParticles)
            {
                item.Play();
            }
            foreach (var item in _loopGreenParticles)
            {
                item.Play();
            }
            _activated = true;
            _anim.Play(_activateAnimation);
        }
        else
        {
            foreach (var item in _loopGreenParticles)
            {
                item.Stop();
            }
            foreach (var connection in _connection)
            {
                if(connection != null) connection.Desactivate();
            }
            _activated = false;
            foreach (var item in _redParticles)
            {
                item.Play();
            }
            _anim.Play(_desactivateAnimation);
        }
        SoundManager.instance.Play(SoundsTypes.Lever, gameObject);
    }

    public void Interact(params object[] param)
    {
        PressLever();
    }

    public void ShowInteract(bool showInteractState)
    {

    }

}
