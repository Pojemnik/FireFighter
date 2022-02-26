using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    [Header("Bar settings")]
    [Tooltip("Width of bar when health is 0")]
    [SerializeField]
    private float minWidth;
    [Tooltip("Width of bar when health is 100")]
    [SerializeField]
    private float maxWidth;

    [Header("Config")]
    [SerializeField]
    private float maxHealth;

    private RectTransform rect;

    public void OnPlayerHealthChange(float value)
    {
        if (value < 0)
        {
            value = 0;
        }
        float hpPercent = value / maxHealth;
        float width = Mathf.Lerp(minWidth, maxWidth, hpPercent);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
    }

}
