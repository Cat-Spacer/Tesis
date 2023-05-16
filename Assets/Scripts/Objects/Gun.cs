using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class Gun : MonoBehaviour
    {
        Animator anim;
        [Header("Stats")]
        public int damage = 10;
        public float fireRate = 1.0f, fireTimer = 1.0f, bulletLifeTime = 1.0f, wait = 1.0f;
        public string shootSound = "gun_shoot";

        [Header("Objects")]
        public Transform firePoint;
        public AudioSource audioSource;
        public ParticleSystem muzzleFlash;
        public Bullet bulletPrefab;
        [SerializeField] ParticleSystem _spitleParticle;
        [SerializeField] ParticleSystem _spitleExplotionParticle;

        public float distance = 150f;

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
            anim = GetComponent<Animator>();
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
                _spitleParticle.Stop();
                StartCoroutine(WaitForAnim());
            }
        }

        public virtual void FireBullet()
        {
            _spitleExplotionParticle.Play();
            _spitleParticle.Stop();
            Shoot.Fire(bulletPrefab, firePoint, gameObject);
            #region ObjectFactory (bugeado)
            /*
            ObjectToSpawn bullet = ObjectFactory.Instance.pool.GetObject();
            //Debug.Log($"{bullet.name} instantiated from factory");
            //SoundManager.instance.Play();
            if (audioSource != null)
                 audioSource.Play();
             if (muzzleFlash != null)
                 muzzleFlash.Play();
            //Shoot.Fire(bullet.gameObject, firePoint);//cambiar por metodo
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;*/
            #endregion
        }

        IEnumerator WaitForAnim()
        {
            yield return new WaitForSeconds(wait);
            FireBullet();
            _spitleParticle.Play();
        }
    }
}