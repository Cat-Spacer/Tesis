using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{/// <summary>
 /// Setea las teclas para cada accion
 /// </summary>
    //Hacer que dependiendo del voton depende de si es ppal o alternativo
    private Dictionary<string, KeyCode[]> _buttonKeys;
    private void OnEnable()
    {
        _buttonKeys = new Dictionary<string, KeyCode[]>();
        // Agarrar del PlayerImputs y cargar las teclas / reemplazarlas (si hubo cambios).
        _buttonKeys["Jump"] = new KeyCode[2] { KeyCode.Space, KeyCode.LeftControl };
        _buttonKeys["Left"] = new KeyCode[2] { KeyCode.A, KeyCode.LeftArrow };
        _buttonKeys["Right"] = new KeyCode[2] { KeyCode.D, KeyCode.RightArrow };

        foreach (var item in _buttonKeys.ToList())
        {
            foreach (var value in item.Value)
            {
                Debug.Log($"Key = {item.Key} || Value = {value}");
            }
        }
    }

    public bool GetButtonDown(string buttonName, int index = 0)
    {
        if (!_buttonKeys.ContainsKey(buttonName))
        {
            Debug.LogError("Error en nombre del boton");
            return false;
        }
        return Input.GetKeyDown(_buttonKeys[buttonName][index]);
    }

    public string[] GetButtonNames()
    {
        return _buttonKeys.Keys.ToArray();
    }

    public Dictionary<string, KeyCode[]> GetButtonKeys()
    {
        return _buttonKeys;
    }

    public string GetKeyNamesForButton(string buttonName, int index = 0)
    {

        if (!_buttonKeys.ContainsKey(buttonName))
        {
            Debug.LogError("Error en nombre del boton");
            return "N/A";
        }

        return _buttonKeys[buttonName][index].ToString();
    }

    public bool SetButtonForKey(string buttonName, KeyCode keyCode, int alt = 0)
    {
        if (alt > _buttonKeys.Values.Count())
            return false;

        foreach (var value in _buttonKeys.Values.ToList())
        {
            foreach (var item in value)
                if (item == keyCode)
                {
                    Debug.LogWarning($"The button {keyCode} is alredy used");
                    return false;
                }
        }
        _buttonKeys[buttonName][alt] = keyCode;
        return true;
    }

}
