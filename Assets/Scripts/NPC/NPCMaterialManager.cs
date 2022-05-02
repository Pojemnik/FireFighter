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

    private bool _fading = false;
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

    public void SetStatus(NPCController.NpcStatus status)
    {
        if (_fading)
        {
            return;
        }
        _meshRenderer.enabled = true;
        switch (status)
        {
            case NPCController.NpcStatus.Dropped:
                _meshRenderer.material.color = Color.white;
                _meshRenderer.material = _defaultMaterial;
                break;
            case NPCController.NpcStatus.Dead:
            case NPCController.NpcStatus.Saved:
                FadeOut();
                break;
            case NPCController.NpcStatus.Hidden:
                _meshRenderer.enabled = false;
                break;
            case NPCController.NpcStatus.CanDrop:
                _meshRenderer.material.color = _canDropColor;
                break;
            case NPCController.NpcStatus.CantDrop:
                _meshRenderer.material.color = _cantDropColor;
                break;
            case NPCController.NpcStatus.SafeToDrop:
                _meshRenderer.material.color = _savedWhenDroppedColor;
                break;
        }
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
