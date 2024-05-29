using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class Gun : MonoBehaviour
    {
        private Animator _anim = default;
        [Header("Stats")]
        public int damage = 10;
        public float fireRate = 1.0f, fireTimer = 1.0f, bulletLifeTime = 1.0f, wait = 1.0f, distance = 150f;
        public string shootSound = "gun_shoot", animState = "OwlPreShoot";

        [Header("Objects")]
        public Transform firePoint = default;
        public AudioSource audioSource = default;
        public ParticleSystem muzzleFlash = default;
        [SerializeField] ParticleSystem _explotionParticle1 = default, _explotionParticle2 = default;

        private string _currentState = default;


        private void Awake()
        {
            #region Sound Setting
            /*if (GetComponent<AudioSource>() != null)
            {
                audioSource = GetComponent<AudioSource>();
                //  if (FindObjectOfType<SoundManager>() != null)
                // audioSource.clip = FindObjectOfType<SoundManager>().GetSound(shootSound).clip;
            }*/
            #endregion
            fireTimer = fireRate;
        }

        private void Start()
        {
            _anim = GetComponentInChildren<Animator>();
        }

        protected virtual void Update()
        {
            FireCooldown();
        }

        public virtual void FireCooldown()
        {
            /* if (Time.time >= fireRate)
            {
                FireBullet();
                 fireRate = Time.time + 1.0f / fireRate;
            }*/

            if (fireTimer > 0)
            {
                fireTimer -= Time.deltaTime;               
            }
            else
            {
                fireTimer = fireRate;
                StartCoroutine(WaitForAnim());
                ChangeAnimationState(animState);
            }
        }

        public virtual void FireBullet()
        {
            Bullet bullet = BulletManager.instance.objectPool.GetObject();
            if (bullet!) return;
            _explotionParticle1.Play();
            _explotionParticle2.Play();
            Shoot.Fire(bullet, firePoint, gameObject);
            _currentState = "";
        }

        IEnumerator WaitForAnim()
        {
            yield return new WaitForSeconds(wait);
            FireBullet();
        }
        public void ChangeAnimationState(string newState)
        {
            if (_currentState == newState) return;
            _anim.Play(newState);

            _currentState = newState;
        }
    }
}