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
        var obj = collision.gameObject.GetComponent<IDamageable>();
        if (obj != null) obj.GetDamage(10);

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

        //ObjectFactory.Instance.ReturnObject(this);
    }

}
