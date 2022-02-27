using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Axe _axe;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartWalking()
    {
        
    }

    public void StopWalking()
    {

    }

    public void StartJump()
    {

    }

    public void StopJump()
    {

    }

    public void GoToIdle()
    {
        _animator.SetTrigger("ForceIdle");
        _animator.SetBool("Extinguisher", false);
    }

    public void UseAxe()
    {
        _animator.SetTrigger("Axe");
    }

    public void StartExtinguishing()
    {
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
}
