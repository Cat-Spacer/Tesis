using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ObjectToSpawn
{
    [Header("Stats")]
    [Header("Has ObjectToSpawn on it")]
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] ParticleSystem particles;
    [SerializeField] ParticleSystem particlesExplotion;
    [SerializeField] GameObject _myFahter;
    void Update()
    {
        NormalMovement();
    }

    private void NormalMovement()
    {
        transform.position += transform.right * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != _myFahter)
        {
            var obj = collision.gameObject.GetComponent<IDamageable>();
            if (obj != null) obj.GetDamage();

            //Debug.LogWarning($"{gameObject.name} collided with {collision.gameObject.name}");

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
            Destroy(gameObject);
        }       
        //ObjectFactory.Instance.ReturnObject(this);
    }
    public Bullet SetBullet(GameObject father)
    {
        _myFahter = father;
        return default;
    }
}
