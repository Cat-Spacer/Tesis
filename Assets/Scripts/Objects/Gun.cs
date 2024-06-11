using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Weapons
{
    public class Gun : NetworkBehaviour
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

        private string _currentState = default;


        private void Awake()
        {
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
            Shoot.Fire(bullet, firePoint, gameObject);
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