using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSpecial : MonoBehaviour
{
   private CatChar _cat;
   private Action _specialAction = delegate {  };

   [Header("SPIT")]
   [SerializeField] private HairBallBullet _hairballBullet;
   [SerializeField] private Vector3 _spitPos;
   [SerializeField] private float spitCd;
   private bool _canShoot;

   private void Start()
   {
      _cat = GetComponent<CatChar>();
   }

   private void Update()
   {

   }

   #region Special Mushrooms

   public void Special()
   {
      _specialAction();
   }
   void SpitSpecial()
   {
      if (!_canShoot) return;
      var bullet = Instantiate(_hairballBullet).Set(gameObject,_cat.GetFaceDirection());
      bullet.transform.position = transform.position + _spitPos;
      _canShoot = false;
      StartCoroutine(SpitCD());
   }

   IEnumerator SpitCD()
   {
      yield return new WaitForSeconds(spitCd);
      _canShoot = true;
   }
   public void SpitMushroom(float time)
   {
      Debug.Log("Spit");
      _canShoot = true;
      _specialAction = SpitSpecial;
      StartCoroutine(SpecialTimmer(time));
   }
   
   void ThrowSpecial()
   {
      
   }
   public void ThrowMushroom(float time)
   {
      _specialAction = ThrowSpecial;
      StartCoroutine(SpecialTimmer(time));
   }
   IEnumerator SpecialTimmer(float time)
   {
      yield return new WaitForSeconds(time);
      _specialAction = delegate {  };
   }
   #endregion
}
