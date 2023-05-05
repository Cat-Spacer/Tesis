using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PriorityQueue
{
    private Dictionary<NodePoint, float> _allNodes = new Dictionary<NodePoint, float>();

    public void Put(NodePoint key, float value)
    {
        if (_allNodes.ContainsKey(key)) _allNodes[key] = value;
        else _allNodes.Add(key, value);
    }
    public NodePoint Get()
    {
        if (Count() == 0) return null;

        NodePoint n = null;
        foreach (var item in _allNodes)
        {
            if (n == null) n = item.Key;
            if (item.Value < _allNodes[n]) n = item.Key;
        }

        _allNodes.Remove(n);

        return n;
    }


    public int Count()
    {
        return _allNodes.Count;
    }
}