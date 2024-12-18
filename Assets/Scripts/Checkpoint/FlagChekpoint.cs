using System.Collections;
using UnityEngine;

public class FlagChekpoint : MonoBehaviour
{
    [SerializeField] private LayerMask _players;
    private bool _checkpointIsOn = false;
    [SerializeField] ParticleSystem _catParticle;
    [SerializeField] ParticleSystem _hamsterParticle;
    [SerializeField] ParticleSystem[] _checkParticles;
    [SerializeField] private Animation _catFlagAnimation;
    [SerializeField] private Animation _hamsterFlagAnimation;
    [SerializeField] Animator _catFlagAnimator;
    [SerializeField] Animator _hamsterFlagAnimator;
    //[SerializeField] ParticleSystem _catFace;
  //  [SerializeField] ParticleSystem _hamsterFace;
    [SerializeField] ParticleSystem _catCircle;
    [SerializeField] ParticleSystem _hamsterCircle;
    [SerializeField] ParticleSystem _catCirclePulse;
    [SerializeField] ParticleSystem _hamsterCirclePulse;
    bool _isCat = false;
    bool _isHamster= false;

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        var playerCharacter = trigger.GetComponent<PlayerCharacter>();
        if (playerCharacter == null) return;

        UpdateCharacterState(playerCharacter, true);

        if (_isCat && _isHamster) OnCheckPoint();
    }

    private void UpdateCharacterState(PlayerCharacter character, bool state)
    {
        if (character.GetCharType() == CharacterType.Cat)
        {
            _isCat = state;
            _catCirclePulse.Play();
        }
        if (character.GetCharType() == CharacterType.Hamster) 
        {
            _isHamster = state;
            _hamsterCirclePulse.Play();
        }
    }
  /*  private void OnTriggerExit2D(Collider2D trigger)
    {
        var playerCharacter = trigger.GetComponent<PlayerCharacter>();
        if (playerCharacter == null) return;

        UpdateCharacterState(playerCharacter, false);
    }*/


    private void OnCheckPoint()
    {
            if (_checkpointIsOn) return; // Avoid triggering if the checkpoint is already active

            _checkpointIsOn = true;
        _hamsterCirclePulse.Stop();
        _catCirclePulse.Stop();
        _hamsterCircle.Play();
        _catCircle.Play();
        // Set respawn points for both characters
        GameManager.Instance.SetCatRespawnPoint(transform.position);
            GameManager.Instance.SetHamsterRespawnPoint(transform.position);

            // Play checkpoint particles
            foreach (var particle in _checkParticles)
            {
                particle.Play();
            }

        // Trigger animations using Animator
        _hamsterFlagAnimation.Play();
        _catFlagAnimation.Play();

        // Play the particle effects
        _catParticle.Play();
            _hamsterParticle.Play();

            // Play sound for checkpoint
            SoundManager.instance.Play(SoundsTypes.Checpoint, gameObject);

            // Disable play on awake for the AudioSource
            var audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.playOnAwake = false;
            }
        
    }


    private IEnumerator CleanAudioSources()
    {
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        AudioSource[] sources = GetComponents<AudioSource>();
        foreach (AudioSource s in sources) Destroy(s);
    }

}
