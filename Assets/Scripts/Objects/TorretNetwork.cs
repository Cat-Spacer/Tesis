using System;
using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class TorretNetwork : Gun
    {
        Action _StunAction = delegate { };
        float currentStunTime;
        bool stun = false;

        protected override void Update()
        {
            base.Update();
            _StunAction();
        }
    }
}
