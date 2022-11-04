using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{
    Dictionary<string, KeyCode> buttonKeys;
    private void OnEnable()
    {
        buttonKeys = new Dictionary<string, KeyCode>();

        buttonKeys["Jump"] = KeyCode.Escape;
        buttonKeys["Left"] = KeyCode.LeftArrow;
        buttonKeys["Left"] = KeyCode.A;
        buttonKeys["Right"] = KeyCode.RightArrow;
        buttonKeys["Right"] = KeyCode.D;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool GetButtonDown(string buttonName)
    {
        if (!buttonKeys.ContainsKey(buttonName))
        {
            Debug.LogError("Error en nombre del boton");
            return false;
        }
        return Input.GetKeyDown(buttonKeys[buttonName]);
    }

    public string[] GetButtonNames()
    {
        return buttonKeys.Keys.ToArray();
    }

    public string GetKeyNamesForButton(string buttonName)
    {

        if (!buttonKeys.ContainsKey(buttonName))
        {
            Debug.LogError("Error en nombre del boton");
            return "N/A";
        }

        return buttonKeys[buttonName].ToString();
    }

    public void SetButtonForKey(string buttonName, KeyCode keyCode)
    {
        buttonKeys[buttonName] = keyCode;
    }

}
