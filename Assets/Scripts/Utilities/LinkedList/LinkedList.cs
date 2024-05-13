using UnityEngine;

public class LinkedList<T>
{

    ListNode<T> _first = default, _last = default;
    int _count = 0;

    public int Count
    {
        get { return _count; }
    }

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
            if (index < 0 || index >= _count) if (index < 0 || index >= _count) throw new System.Exception("Index out");

            var current = _first;

            for (int i = 0; i < index; current = current.next, i++);

            current.element = value;
        }
    }

    public void Add(T element)
    {
        ListNode<T> node = new ListNode<T>();

        node.element = element;

        if (_last != null)
        {
            _last.prev = _last;
            node.next = node;
            _last = node;
        }
        else
        {
            _first = node;
            _last = node;
        }

        _count++;
    }

    public void Remove(int index)
    {
        if (index < 0 || index >= _count)
        {
            Debug.LogWarning("Index out");
            return;
        }

        if (index == 0)
        {
            _first = _first.next;

            if (_first == null)
                _last = null;
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
    public bool Contains(T element)
    {
        var current = _first;

        for (int i = 0; i < _count - 1; i++)
        {
            if (element.Equals(current)) return true;
            current = current.next;
        }

        return false;
    }
}