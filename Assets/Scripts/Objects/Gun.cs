using UnityEngine;

namespace Weapons
{
    public class Gun : MonoBehaviour
    {

        [Header("Stats")]
        public int damage = 10;
        public float fireRate = 1.0f;
        public float fireTimer = 1.0f;
        public float bulletLifeTime = 1.0f;
        public string shootSound = "gun_shoot";

        [Header("Objects")]
        public Transform firePoint;
        public AudioSource audioSource;
        public ParticleSystem muzzleFlash;

        public GameObject _player;
        public float distance = 150f;

        private void Awake()
        {/*
            if (GetComponent<AudioSource>() != null)
            {
                audioSource = GetComponent<AudioSource>();
                //  if (FindObjectOfType<SoundManager>() != null)
                // audioSource.clip = FindObjectOfType<SoundManager>().GetSound(shootSound).clip;
            }*/
            if (_player == null)
                _player = FindObjectOfType<PlayerDatas>().gameObject;
            else
                Debug.LogWarning($"No player setted on {name}");
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _player.transform.position) < distance)
            {
                FireCooldown();
            }
        }

        public void FireCooldown()
        {
           /* if (Time.time >= fireRate)
           {
               FireBullet();
                fireRate = Time.time + 1.0f / fireRate;
           }*/

            if (Time.time >= fireTimer)
            {
                FireBullet();
                fireTimer = Time.time + 1.0f / fireRate;
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
    }
}