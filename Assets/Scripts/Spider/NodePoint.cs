using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodePoint : MonoBehaviour
{

    public List<NodePoint> neighbours;

    public int cost = 1;
    /*public bool blocked;
    
    void SetBlocked(bool b)
    {
        blocked = b;
        //Color color = b ? Color.black : Color.white;
        //GameManager.instance.ChangeGameObjectColor(gameObject, color);
    }

   

    private void OnMouseOver()
    {
        if (Input.GetKey(KeyCode.UpArrow)) ChangeCost(cost + 1);
        if (Input.GetKey(KeyCode.DownArrow)) ChangeCost(cost - 1);
        if (Input.GetKey(KeyCode.R)) ChangeCost(1);
    }


    void ChangeCost(int c)
    {
        if (c < 1) c = 1;
        cost = c;

       // ChangeTextCost();
    }

    void ChangeTextCost()
    {
        textCost.enabled = cost > 1;
        textCost.text = cost.ToString();
    }

    void SetBlocked(bool b)
    {
        blocked = b;
        Color color = b ? Color.black : Color.white;
        //GameManager.instance.ChangeGameObjectColor(gameObject, color);
    }*/
}