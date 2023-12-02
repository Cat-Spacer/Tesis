using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : Obstacle, IInteract
{
    [SerializeField] private GameObject[] targetA;
    [SerializeField] private GameObject[] targetB;
    [SerializeField] private Crystal targetC;
    [SerializeField] GameObject interactLight;
    private float _currentLife;
    public float dmg;
    bool alreadyShowLight;
    [SerializeField] public Animator anim;

    [SerializeField] bool _isFirst = false, _doOnce = false, canRepeat = false;

    public void Interact()
    {

        // if (_isFirst && !GameManager.Instance.celestialDiamond)
        // {
        //     return;
        // }
        
        if (_doOnce && canRepeat) return;

        Debug.Log("interactuando");
        SoundManager.instance.Play(SoundManager.Types.MagicCat);
        anim.SetTrigger("appear");
        foreach (var item in targetA)
        {
            item.gameObject.SetActive(!item.activeSelf);
        }
        foreach (var item in targetB)
        {
            item.gameObject.SetActive(!item.activeSelf);
        }
        if (targetC != null)
            targetC.CallCrystal(null);

        _doOnce = true;
    }

    public void ShowInteract(bool showInteractState)
    {
        if (showInteractState && !alreadyShowLight)
        {
            interactLight.SetActive(true);
            alreadyShowLight = true;
        }
        else if(!showInteractState)
        {
            Debug.Log("Desactivado");
            interactLight.SetActive(false);
            alreadyShowLight = false;
        }
    }
}

 
