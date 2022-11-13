using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace InputKey
{
    public class InputManager
    {/// <summary>
     /// Setea las teclas para cada accion
     /// </summary>
        //Hacer que dependiendo del voton depende de si es ppal o alternativo
        private static Dictionary<string, KeyCode> _buttonKeys;
        public void OnEnable()
        {
            _buttonKeys = new Dictionary<string, KeyCode>();
            // Agarrar del PlayerImputs y cargar las teclas / reemplazarlas (si hubo cambios).
            _buttonKeys["Jump"] = KeyCode.Space;
            _buttonKeys["Left"] = KeyCode.A;
            _buttonKeys["Right"] = KeyCode.D;

        }

        public bool GetButtonDown(string buttonName)
        {
            if (!_buttonKeys.ContainsKey(buttonName))
            {
                Debug.LogError("Error en nombre del boton");
                return false;
            }
            return Input.GetKeyDown(_buttonKeys[buttonName]);
        }

        public string[] GetButtonNames()
        {
            return _buttonKeys.Keys.ToArray();
        }

        public Dictionary<string, KeyCode> GetButtonKeys()
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

            return _buttonKeys[buttonName].ToString();
        }

        public bool SetButtonForKey(string buttonName, KeyCode keyCode, int alt = 0)
        {
            if (alt > _buttonKeys.Values.Count())
                return false;

            foreach (var value in _buttonKeys.Values.ToList())
            {
                if (value == keyCode)
                {
                    Debug.LogWarning($"The button {keyCode} is alredy used");
                    return false;
                }
            }
            _buttonKeys[buttonName] = keyCode;
            return true;
        }

    }
}

