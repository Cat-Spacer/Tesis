using System.Collections.Generic;
using System;

public class ObjectPool<T>
{
    public delegate T FactoryMethod();
    private FactoryMethod _factory = default;

    private Action<T> _turnOff = default, _turnOn = default;

    private List<T> _stock = new();
    private bool _dynamic = true;

    public ObjectPool(FactoryMethod factory, Action<T> TurnOff, Action<T> TurnOn, int initialCount = 5, bool dynamic = true)
    {
        _factory = factory;
        _turnOff = TurnOff;
        _turnOn = TurnOn;
        _dynamic = dynamic;

        for (int i = 0; i < initialCount; i++)
        {
            var obj = _factory();

            _turnOff(obj);

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

        if(obj != null) _turnOn(obj);

        return obj;
    }
    /// <summary>
    /// Return to pool stock list
    /// </summary>
    /// <param name="obj"></param>
    public void ReturnObject(T obj)
    {
        _turnOff(obj);

        _stock.Add(obj);
    }
}