using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    private Stack<IScreen> _stack = default;

    public string lastResult = default;

    static public ScreenManager Instance = default;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this);


        _stack = new Stack<IScreen>();
    }

    public void Pop(bool pause = true)
    {
        if (_stack.Count <= 1 && pause) return;

        lastResult = _stack.Pop().Free();

        if (_stack.Count > 0) _stack.Peek().Activate();
    }

    public void Push(IScreen screen)
    {
        if (_stack.Count > 0) _stack.Peek().Deactivate();

        _stack.Push(screen);

        screen.Activate();
    }

    public void PushInstance(string resource)
    {
        var go = Instantiate(Resources.Load<GameObject>(resource));
        Push(go.GetComponent<IScreen>());
    }
}