using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsDetection : MonoBehaviour
{
    [SerializeField]CustomMovement _cat;
    [SerializeField] Rigidbody2D _catRb;

    [SerializeField] LayerMask _playerLayer;
   float _UppderDetectionDistance = 1.0f;
     float _LowerDetectionDistance = 1.0f;
    [SerializeField] Vector2 _boxSize = new Vector2(1, 1.0f);
    [SerializeField] BoxCollider2D _collider;
    [SerializeField] Vector2 _offSiteUpperBox = new Vector2(0, 0);
    [SerializeField] Vector2 _offSiteLowerBox = new Vector2(0, 0);

    private void Awake()
    {
        _collider = gameObject.GetComponent<BoxCollider2D>();
    }

    //private bool _isUnder = false;
    private void FixedUpdate()
    {
        // Realizar BoxCast hacia arriba
        RaycastHit2D hitUp = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y)+ _offSiteUpperBox, _boxSize, 0f, Vector2.up, _UppderDetectionDistance, _playerLayer);

        // Realizar BoxCast hacia abajo
        RaycastHit2D hitDown = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y)+ _offSiteLowerBox, _boxSize, 0f, Vector2.down, _LowerDetectionDistance, _playerLayer);

        // Verificar si el jugador fue detectado en el BoxCast superior
        if (hitUp.collider != null && hitDown.collider == null)
        {
            Debug.Log("Jugador detectado arriba");
            _collider.isTrigger = false;
        }

        // Verificar si el jugador fue detectado en el BoxCast inferior
        if (hitDown.collider != null)
        {
            Debug.Log("Jugador detectado abajo");
            _collider.isTrigger = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Dibujar el cuadro del BoxCast hacia arriba
        Gizmos.DrawWireCube(new Vector2(transform.position.x , transform.position.y)+ _offSiteUpperBox + Vector2.up * _UppderDetectionDistance * 0.5f, new Vector2(_boxSize.x, _boxSize.y));

        Gizmos.color = Color.blue;

        // Dibujar el cuadro del BoxCast hacia abajo
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y) + _offSiteLowerBox + Vector2.down * _LowerDetectionDistance * 0.5f, new Vector2(_boxSize.x, _boxSize.y));
    }

}
/*
private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(_catRb.velocity.y<0)
        if(collision.gameObject.layer ==31 && !_cat.insidePlatform && _catRb.velocity.y < 0)
        {
            Debug.Log("wey");
            collision.isTrigger = false;

            foreach (var item in _cat.platformsColliders)
            {
                item.isTrigger = false;
            }
        }

    }
}*/
