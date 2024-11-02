using System.Collections.Generic;
using UnityEngine;

public class TubeSystem : MonoBehaviour
{
    bool onTube = false;
    public void EnterTubeSystem(bool state)
    {
        onTube = state;
    }
    public bool IsPlayerOnTube(){return onTube;}
}