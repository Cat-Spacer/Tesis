using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace InputKey
{
    public class InputDictionary
    {/// <summary>
     /// Setea las teclas para cada accion
     /// </summary>
        //Hacer que dependiendo del voton depende de si es ppal o alternativo
        private static Dictionary<TypeOfKeys, KeyCode> _buttonKeys;

        public enum TypeOfKeys
        {
            None,
            Jump,
            ClimbUp,
            ClimbDown,
            Left,
            Right,
            Attack,
            Interact
        }
        public void OnStartIfNotSave()
        {
            _buttonKeys = new Dictionary<TypeOfKeys, KeyCode>();
            // Agarrar del PlayerImputs y cargar las teclas / reemplazarlas (si hubo cambios).
            _buttonKeys[TypeOfKeys.Jump] = KeyCode.Space;
            _buttonKeys[TypeOfKeys.Left] = KeyCode.A;
            _buttonKeys[TypeOfKeys.Right] = KeyCode.D;
            _buttonKeys[TypeOfKeys.ClimbUp] = KeyCode.W;
            _buttonKeys[TypeOfKeys.ClimbDown] = KeyCode.S;
            _buttonKeys[TypeOfKeys.Attack] = KeyCode.J;
            _buttonKeys[TypeOfKeys.Interact] = KeyCode.E;

        }

        public bool GetButtonDown(TypeOfKeys buttonName)
        {
            if (!_buttonKeys.ContainsKey(buttonName))
            {
                Debug.LogError("Error en nombre del boton");
                return false;
            }
            return Input.GetKeyDown(_buttonKeys[buttonName]);
        }

        public TypeOfKeys[] GetButtonNames()
        {
            return _buttonKeys.Keys.ToArray();
        }

        public Dictionary<TypeOfKeys, KeyCode> GetButtonKeys()
        {
            return _buttonKeys;
        }

        public string GetKeyNamesForButton(TypeOfKeys buttonName)
        {

            if (!_buttonKeys.ContainsKey(buttonName))
            {
                Debug.LogError("Error en nombre del boton");
                return "N/A";
            }

            return _buttonKeys[buttonName].ToString();
        }

        public bool SetButtonForKey(TypeOfKeys buttonName, KeyCode keyCode, int alt = 0)
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

