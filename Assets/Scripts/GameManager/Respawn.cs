using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 _currentRespawnPoint;

    public void SetRespawnPoint(Vector3 pos)
    {
        _currentRespawnPoint = pos;
    }

    public Vector3 GetRespawnPoint()
    {
        return _currentRespawnPoint;
    }
}