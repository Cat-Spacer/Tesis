using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowerUI : MonoBehaviour
{
    [SerializeField] private FlowerType _type;
    [SerializeField] Image _flowerImage;

    public FlowerType Type => _type;

    private void Start()
    {
        _flowerImage.enabled = false;
    }

    public void ActivateImage()
    {
        if (_flowerImage != null )
        {
            _flowerImage.enabled=true;
        }
        else
        {
            Debug.LogWarning("FlowerUI: La imagen es nulo.");
        }
    }

    public bool IsActive()
    {
        return _flowerImage.enabled;
    }

}
public enum FlowerType
{
    Green,
    Yellow,
    Purple
}