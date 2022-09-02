using UnityEngine;

public abstract class Prototype : MonoBehaviour
{
    public abstract Prototype Clone(float x = 0, float y = 0, float z = 0);
}
