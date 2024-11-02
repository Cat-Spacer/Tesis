using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EgoSystem : MonoBehaviour
{
    public static EgoSystem instance;
    [SerializeField] private int catPoints = 0;
    [SerializeField] private int hamsterPoints = 0;
    
    [SerializeField] private TextMeshProUGUI catText;
    [SerializeField] private TextMeshProUGUI hamsterText;

    private void Awake()
    {
        if(instance == null) instance = this;
    }

    private void Start()
    {
        EventManager.Instance.Subscribe(EventType.OnUpdateEgoPoints, OnUpdatePoints);
    }

    private void OnUpdatePoints(object[] obj)
    {
        var type = (CharacterType) obj[0];
        var amount = (int) obj[1];
        UpdatePoints(type, amount);
    }

    void UpdatePoints(CharacterType type, int amount)
    {
        if (type == CharacterType.Cat)
        {
            if (catPoints + amount > 0) catPoints += amount;
            else catPoints = 0;
            catText.text = catPoints.ToString();
        }
        else
        {
            if(hamsterPoints + amount > 0) hamsterPoints += amount;
            else hamsterPoints = 0;
            hamsterText.text = hamsterPoints.ToString();
        }
    }
    public Tuple<int, int> GetEgoPoints(){return Tuple.Create(catPoints, hamsterPoints);}
}


