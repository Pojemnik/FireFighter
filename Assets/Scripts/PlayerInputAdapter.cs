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

    public void UseAxe(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _axe.Use();
        }
    }

    public void UseExtinguisher(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _extinguisher.StartExtinguishing();
        }
        if(context.canceled)
        {
            _extinguisher.StopExtingiushing();
        }
    }
}
