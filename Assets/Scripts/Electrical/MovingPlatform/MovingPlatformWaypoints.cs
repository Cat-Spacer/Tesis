using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformWaypoints : MonoBehaviour
{
    [SerializeField] List<Transform> _waypoints = new List<Transform>();
    [SerializeField] LineRenderer _lines;

    void Start()
    {
        for (int i = 0; i < _waypoints.Count; i++)
        {
            _lines.SetPosition(i, _waypoints[i].localPosition);
        }
    }
}
