using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = default;
    [SerializeField] private List<StaticScreen> _staticScreens = default;

    //public StaticScreen AddValueStaticScreens { set { _staticScreens.Add(value); } }

    private void Awake()
    {
        if (!instance) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public List<StaticScreen> StaticScreens(StaticScreen value = default)
    {
        if(value) _staticScreens.Add(value);
        return _staticScreens;
    }
}