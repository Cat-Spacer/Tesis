using System.Collections;

public class LinkedList<T> : IEnumerable
{
    ListNode<T> _first = null, _last = null;

    int _count = 0;

    public int Count { get { return _count; } }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _count) throw new System.Exception("Index out");
            var current = _first;

            for (int i = 0; i < index; current = current.next, i++);

            return current.element;
        }

        set
        {
            if (index < 0 || index >= _count) throw new System.Exception("Index out");

            var current = _first;

            for (int i = 0; i < index; current = current.next, i++);

            current.element = value;
        }
    }

    public void Add(T value)
    {
        var node = new ListNode<T>();
        node.element = value;

        if (_first == null) _first = _last = node;
        else _last.next = _last = node;

        _count++;
    }

    public void Remove(int index)
    {
        if (index < 0 || index >= _count) throw new System.Exception("Index out");

        if (index == 0)
        {
            _first = _first.next;
            if (_first == null) _last = null;
        }
        else
        {
            var current = _first;

            for (int i = 0; i < index - 1; current = current.next, i++);

            current.next = current.next.next;

            if (current.next == null) _last = current;
        }

        _count--;
    }

    public IEnumerator GetEnumerator()
    {
        var node = _first;

        while (node != null)
        {
            yield return node.element;

            node = node.next;
        }
    }
}