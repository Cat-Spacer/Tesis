using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SpritesFinder : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _sprites;
    [SerializeField] private Image[] _images;
    [SerializeField] private RawImage[] _rawImages;
    [SerializeField] private string[] _names;
    [SerializeField] private bool _allDataBase = false;

    //void Start()
    //{
    //    ChargeArrayNames();
    //    ClearNames();
    //    SaveOnFile();
    //}

    //private void ChargeArrayNames()
    //{
    //    if (_allDataBase)
    //    {
    //        _names = AssetDatabase.FindAssets("t:Sprite");
    //        return;
    //    }
    //    _sprites = FindObjectsOfType<SpriteRenderer>();
    //    _images = FindObjectsOfType<Image>();
    //    _rawImages = FindObjectsOfType<RawImage>();
    //    int totalLenght = _sprites.Length + _images.Length + _rawImages.Length;
    //    _names = new string[totalLenght];

    //    for (int i = 0; i < totalLenght; i++)
    //    {
    //        if (i < _sprites.Length)
    //            _names[i] = _sprites[i].sprite.name;
    //        else if (i < _sprites.Length + _images.Length)
    //            _names[i] = _images[i - _sprites.Length].sprite.name;
    //        else
    //            _names[i] = _rawImages[i - _sprites.Length - _images.Length].texture.name;
    //    }
    //}

    //private void ClearNames()
    //{
    //    for (int i = 0; i < _names.Length; i++)
    //        for (int j = 0; j < i; j++)
    //            if (_names[i] == _names[j])
    //                _names[j] = null;

    //    _names = _names.OrderBy(x => x).ToArray();
    //}

    //private void SaveOnFile()
    //{
    //    //KeybindManager.Instance.saveManager.LoadData().mySpritesName = new string[_names.Length];
    //    KeybindManager.Instance.saveManager.LoadData().mySpritesName = _names;
    //    KeybindManager.Instance.saveManager.SaveJSON();
    //}
}