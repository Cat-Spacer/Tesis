using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumInventory : MonoBehaviour
{
    public GameObject[] inventory;
    public SpriteRenderer[] sprite;
    private void Start()
    {
        inventory = new GameObject[4];
    }
    public void OnInventoryAdd(GameObject obj, Sprite sp)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = obj;
                UpdateSprites(i, sp);
                return;
            } 
        }
    }
    private void UpdateSprites(int slot, Sprite sp)
    {
        sprite[slot].sprite = sp;
    }
    public GameObject GetObject(int slot)
    {
        return inventory[slot];
    }
}
