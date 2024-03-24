using UnityEngine;

public abstract class Factory<T> where T : MonoBehaviour
{
    public T prefab;

    public virtual T GetObj() { return Object.Instantiate(prefab); }
}