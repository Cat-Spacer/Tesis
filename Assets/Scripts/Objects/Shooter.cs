using System;
using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class Shooter : Gun
    {
        Action _StunAction = delegate { };
        float currentStunTime;
        bool stun = false;

        protected override void Update()
        {
            base.Update();
            _StunAction();
        }

        public void Stun()
        {
            currentStunTime -= Time.deltaTime;
            if (currentStunTime <= 0)
            {
                stun = false;
                _StunAction = delegate { };
            }
        }

        public override void FireCooldown()
        {
            if (stun)
            {
                Debug.Log("Stuned");
                return;
            }
            base.FireCooldown();
        }
    }
}
