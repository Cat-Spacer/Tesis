using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HamsterChekpoint : MonoBehaviour
{
    [SerializeField] private Vector2 _boxArea;
    [SerializeField] private LayerMask _players;
    private bool _hamsterIsOn = false;
    [SerializeField] ParticleSystem _hamsterParticle;
    [SerializeField] ParticleSystem[] _checkParticles;
    [SerializeField] private Animation _hamsterFlagAnimation;
    [SerializeField] Animator _hamsterFlagAnimator;

    void Update()
    {
        var coll = Physics2D.OverlapBox(transform.position, _boxArea, 0, _players);
        if (!coll) return;

        var player = coll.gameObject.GetComponent<PlayerCharacter>();
        if (!player.gameObject) return;
        if (player.GetCharType() == CharacterType.Hamster)
        {
            if (_hamsterIsOn) return;
            _hamsterIsOn = true;
            GameManager.Instance.SetHamsterRespawnPoint(transform.position);
            foreach (var item in _checkParticles)
            {
                item.Play();
            }
            _hamsterFlagAnimation.Play();
            _hamsterParticle.Play();
            SoundManager.instance.Play(SoundsTypes.Checpoint, gameObject);
            GetComponent<AudioSource>().playOnAwake = false;
        }
    }

    private IEnumerator CleanAudioSources()
    {
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        AudioSource[] sources = GetComponents<AudioSource>();
        foreach (AudioSource s in sources) Destroy(s);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, _boxArea);
    }

}