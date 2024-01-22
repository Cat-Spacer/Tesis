using System.Collections;
using UnityEngine;

public class DoubleLinkedList<T> : IEnumerable
{
    ListNode<T> _first = default, _last = default;
    int _count = 0;

    public int Count { get { return _count; } }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _count) throw new System.Exception("Index out");

            var current = _first;

            if (index > _count * 0.5f)
            {
                current = _last;

                for (int i = _count - 1; i > index; current = current.prev, i--);
            }
            else
            {
                for (int i = 0; i < index; current = current.next, i++);
            } 

            return current.element;
        }

        set
        {
            if (index < 0 || index >= _count) throw new System.Exception("Index out");

            var current = _first;

            if (index > _count * 0.5f)
            {
                current = _last;

                for (int i = _count - 1; i > index; current = current.prev, i--);
            }
            else
            {
                for (int i = 0; i < index; current = current.next, i++);
            }

            current.element = value;
        }
    }

    public void Add(T element)
    {
        ListNode<T> node = new ListNode<T>();

        node.element = element;

        if(_last != null)
        {
            _last.next = node;
            node.prev = _last;
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

        if(index == 0)
        {
            _first = _first.next;

            if (_first == null) _last = null;
        }
        else
        {
            var current = _first;

            for (int i = 0; i < index - 1; current = current.next, i++);

            current.next = current.next.next;

            if(current.next == null) _last = current;
        }

        _count--;
    }

    public bool Contains(T element)
    {
        var current = _first;

        for (int i = 0; i < _count - 1; current = current.next, i++) 
            if (element.Equals(current)) return true;

        return false;
    }

    public IEnumerator GetEnumerator()
    {
        var current = _first;

        while (current != null)
        {
            yield return current.element;
            current = current.next;
        }
    }
}