using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    private Stack<IScreen> _stack = default;

    public string lastResult = default;

    static public ScreenManager instance = default;

    [SerializeField] private GameObject[] _objToActivateDesactivate = default;
    
    void Awake()
    {
        instance = this;

        _stack = new Stack<IScreen>();
    }

    public void Pop(bool pause = true)
    {
        if (_stack.Count <= 1 && pause) return;

        lastResult =_stack.Pop().Free();

        if (_stack.Count > 0)
        {
            _stack.Peek().Activate();
        }

        foreach (var obj in _objToActivateDesactivate)
        {
            obj.SetActive(true);
        }
    }

    public void Push(IScreen screen)
    {
        if (_stack.Count > 0)
        {
            _stack.Peek().Deactivate();
        }

        _stack.Push(screen);

        screen.Activate();
    }

    public void PushInstance(string resource)
    {
        var go = Instantiate(Resources.Load<GameObject>(resource));
        Push(go.GetComponent<IScreen>());

        foreach (var obj in _objToActivateDesactivate)
        {
            obj.SetActive(false);
        }
    }  
}