using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Sign : MonoBehaviour,IInteract
{
    [SerializeField] SpriteRenderer spriteRenderer;

    private SignSprites sign;
    [FormerlySerializedAs("sings")] [SerializeField] private SignSprites[] signs = new SignSprites[6];
    private bool _wasDrawed = false;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sign = signs[Random.Range(0, signs.Length)];
        spriteRenderer.sprite = sign.signSprite;
    }

    void Draw()
    {
        SoundManager.instance.Play(SoundsTypes.Spray, gameObject);
        GetComponent<BoxCollider2D>().enabled = false;
        _wasDrawed = true;
        spriteRenderer.sprite = sign.happySignSprite;
        if(EventManager.Instance != null) EventManager.Instance.Trigger(EventType.OnGetShield);
    }
    
    public void Interact(params object[] param)
    {
        if (_wasDrawed) return;
        Draw();
    }

    public void ShowInteract(bool showInteractState)
    {

    }
    private void OnValidate()
    {
        // if (signSprite == null || spriteRenderer == null) return;
        // spriteRenderer.sprite = signSprite;
    }
}

[Serializable]
class SignSprites
{
    [SerializeField] public Sprite signSprite;
    [SerializeField] public Sprite happySignSprite;
}