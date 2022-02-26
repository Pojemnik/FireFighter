using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarController : MonoBehaviour
{
    [System.Serializable]
    public enum BarOrientation
    {
        Vertical,
        Horizontal
    }

    public Sprite deathSprite;

    [Header("Bar settings")]
    [Tooltip("Size of bar when value is 0")]
    [SerializeField]
    private float _minSize;
    [Tooltip("Size of bar when calue is 100")]
    [SerializeField]
    private float _maxSize;
    [SerializeField]
    private BarOrientation _orientation;

    [Header("Config")]
    [SerializeField]
    private float _maxValue;

    private RectTransform _rect;
    private Image _image;

    public void OnValueChange(float value)
    {
        if (value < 0)
        {
            return;
        }

        float precent = value / _maxValue;
        float size = Mathf.Lerp(_minSize, _maxSize, precent);
        _rect.SetSizeWithCurrentAnchors((_orientation == BarOrientation.Horizontal) ? RectTransform.Axis.Horizontal : RectTransform.Axis.Vertical, size);
    }

    public void OnDeath()
    {
        this._image.sprite = deathSprite;
        this._image.color = Color.white;
        _rect.SetSizeWithCurrentAnchors((_orientation == BarOrientation.Horizontal) ? RectTransform.Axis.Horizontal : RectTransform.Axis.Vertical, _rect.rect.height);
    }

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _rect.SetSizeWithCurrentAnchors((_orientation == BarOrientation.Horizontal) ? RectTransform.Axis.Horizontal : RectTransform.Axis.Vertical, _maxSize);
        _image = GetComponent<Image>();
    }
}
