using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class NPCMaterialManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float _totalFadeTime;

    [Header("Materials")]
    [SerializeField]
    private Material _carriedMaterial;
    [SerializeField]
    private Material _fadeMaterial;

    [Header("Colors")]
    [SerializeField]
    private Color _savedWhenDroppedColor;
    [SerializeField]
    private Color _canDropColor;
    [SerializeField]
    private Color _cantDropColor;


    public event System.EventHandler FadeOutEnd;

    private Material _defaultMaterial;
    private SkinnedMeshRenderer _meshRenderer;

    private bool _safe = false;
    private bool _picked = false;
    private bool _fading = false;
    private bool _canDrop = true;
    private float _fadeTime = 0;

    private void Start()
    {
        _meshRenderer = GetComponent<SkinnedMeshRenderer>();
        _defaultMaterial = _meshRenderer.material;
    }

    private void Update()
    {
        if (!_fading)
        {
            return;
        }
        _fadeTime += Time.deltaTime;
        if (_fadeTime >= _totalFadeTime)
        {
            _fading = false;
            FadeOutEnd?.Invoke(this, null);
            return;
        }
        Color c = _meshRenderer.material.color;
        c.a = Mathf.Lerp(1, 0, _fadeTime / _totalFadeTime);
        _meshRenderer.material.color = c;
    }

    public void OnSafeStateChange(bool state)
    {
        if (_fading)
        {
            return;
        }
        _safe = state;
        if (_safe)
        {
            _meshRenderer.material.color = _savedWhenDroppedColor;
        }
        else
        {
            _meshRenderer.material.color = _canDrop ? _canDropColor : _cantDropColor;
        }
    }

    public void OnPickupStateChange(bool state)
    {
        if (_fading)
        {
            return;
        }
        _picked = state;
        if (_picked)
        {
            _meshRenderer.material = _carriedMaterial;
            if (_safe)
            {
                _meshRenderer.material.color = _savedWhenDroppedColor;
            }
            else
            {
                _meshRenderer.material.color = _canDrop ? _canDropColor : _cantDropColor;
            }
        }
        else
        {
            _meshRenderer.material.color = Color.white;
            _meshRenderer.material = _defaultMaterial;
        }
    }

    public void CanDropStateChanged(bool state)
    {
        if (_fading)
        {
            return;
        }
        _canDrop = state;
        if (!_safe)
        {
            _meshRenderer.material.color = _canDrop ? _canDropColor : _cantDropColor;
        }
    }

    public void OnDeath()
    {
        FadeOut();
    }

    public void OnSafe()
    {
        FadeOut();
    }

    private void FadeOut()
    {
        if (_fading)
        {
            return;
        }
        _fading = true;
        _meshRenderer.material = _fadeMaterial;
        _meshRenderer.material.color = Color.white;
    }
}
