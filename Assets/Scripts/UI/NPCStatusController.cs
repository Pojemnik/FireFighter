using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStatusController : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image _image;
    [SerializeField]
    private Sprite deathSprite;
    [SerializeField]
    private Sprite waitSprite;
    [SerializeField]
    private Sprite safeSprite;

    public void OnDeath()
    {
        _image.sprite = deathSprite;
    }

    public void OnSaved()
    {
        _image.sprite = safeSprite;
    }

    private void Start()
    {
        _image.sprite = waitSprite;
    }
}
