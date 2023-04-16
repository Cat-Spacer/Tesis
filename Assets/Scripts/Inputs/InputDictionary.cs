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
        //Hacer que dependiendo del boton que sea ppal o alternativo
        public static Dictionary<TypeOfKeys, KeyCode> buttonKeys;
        private TypeOfKeys _replaceThisKey, _oldKey;

        public enum TypeOfKeys
        {
            None = -1,
            JumpDown,
            JumpUp,
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
                    Debug.Log($"<color=orange>{key} = {buttonKeys[key]}</color>");

            if (KeybindManager.Instance.saveManager.CheckFile() && buttonKeys != null) return;

            buttonKeys = new Dictionary<TypeOfKeys, KeyCode>();
            buttonKeys[TypeOfKeys.JumpDown] = KeyCode.Space;
            buttonKeys[TypeOfKeys.JumpUp] = KeyCode.Space;
            buttonKeys[TypeOfKeys.ClimbUp] = KeyCode.W;
            buttonKeys[TypeOfKeys.ClimbDown] = KeyCode.S;
            buttonKeys[TypeOfKeys.Left] = KeyCode.A;
            buttonKeys[TypeOfKeys.Right] = KeyCode.D;
            buttonKeys[TypeOfKeys.Attack] = KeyCode.J;
            buttonKeys[TypeOfKeys.Interact] = KeyCode.E;
            buttonKeys[TypeOfKeys.Dash] = KeyCode.LeftShift;

            foreach (var key in buttonKeys.Keys)
                KeybindManager.Instance.saveManager.LoadData().buttonKeys.Add(key);

            foreach (var value in buttonKeys.Values)
                KeybindManager.Instance.saveManager.LoadData().buttonValues.Add(value);

            KeybindManager.Instance.saveManager.SaveJSON();
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
                    //pregunto si cambia y le paso el typofkey por parametro.
                    _replaceThisKey = buttonName;
                    _oldKey = buttonKeys.FirstOrDefault(x => x.Value == value).Key;
                    return false;
                }
            }
            //buttonKeys[buttonName] = keyCode;
            SaveKeysData(buttonName, keyCode);
            Debug.Log($"Succesfuly changed {buttonName} to {keyCode}: on dictionary type {buttonKeys[buttonName]}");
            return true;
        }

        public void LoadDictionary(List<TypeOfKeys> keys, List<KeyCode> values)
        {
            if (buttonKeys == null)
                buttonKeys = new Dictionary<TypeOfKeys, KeyCode>();

            Debug.Log($"<color=yellow>Loading list of keys {keys} and values {values}: keys.Count {keys.Count}</color>");
            for (int i = 0; i < keys.Count; i++)
            {
                Debug.Log($"<color=green>Loading key {keys[i]} and value {values[i]}</color>");
                buttonKeys[keys[i]] = values[i];
            }
        }

        public void SaveKeysData(TypeOfKeys key, KeyCode keyCode)
        {
            buttonKeys[key] = keyCode;
            KeybindManager.Instance.saveManager.LoadData().buttonValues[key.GetHashCode()] = buttonKeys[key];
        }

        public void ReplaceKeyValue()
        {
            KeyCode valueToReplace = buttonKeys[_oldKey];
            buttonKeys[_oldKey] = buttonKeys[_replaceThisKey];
            buttonKeys[_replaceThisKey] = valueToReplace;
            KeybindManager.Instance.saveManager.LoadData().buttonValues[_oldKey.GetHashCode()] = buttonKeys[_oldKey];
            KeybindManager.Instance.saveManager.LoadData().buttonValues[_replaceThisKey.GetHashCode()] = valueToReplace;

            /*foreach (var key in buttonKeys.Keys)
                foreach (var value in buttonKeys.Values)
                    if (value == _replaceThisKey && KeybindManager.Instance.saveManager.LoadData().buttonKeys.Contains(key))
                    {
                        KeyCode aux = value;
                        buttonKeys[key] = value;
                        buttonKeys[keyToReplace] = valueOld;
                        KeybindManager.Instance.saveManager.LoadData().buttonValues[value.CompareTo(key)] = aux;
                        KeybindManager.Instance.saveManager.LoadData().buttonValues[value.CompareTo(_replaceThisKey)] = valueOld;
                    }
            */
        }
    }
}