using System.Collections;
using UnityEngine;

public class Bullet : ObjectToSpawn
{
    [Header("Stats")]
    [SerializeField] private GameObject _myFather = default;
    [SerializeField] private ParticleSystem _particles = default, _particlesExplotion = default;
    [SerializeField] private float _speed = 1.0f, _lifeTime = 3.0f;

    private void Start()
    {
        //StartCoroutine(ReturnToPool());
    }

    void Update()
    {
        NormalMovement();
    }

    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(_lifeTime);
        objectPool.ReturnObject(this);
    }

    private void NormalMovement()
    {
        transform.position += transform.right * _speed * Time.deltaTime;
    }

    public Bullet SetSpeed(float newSpeed = 1.0f)
    {
        _speed = newSpeed;
        return this;
    }

    public Bullet SetLifeTime(float newlifeTime = 1.0f)
    {
        _lifeTime = newlifeTime;
        return this;
    }

    public Bullet SetParticles(ParticleSystem newParticles)
    {
        _particles = newParticles;
        return this;
    }
    
    public Bullet SetParticlesExplosion(ParticleSystem newParticlesExplosion)
    {
        _particlesExplotion = newParticlesExplosion;
        return this;
    }

    public Bullet SetFather(GameObject father)
    {
        _myFather = father;
        return this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != _myFather)
        {
            var obj = collision.gameObject.GetComponent<IDamageable>();
            if (obj != null) obj.GetDamage();

            //Debug.LogWarning($"{gameObject.name} collided with {collision.gameObject.name}");
            if (_particles && _particlesExplotion)
            {
                if (transform.rotation.y == 0)
                {
                    var particleLeft = Instantiate(_particles);
                    particleLeft.transform.rotation = Quaternion.Euler(0, 180, 0);
                    particleLeft.transform.position = gameObject.transform.position;
                }
                else
                {
                    var particleRight = Instantiate(_particles);
                    particleRight.transform.position = gameObject.transform.position;
                }
                var particle = Instantiate(_particlesExplotion);
                particle.transform.position = gameObject.transform.position;
            }

            //Destroy(gameObject);
            Reset();
            objectPool.ReturnObject(this);
        }
    }

    public override void Reset()
    {
        StopAllCoroutines();
        StartCoroutine(ReturnToPool());
    }
}