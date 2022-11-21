using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animation _animator;
    [SerializeField] private SpriteMask _mask;
    public void ActivateDesactivate(bool active)
    {
        gameObject.SetActive(active);


    }
}
