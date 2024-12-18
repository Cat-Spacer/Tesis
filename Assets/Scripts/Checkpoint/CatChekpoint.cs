using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CatChekpoint : MonoBehaviour
{
    [SerializeField] private Vector2 _boxArea;    
    [SerializeField] private LayerMask _players;
    private bool _catIsOn = false;
    [SerializeField] ParticleSystem _catParticle;
    [SerializeField] ParticleSystem[] _checkParticles;
    [SerializeField] private Animation _catFlagAnimation;
    [SerializeField] Animator _catFlagAnimator;
    [SerializeField] ParticleSystem _catCircle;

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        var player = trigger.GetComponent<PlayerCharacter>();
        if (player == null) return;
        if (player.GetCharType() == CharacterType.Cat)
        {
            if (_catIsOn) return;
            _catIsOn = true;
            GameManager.Instance.SetCatRespawnPoint(transform.position);
            foreach (var item in _checkParticles)
            {
                item.Play();
            }
            _catCircle.Play();
            _catFlagAnimation.Play();
            _catParticle.Play();
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
