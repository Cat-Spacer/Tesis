using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    public static void ResetPosition(this Transform transform)
    {
        transform.position = Vector3.zero;
    }

    public static void ResetStats(this GameObject gameObject)
    {
        //gameObject.GetComponent<Health>().currentHitPoints = gameObject.GetComponent<Health>().maxHitPoints;        
    }

    public static void ShowCursor(this GameObject gameObject)
    {
        if (gameObject.name == "Player")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public static void HideCursor(this GameObject gameObject)
    {
        if (gameObject.name == "Player")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public static void Targeting (this GameObject gameObject, Transform target)
    {
        gameObject.transform.LookAt(target);
    }
}