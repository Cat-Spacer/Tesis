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
        public static Dictionary<TypeOfKeys, KeyCode> buttonKeys;

        public enum TypeOfKeys
        {
            None,
            Jump,
            ClimbUp,
            ClimbDown,
            Left,
            Right,
            Attack,
            Interact,
            Dash
        }

        public void OnStartIfNotSave()
        {
            if (buttonKeys != null)
                foreach (var key in buttonKeys.Keys)
                    Debug.Log($"<color=green>{key} = {buttonKeys[key]}</color>");

            Debug.Log($"Ingrese");

            if (buttonKeys != null) return;
            buttonKeys = new Dictionary<TypeOfKeys, KeyCode>();
            // Agarrar del PlayerImputs y cargar las teclas / reemplazarlas (si hubo cambios).
            buttonKeys[TypeOfKeys.Jump] = KeyCode.Space;
            buttonKeys[TypeOfKeys.Left] = KeyCode.A;
            buttonKeys[TypeOfKeys.Right] = KeyCode.D;
            buttonKeys[TypeOfKeys.ClimbUp] = KeyCode.W;
            buttonKeys[TypeOfKeys.ClimbDown] = KeyCode.S;
            buttonKeys[TypeOfKeys.Attack] = KeyCode.J;
            buttonKeys[TypeOfKeys.Interact] = KeyCode.E;
            buttonKeys[TypeOfKeys.Dash] = KeyCode.LeftShift;
            Debug.Log($"<color=yellow>Button Keys set to default</color>");
        }

        public bool GetButtonDown(TypeOfKeys buttonName)
        {
            if (!buttonKeys.ContainsKey(buttonName))
            {
                Debug.LogError("Error en nombre del boton");
                return false;
            }
            return Input.GetKeyDown(buttonKeys[buttonName]);
        }

        public TypeOfKeys[] GetButtonNames()
        {
            return buttonKeys.Keys.ToArray();
        }

        public Dictionary<TypeOfKeys, KeyCode> GetButtonKeys()
        {
            return buttonKeys;
        }

        public string GetKeyNamesForButton(TypeOfKeys buttonName)
        {

            if (!buttonKeys.ContainsKey(buttonName))
            {
                Debug.LogError("Error en nombre del boton");
                return "N/A";
            }

            return buttonKeys[buttonName].ToString();
        }

        public bool SetButtonForKey(TypeOfKeys buttonName, KeyCode keyCode, int alt = 0)
        {
            if (alt > buttonKeys.Values.Count())
                return false;

            foreach (var value in buttonKeys.Values.ToList())
            {
                if (value == keyCode)
                {
                    Debug.LogWarning($"The button {keyCode} is alredy used");
                    return false;
                }
            }
            buttonKeys[buttonName] = keyCode;
            Debug.Log($"Succesfuly changed {buttonName} to {keyCode}: on dictionary type {buttonKeys[buttonName]}");
            return true;
        }
    }
}