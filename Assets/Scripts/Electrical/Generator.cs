using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public List<GameObject> _connection;

    private void Start()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        foreach (var obj in _connection)
        {
            if (obj != null && obj.GetComponent<IElectric>() != null)
            {
                obj.GetComponent<IElectric>().TurnOn();
            }
            
        }
    }
}
