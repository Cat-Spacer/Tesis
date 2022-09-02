using System.Collections.Generic;
using UnityEngine;

public enum AgentStates
{
    Enhancers,
    Enemies
}

public class Spawner : MonoBehaviour
{
    public ObjectForSpawner[] prefab;
    public List<GameObject> spawnPoint = new List<GameObject>();
    public Vector3[] scale;

    private IState _currentState;
    private Dictionary<AgentStates, IState> _allStates = new Dictionary<AgentStates, IState>();

    private void Start()
    {
        if (prefab == null) return;

        for (int i = 0; i < prefab.Length; i++)
        {
            var objects = Instantiate(prefab[i])
                .SetPosition(spawnPoint[i].transform.position.x, spawnPoint[i].transform.position.y, spawnPoint[i].transform.position.z)
                    .SetScale(scale[i].x, scale[i].y, scale[i].z);
        }
    }

    public void Update()
    {
        if (_currentState != null) _currentState.OnUpdate();
    }

    public void InstantiatePrefabs(int index, int scaleIndex)
    {
        var objects = Instantiate(prefab[index])
            .SetPosition(spawnPoint[spawnPoint.Count].transform.position.x, spawnPoint[spawnPoint.Count].transform.position.y, spawnPoint[spawnPoint.Count].transform.position.z)
                .SetScale(scale[scaleIndex].x, scale[scaleIndex].y, scale[scaleIndex].z);
    }

    public void AddState(AgentStates key, IState state)
    {
        if (_allStates.ContainsKey(key)) return;

        _allStates.Add(key, state);
    }

    public void ChangeState(AgentStates key)
    {
        if (!_allStates.ContainsKey(key)) return;
        _currentState?.OnExit();
        _currentState = _allStates[key];
        _currentState.OnEnter();
    }
}
