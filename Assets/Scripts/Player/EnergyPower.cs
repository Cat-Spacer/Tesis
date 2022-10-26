using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyPower : MonoBehaviour
{
    [SerializeField] private Slider _energyBar;
    [SerializeField] private Image[] _imgSlider;
    [SerializeField] private Color[] _colorSlider;
    [SerializeField] private float _maxEnergy = 100f, _energyReg = 1f/*, _energyRegLimit = 5f*/
                                    ,_currentEnergy = 0, _time = 0f,_timer = 2.5f, _alphaValue, _fadeSpeed;
    Color alpha;
    private bool _canReg = true;

    public bool hasEnergy = false;

    private void Start()
    {
        SetStats();
        for (int i = 0; i < _imgSlider.Length; i++)
        {
            _colorSlider[i] = _imgSlider[i].color;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.V))
        {
            FadeIn();
        }
        if (Input.GetKey(KeyCode.B))
        {
            FadeOut();
        }
        UpdateEnergyBar();
        if (!_canReg && _time < _timer)
        {
            _time += Time.deltaTime;
        }
        else
        {
            _canReg = true;
            hasEnergy = true;
            _time = 0f;
            EnergyRegen();
            if (_currentEnergy > _maxEnergy)
                _currentEnergy = _maxEnergy;
        }
    }

    public void EnergyRegen()
    {
        if (_currentEnergy < _maxEnergy && _canReg)
            _currentEnergy += (_energyReg * Time.deltaTime);
    }

    public bool EnergyDrain(float cuant = 10.0f) // retorna bool para que saber si puedo hacer esa accion o no (ejecuta en el proceso si es que puede).
    {
        if (_currentEnergy - cuant > 0)
        {
            _currentEnergy -= cuant;
            _time = 0f;
            _canReg = false;
            return true;
        }else
        {
            hasEnergy = false;
             return false;
        }
            
    }

    private void UpdateEnergyBar()
    {       
        _energyBar.value = _currentEnergy;
        if (_currentEnergy == _maxEnergy)
        {
            FadeOut();
        }
        else
        {
            FadeIn();
        }
    }
    public void FadeIn()
    {
        alpha.a = _alphaValue += _fadeSpeed * Time.deltaTime;
        if (_alphaValue > 1)
        {
            _alphaValue = 1;
        }
        for (int i = 0; i < _imgSlider.Length; i++)
        {
            _colorSlider[i].a = alpha.a;
            _imgSlider[i].color = _colorSlider[i];
        }

    }
    public void FadeOut()
    {
        alpha.a = _alphaValue -= _fadeSpeed * Time.deltaTime;
        if (_alphaValue < 0)
        {
            _alphaValue = 0;
        }
        for (int i = 0; i < _imgSlider.Length; i++)
        {
            _colorSlider[i].a = alpha.a;
            _imgSlider[i].color = _colorSlider[i];
        }
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
