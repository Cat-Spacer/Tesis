using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrap
{
    void Trap(bool trapState, GameObject enemy);
    void Liberate();
}
