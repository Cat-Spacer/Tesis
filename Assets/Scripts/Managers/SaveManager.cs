using UnityEngine;
using System;

[Serializable]
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance = default;
    public JsonSaves JsonSaves = new ();
    
    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(this);
        JsonSaves ??= new();
        JsonSaves.OnAwake();
    }
}