using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

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
}
