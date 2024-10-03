using UnityEngine;

public abstract class Factory<T> where T : MonoBehaviour
{
    public T prefab = default;
    public Transform parent = default;

    public virtual T GetObj() { return Object.Instantiate(prefab, parent); }
}