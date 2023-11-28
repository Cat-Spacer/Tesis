using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class SpecialMushroom : MonoBehaviour, IInteract
{
    public SpecialMushroom_SO type;
    private SpriteRenderer _sp;
    public Material outLineMat;
    private Material _defaultMat;
    private Material _mat;
    private BoxCollider2D _collider;
    
    [SerializeField] private bool _isOutline;
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _sp = GetComponent<SpriteRenderer>();
        _defaultMat = _sp.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        PowerUpManager.instance.PowerUp(type);
        GetEated();
    }

    void GetEated()
    {
        _sp.enabled = false;
        _collider.enabled = false;
        StartCoroutine(TimeUntilRespanw());
    }

    void Respawn()
    {
        _sp.enabled = true;
        _collider.enabled = true;
    }

    IEnumerator TimeUntilRespanw()
    {
        yield return new WaitForSeconds(type.time);
        Respawn();
    }
    public void ShowInteract(bool showInteractState)
    {
        // if (!_isOutline || !showInteractState)
        // {
        //     _mat = outLineMat;
        //     _isOutline = true;
        // }
        // else
        // {
        //     _mat = _defaultMat;
        //     _isOutline = false;
        // }
    }
}
