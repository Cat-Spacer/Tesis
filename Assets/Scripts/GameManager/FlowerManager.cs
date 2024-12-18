using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Linq;

public class FlowerManager : MonoBehaviour
{
    [SerializeField] FlowerUI[] flowerImage;
    [SerializeField] private int currentFlowers = 0;
    private void Start()
    {
        EventManager.Instance.Subscribe(EventType.OnPutFlower, OnPutFlower);
        EventManager.Instance.Subscribe(EventType.OnPutGreenFlower, OnPutGreenFlower);
        EventManager.Instance.Subscribe(EventType.OnPutPurpleFlower, OnPutPurpleFlower);
        EventManager.Instance.Subscribe(EventType.OnPutYellowFlower, OnPutYellowFlower);

        flowerImage = LiveCamera.instance.GetFlowerImagesUI();
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
        CheckCollection();
    }
    private void OnPutPurpleFlower(object[] obj)
    {
        if (currentFlowers >= 3) return;
        currentFlowers++;
        var flower = flowerImage.Where(x => x.Type == FlowerType.Purple).FirstOrDefault();
        flower.ActivateImage();
        Debug.Log("PURPLE");
        CheckCollection();
    }
    private void OnPutYellowFlower(object[] obj)
    {
        if (currentFlowers >= 3) return;
        currentFlowers++;
        var flower = flowerImage.Where(x => x.Type == FlowerType.Yellow).FirstOrDefault();
        flower.ActivateImage();
        Debug.Log("YELLOW");
        CheckCollection();
    }
    private void OnPutGreenFlower(object[] obj)
    {
        if (currentFlowers >= 3) return;
        currentFlowers++;
        var flower = flowerImage.Where(x => x.Type == FlowerType.Green).FirstOrDefault();
        flower.ActivateImage();
        Debug.Log("GREEN");
        CheckCollection();
    }

   void CheckCollection()
    {
        if (currentFlowers == 3)
        {
            EventManager.Instance.Trigger(EventType.OnOpenDoors);
        }
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnPutFlower, OnPutFlower);
        EventManager.Instance.Unsubscribe(EventType.OnPutGreenFlower, OnPutGreenFlower);
        EventManager.Instance.Unsubscribe(EventType.OnPutYellowFlower, OnPutYellowFlower);
        EventManager.Instance.Unsubscribe(EventType.OnPutPurpleFlower, OnPutPurpleFlower);
    }
}
