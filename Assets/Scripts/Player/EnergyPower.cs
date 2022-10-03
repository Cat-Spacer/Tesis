using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyPower : MonoBehaviour
{
    [SerializeField] private Slider _energyBar;
    [SerializeField] private float _maxEnergy = 100f, _energyReg = 1f/*, _energyRegLimit = 5f*/
                                    , _currentEnergy = 0f, _time = 0f,_timer = 2.5f;
    private bool _canReg = true;

    private void Start()
    {
        SetStats();
    }

    private void Update()
    {
        UpdateEnergyBar();
        if (!_canReg && _time < _timer)
        {
            _time += Time.deltaTime;
        }
        else
        {
            _canReg = true;
            _time = 0f;
        }
    }

    public void EnergyRegen()
    {
        if (_currentEnergy < _maxEnergy && _canReg)
            _currentEnergy += (_energyReg * Time.deltaTime);
    }

    public bool EnergyDrain(float cuant = 10.0f) // retorna bool para que saber si puedo hacer esa accion o no (ejecuta en el proceso si es que puede).
    {
        if (_currentEnergy - cuant >= 0)
        {
            _currentEnergy -= cuant;
            _time = 0f;
            _canReg = false;
            return true;
        }else
            return false;
    }

    private void UpdateEnergyBar()
    {
        _energyBar.value = _currentEnergy;
    }

    private void SetStats()
    {
        _currentEnergy = _maxEnergy;
        _energyBar.maxValue = _maxEnergy;
        _energyBar.value = _energyBar.maxValue;
    }
    /*
    public void UpdateStats(enum type, float newValue)
    {

    }*/
}
