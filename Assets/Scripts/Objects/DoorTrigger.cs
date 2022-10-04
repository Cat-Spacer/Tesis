using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : Obstacle, IDamageable
{
    [SerializeField] private GameObject target;
    [SerializeField] private Crystal targetC;
    private float _currentLife;
    public float dmg;

  /*  private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 14)
        {
            Debug.Log("none");
            return;
        }
            

        target.gameObject.SetActive(!target.activeSelf);
       if (targetC!=null) targetC.CallCrystal();
        //Destroy(gameObject);
        
    }*/

  /* private void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.gameObject.GetComponent<IDamageable>();
        if (obj == null) return;
       // obj.GetDamage(dmg);
    }*/
    public void GetDamage(float dmg)
    {
        Debug.Log("interactuanmdo");

        target.gameObject.SetActive(!target.activeSelf);
        if (targetC != null) 
            targetC.CallCrystal();
    }
}
