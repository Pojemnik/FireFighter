using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Axe _axe;

    private Animator _animator;

    private bool _resetLandNextFrame = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartWalking()
    {
        if(!_animator.GetBool("Extinguisher"))
        {
            _animator.SetBool("Run", true);
        }
    }

    public void StopWalking()
    {
        _animator.SetBool("Run", false);
    }

    public void StartJump()
    {
        //ResetBools();
        _animator.SetTrigger("Jump");
    }

    public void StopJump()
    {
        _animator.SetTrigger("Land");
        _resetLandNextFrame = true;
    }

    public void GoToIdle()
    {
        _animator.SetTrigger("ForceIdle");
        ResetBools();
    }

    public void UseAxe()
    {
        _animator.SetTrigger("Land");
        _resetLandNextFrame = true;
        _animator.SetTrigger("Axe");
    }

    public void StartExtinguishing()
    {
        _animator.SetTrigger("Land");
        _resetLandNextFrame = true;
        _animator.SetBool("Run", false);
        _animator.SetBool("Extinguisher", true);
    }

    public void StopExtinguishing()
    {
        _animator.SetBool("Extinguisher", false);
    }

    private void OnAxeUseEvent()
    {
        _axe.Use();
    }

    private void ResetBools()
    {
        _animator.SetBool("Extinguisher", false);
        _animator.SetBool("Run", false);
    }

    private void Update()
    {
        if(_resetLandNextFrame)
        {
            _resetLandNextFrame = false;
            _animator.ResetTrigger("Land");
        }
    }
}
