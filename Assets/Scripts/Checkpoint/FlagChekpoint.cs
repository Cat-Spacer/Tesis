using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FlagChekpoint : MonoBehaviour
{
    [SerializeField] private Vector2 _boxArea;    
    [SerializeField] private LayerMask _players;
    private Animator _anim;
    private bool _isOn = false;
    [SerializeField] bool sharedCheckpoint = false;
    [SerializeField] ParticleSystem _particle;
    void Start()
    {
        _anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (_isOn) return;
        var coll = Physics2D.OverlapBox(transform.position, _boxArea, 0, _players);
        if (coll != null)
        {
            if (sharedCheckpoint)
            {
                GameManager.Instance.SetCatRespawnPoint(transform.position);
                GameManager.Instance.SetHamsterRespawnPoint(transform.position);
                _isOn = true;
                _anim.SetTrigger("ON");
                _particle.Play();
            }
            else
            {
                var player = coll.gameObject.GetComponent<PlayerCharacter>();
                if (player.gameObject== null) return;
                if (player.GetCharType() == CharacterType.Cat) GameManager.Instance.SetCatRespawnPoint(transform.position);
                else GameManager.Instance.SetHamsterRespawnPoint(transform.position);
                _isOn = true;
                _anim.SetTrigger("ON");
                _particle.Play();
            }
            SoundManager.instance.Play(SoundsTypes.Checpoint, gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, _boxArea);
    }

}
