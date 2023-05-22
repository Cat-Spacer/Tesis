using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MiniGame : MonoBehaviour
{
    [SerializeField] Generator _generator;
    Action _Action = delegate { };
    [SerializeField] GameObject _handle;
    [SerializeField] GameObject miniGameCanva;

    [SerializeField] float speed;
    [SerializeField] int random, current, max;
    bool turnOn;
    [SerializeField] List<GameObject> _OnSquare;
    [SerializeField] List<GameObject> _OffSquare;
    [SerializeField] List<int> _sweetSquares;

    void Start()
    {
        current = 0;
        max = _OffSquare.Count - 1;
        TurnOff();
    }

    void Update()
    {
        _Action();
    }

    IEnumerator MoveRight()
    {
        yield return new WaitForSeconds(speed);
        current += 1;
        _handle.transform.position = _OffSquare[current].transform.position;
        if (current == max)
        {
            current = max;
            StartCoroutine(MoveLeft());
        }
        else StartCoroutine(MoveRight());
    }
    IEnumerator MoveLeft()
    {
        yield return new WaitForSeconds(speed);
        current -= 1;
        _handle.transform.position = _OffSquare[current].transform.position;
        if (current == 0)
        {
            current = 0;
            StartCoroutine(MoveRight());
        }
        else StartCoroutine(MoveLeft());
    }
    public void Check()
    {
        _Action = delegate { };
        if (turnOn)
        {
            StopAllCoroutines();
            if (_sweetSquares.Contains(current))
            {
                _generator.OnWinMiniGame();
            }
            else
            {
                ChangeValues();
                current = 0;
                _handle.transform.position = _OffSquare[current].transform.position;
                StartCoroutine(MoveRight());
            }
        }
    }
    public void TurnOn()
    {
        ChangeValues();

        turnOn = true;
        miniGameCanva.gameObject.SetActive(true);
        StartCoroutine(MoveRight());
    }
    void ChangeValues()
    {
        _sweetSquares.Clear();
        foreach (var square in _OnSquare)
        {
            square.SetActive(false);
        }
        random = (int)UnityEngine.Random.Range(0, max);
        if (random == 0)
        {
            _OnSquare[0].SetActive(true);
            _OnSquare[1].SetActive(true);
            _OnSquare[2].SetActive(true);
            _sweetSquares.Add(0);
            _sweetSquares.Add(1);
            _sweetSquares.Add(2);
        }
        else if (random == max)
        {
            _OnSquare[max].SetActive(true);
            _OnSquare[max - 1].SetActive(true);
            _OnSquare[max - 2].SetActive(true);
            _sweetSquares.Add(max);
            _sweetSquares.Add(max - 1);
            _sweetSquares.Add(max - 2);
        }
        else
        {
            _OnSquare[random + 1].SetActive(true);
            _OnSquare[random].SetActive(true);
            _OnSquare[random - 1].SetActive(true);
            _sweetSquares.Add(random + 1);
            _sweetSquares.Add(random);
            _sweetSquares.Add(random - 1);
        }
    }
    public void TurnOff()
    {
        miniGameCanva.gameObject.SetActive(false);
        foreach (var square in _OnSquare)
        {
            square.SetActive(false);
        }
        turnOn = false;
        _Action = delegate { };
    }
}
