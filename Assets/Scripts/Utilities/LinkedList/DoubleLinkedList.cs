using System.Collections;
using UnityEngine;

public class DoubleLinkedList<T> : IEnumerable
{
    DoubleListNode<T> _first = default, _last = default;
    int _count = 0;

    public int Count { get { return _count; } }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _count) throw new System.ArgumentOutOfRangeException("Index out");

            DoubleListNode<T> current = _first;

            if (index > _count * 0.5f)
            {
                current = _last;

                for (int i = _count - 1; i > index; current = current.prev, i--);
            }
            else
            {
                for (int i = 0; i < index; current = current.next, i++);
            } 

            return current.value;
        }

        set
        {
            if (index < 0 || index >= _count) throw new System.ArgumentOutOfRangeException("Index out");

            DoubleListNode<T> current = _first;

            if (index > _count * 0.5f)
            {
                current = _last;

                for (int i = _count - 1; i > index; current = current.prev, i--);
            }
            else
            {
                for (int i = 0; i < index; current = current.next, i++);
            }

            current.value = value;
        }
    }

    public void Add(T element)
    {
        DoubleListNode<T> node = new ();

        node.value = element;

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

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _count) throw new System.ArgumentOutOfRangeException("Index Out");


        if(index == 0)
        {
            _first = _first.next;

            if (_first == null) _last = null;
        }
        else
        {
            DoubleListNode<T> current = _first;

            for (int i = 0; i < index - 1; current = current.next, i++);

            current.next = current.next.next;

            if(current.next == null) _last = current;
        }

        _count--;
    }

    public void Remove(T element)
    {
        if (element == null) throw new System.ArgumentNullException();

        DoubleListNode<T> current = _first;

        while (current != null)
        {
            if (current.value.Equals(element))
            {
                if (current == _first)
                {
                    _first = current.next;

                    if (_first != null) _first.prev = null;
                    else _last = null;
                } 
                else if (current == _last)
                {
                    _last = current.prev;
                    _last.next = null;
                }
                else
                {
                    current.prev.next = current.next;
                    current.next.prev = current.prev;
                }

                _count--;
                return;
            }

            current = current.next;
        }
    }

    public bool Contains(T element)
    {
        DoubleListNode<T> current = _first;

        for (int i = 0; i < _count - 1; current = current.next, i++) 
            if (element.Equals(current)) return true;

        return false;
    }

    public IEnumerator GetEnumerator()
    {
        var current = _first;

        while (current != null)
        {
            yield return current.value;
            current = current.next;
        }
    }
}