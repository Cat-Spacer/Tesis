using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 _hamsterRespawnPoint;
    private Vector3 _catRespawnPoint;
    
    public void SetCatRespawnPoint(Vector3 pos)
    {
        _catRespawnPoint = pos;
    }
    public void SetHamsterRespawnPoint(Vector3 pos)
    {
        _hamsterRespawnPoint = pos;
    }

    public Vector3 GetCatRespawnPoint()
    {
        return _catRespawnPoint;
    }
    public Vector3 GetHamsterRespawnPoint()
    {
        return _hamsterRespawnPoint;
    }

}