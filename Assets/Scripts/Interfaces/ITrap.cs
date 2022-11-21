using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrap
{
    void Trap(bool trapState, float life, GameObject enemy);
    void Liberate();
}
