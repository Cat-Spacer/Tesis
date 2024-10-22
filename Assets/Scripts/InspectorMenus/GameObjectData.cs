using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameObjectData", menuName = "Custom/GameObjectData")]
public class GameObjectData : ScriptableObject
{
    public List<GameObject> myGameObjects = new List<GameObject>(); // Ajusta el tamaño según lo necesites
}

