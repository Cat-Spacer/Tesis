using System.Collections.Generic;

public class LookUpTable<T1, T2>
{
    public delegate T2 Ecuation(T1 keyToReturn);

    private Ecuation _factoryMethod = default;

    private Dictionary<T1, T2> _table = new Dictionary<T1, T2>();

    public LookUpTable(Ecuation newFactory)
    {
        _factoryMethod = newFactory;
    }

    public T2 ReturnValue(T1 myKey)
    {
        if (_table.ContainsKey(myKey))
            return _table[myKey];
        else
            return _table[myKey] = _factoryMethod(myKey);
    }
}