using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FlowerManager : MonoBehaviour
{
    [SerializeField] Image[] flowerImage;
    [SerializeField] private int currentFlowers = 0;
    private void Start()
    {
        EventManager.Instance.Subscribe(EventType.OnPutFlower, OnPutFlower);
        flowerImage = LiveCamera.instance.GetFlowerImagesUI();
        foreach (var flower in flowerImage) 
        {
            flower.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(Input.GetKeyDown(KeyCode.F)) EventManager.Instance.Trigger(EventType.OnPutFlower);
        }
    }

    private void OnPutFlower(object[] obj)
    {
        if (currentFlowers >= 3) return;
        currentFlowers++;
        flowerImage[currentFlowers - 1].gameObject.SetActive(true);
        if (currentFlowers == 3)
        {
            EventManager.Instance.Trigger(EventType.OnOpenDoors);
        }
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnPutFlower, OnPutFlower);
    }
}
