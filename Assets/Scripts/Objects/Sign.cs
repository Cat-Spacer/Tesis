using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Sign : MonoBehaviour,IInteract
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private SignSprites _sign;
    [FormerlySerializedAs("sings")] [SerializeField] private SignSprites[] signs = new SignSprites[6];
    [SerializeField] private ParticleSystem[] particles;
    private bool _wasDrawed = false;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _sign = signs[Random.Range(0, signs.Length)];
        spriteRenderer.sprite = _sign.signSprite;
    }

    private void Draw()
    {
        foreach (var item in particles)
        {
            item.Play();
        }
        SoundManager.instance.Play(SoundsTypes.Spray, gameObject);
        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<BoxCollider2D>().enabled = false;
        _wasDrawed = true;
        spriteRenderer.sprite = _sign.happySignSprite;
        if(EventManager.Instance) EventManager.Instance.Trigger(EventType.OnGetShield);
    }
    
    public void Interact(params object[] param)
    {
        if (_wasDrawed) return;
        Draw();
    }

    public void ShowInteract(bool showInteractState)
    {

    }
}

[Serializable]
class SignSprites
{
    [SerializeField] public Sprite signSprite;
    [SerializeField] public Sprite happySignSprite;
}