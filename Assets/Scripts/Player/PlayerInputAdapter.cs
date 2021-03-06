using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputAdapter : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Extinguisher _extinguisher;
    [SerializeField]
    private PlayerAnimationController _animationController;
    [SerializeField]
    private PlayerMovement _movement;
    [SerializeField]
    private PlayerInteractor _interactor;
    [SerializeField]
    private PlayerNPCCarrier _carrier;

    [HideInInspector]
    public bool EnableMovement = true;

    public void UseAxe(InputAction.CallbackContext context)
    {
        if(!EnableMovement || _carrier.IsCarrying)
        {
            return;
        }
        if(context.started)
        {
            _animationController.UseAxe();
        }
    }

    public void UseExtinguisher(InputAction.CallbackContext context)
    {
        if (!EnableMovement || _carrier.IsCarrying)
        {
            return;
        }
        if (context.started)
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

    public void Move(InputAction.CallbackContext context)
    {
        if (!EnableMovement)
        {
            return;
        }
        _movement.Move(context.ReadValue<Vector2>());
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!EnableMovement || _carrier.IsCarrying)
        {
            return;
        }
        if (context.started)
        {
            _movement.Jump();
        }
    }

    public void Look(InputAction.CallbackContext context)
    {
        if (!EnableMovement)
        {
            return;
        }
        _movement.Look(context.ReadValue<Vector2>());
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (!EnableMovement || _carrier.IsCarrying)
        {
            return;
        }
        if (context.started)
        {
            _movement.StartCrouching();
        }
        if (context.canceled)
        {
            _movement.StopCrouching();
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!EnableMovement)
        {
            return;
        }
        if (context.started)
        {
            if (_carrier.IsCarrying)
            {
                _carrier.DropNPC();
            }
            else
            {
                if (_movement.IsCrouching)
                {
                    _movement.StopCrouching();
                }
                else
                {
                    _interactor.PickUp();
                }
            }
        }
    }
}
