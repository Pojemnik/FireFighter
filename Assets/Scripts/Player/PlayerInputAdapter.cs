using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputAdapter : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Axe _axe;
    [SerializeField]
    private Extinguisher _extinguisher;
    [SerializeField]
    private PlayerAnimationController _animationController;

    public void UseAxe(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _axe.Use();
            _animationController.UseAxe();
        }
    }

    public void UseExtinguisher(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _extinguisher.StartExtinguishing();
            _animationController.StartExtinguishing();
        }
        if(context.canceled)
        {
            _extinguisher.StopExtingiushing();
            _animationController.StopExtinguishing();
        }
    }
}
