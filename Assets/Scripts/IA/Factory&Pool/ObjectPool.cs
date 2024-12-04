using System.Collections.Generic;
using System;

public class ObjectPool<T>
{
    public delegate T FactoryMethod();
    private FactoryMethod _factory = default;

    private Action<T, bool> _turnOnOff = default;

    private List<T> _stock = new();
    private bool _dynamic = true;

    public List<T> GetStock {  get { return _stock; } }

    public ObjectPool(FactoryMethod factory, Action<T, bool> TurnOnOff, int initialCount = 5, bool dynamic = true)
    {
        _factory = factory;
        _turnOnOff = TurnOnOff;
        _dynamic = dynamic;

        for (int i = 0; i < initialCount; i++)
        {
            var obj = _factory();

            _turnOnOff(obj, false);

            _stock.Add(obj);
        }

        _dynamic = dynamic;
    }
    /// <summary>
    /// Get object from the pool
    /// </summary>
    /// <returns></returns>
    public T GetObject()
    {
        T obj = default;

        if (_stock.Count > 0)
        {
            obj = _stock[0];

            _stock.RemoveAt(0);
        }
        else if(_dynamic)
        {
            obj = _factory();
        }

        if(obj != null) _turnOnOff(obj, true);

        return obj;
    }
    /// <summary>
    /// Return to pool stock list
    /// </summary>
    /// <param name="obj"></param>
    public void ReturnObject(T obj)
    {
        if(obj == null) return;
        _turnOnOff(obj, false);

        _stock.Add(obj);
    }
}