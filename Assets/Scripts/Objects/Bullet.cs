using System.Collections;
using UnityEngine;
/// <summary>
/// Has ObjectToSpawn on it
/// </summary>
public class Bullet : ObjectToSpawn
{
    [Header("Has ObjectToSpawn on it")]
    [Header("Stats")]
    [SerializeField] private float _speed = 1.0f, _lifeTime = 3.0f;
    [SerializeField] ParticleSystem particles = default;
    [SerializeField] ParticleSystem particlesExplotion = default;
    [SerializeField] GameObject _myFather = default;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != _myFather)
        {
            var obj = collision.gameObject.GetComponent<IDamageable>();
            if (obj != null) obj.GetDamage();

            //Debug.LogWarning($"{gameObject.name} collided with {collision.gameObject.name}");
            if (particles && particlesExplotion)
            {
                if (transform.rotation.y == 0)
                {
                    var particleLeft = Instantiate(particles);
                    particleLeft.transform.rotation = Quaternion.Euler(0, 180, 0);
                    particleLeft.transform.position = gameObject.transform.position;
                }
                else
                {
                    var particleRight = Instantiate(particles);
                    particleRight.transform.position = gameObject.transform.position;
                }
                var particle = Instantiate(particlesExplotion);
                particle.transform.position = gameObject.transform.position;
            }

            //Destroy(gameObject);
            objectPool.ReturnObject(this);
            Reset();
        }
    }

    public override void Reset()
    {
        StopAllCoroutines();
        StartCoroutine(ReturnToPool());
    }

    public Bullet SetBullet(GameObject father)
    {
        _myFather = father;
        return this;
    }
}