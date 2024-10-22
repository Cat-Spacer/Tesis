using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class Gun : MonoBehaviour
    {
        private Animator _anim = default;
        [Header("Stats")]
        [SerializeField] private float _fireRate = 1.0f;
        [SerializeField] private float _fireTimer = 1.0f, _wait = 1.0f;
        [SerializeField] private string _shootSound = "gun_shoot", _animState = "OwlPreShoot";

        [Header("Objects")]
        [SerializeField] private Transform _firePoint = default;
        [SerializeField] private AudioSource _audioSource = default;
        [SerializeField] private ParticleSystem _muzzleFlash = default;

        [Header("No Pool Manager")]
        [SerializeField] private ObjectToSpawn _objPrefab = default;
        [SerializeField] private int _initialCount = 5;
        [SerializeField] private bool _dynamic = true;
        private OwnFactory _myFactory = default;

        [SerializeField] private string _currentState = default;

        public float FireRate { get { return _fireRate; } set { _fireRate = value; } }

        public void Start()
        {
            if (!_anim) _anim = GetComponentInChildren<Animator>();
            _fireTimer = _fireRate;
            if (!BulletManager.instance)
            {
                _myFactory = new OwnFactory(_objPrefab, transform, _initialCount, _dynamic);
            }
        }

        public virtual void FireCooldown()
        {
            if (_fireTimer > 0)
            {
                _fireTimer -= Time.deltaTime;
            }
            else
            {
                _fireTimer = _fireRate;
                StartCoroutine(WaitForAnim());
                ChangeAnimationState(_animState);
            }
        }

        public virtual void FireBullet()
        {
            Bullet bullet = default;
            if (BulletManager.instance)
            {
                bullet = BulletManager.instance.ObjectPool.GetObject().GetComponent<Bullet>();
                bullet.AddReference(BulletManager.instance.ObjectPool);
            }
            else
            {
                bullet = _myFactory.ObjectPool.GetObject().GetComponent<Bullet>();
                bullet.AddReference(_myFactory.ObjectPool);
            }

            if (!bullet) return;
            Shoot.Fire(bullet, _firePoint, gameObject);
        }

        IEnumerator WaitForAnim()
        {
            yield return new WaitForSeconds(_wait);
            FireBullet();
        }

        public void ChangeAnimationState(string newState)
        {
            if (_currentState == newState) return;
            if (_anim) _anim.Play(newState);

            _currentState = newState;
        }
    }
}