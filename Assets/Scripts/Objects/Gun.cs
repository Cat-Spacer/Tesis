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


        public float distance = 150f;

        private void Awake()
        {/*
            if (GetComponent<AudioSource>() != null)
            {
                audioSource = GetComponent<AudioSource>();
                //  if (FindObjectOfType<SoundManager>() != null)
                // audioSource.clip = FindObjectOfType<SoundManager>().GetSound(shootSound).clip;
            }*/
            fireTimer = fireRate;
        }
        private void Start()
        {
            anim = GetComponent<Animator>();
        }
        private void Update()
        {
            FireCooldown();
        }

        public void FireCooldown()
        {
           /* if (Time.time >= fireRate)
           {
               FireBullet();
                fireRate = Time.time + 1.0f / fireRate;
           }*/

            if (fireTimer > 0)
            {
                fireTimer -= Time.deltaTime;
            }else
            {
                fireTimer = fireRate;
                anim.SetTrigger("Attack");
                StartCoroutine(WaitForAnim());
            }
        }
        public virtual void FireBullet()
        {
            ObjectToSpawn bullet = ObjectFactory.Instance.pool.GetObject();
            //SoundManager.instance.Play();
           /* if (audioSource != null)
                audioSource.Play();
            if (muzzleFlash != null)
                muzzleFlash.Play();*/
            Shoot.Fire(bullet.gameObject, firePoint);
        }
        IEnumerator WaitForAnim()
        {
            yield return new WaitForSeconds(wait);
            FireBullet();
        }
    }
}