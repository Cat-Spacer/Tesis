using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FlowerManager : MonoBehaviour
{
    [FormerlySerializedAs("image")] [SerializeField] Image[] flowerImage;
    [SerializeField] private int currentFlowers = 0;
    private void Start()
    {
        EventManager.Instance.Subscribe(EventType.OnPutFlower, OnPutFlower);
        flowerImage = LiveCamera.instance.GetFlowerImagesUI();
        foreach (var flower in flowerImage) 
        {
            flower.gameObject.SetActive(true);
            flower.color = Color.gray;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)) EventManager.Instance.Trigger(EventType.OnPutFlower);
    }

    private void OnPutFlower(object[] obj)
    {
        if (currentFlowers >= 3) return;
        currentFlowers++;
        flowerImage[currentFlowers - 1].color = Color.white;
        if (currentFlowers == 3)
        {
            EventManager.Instance.Trigger(EventType.OnOpenDoors);
        }
    }
}
