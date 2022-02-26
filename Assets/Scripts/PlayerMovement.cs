using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float _normalSpeed;
    [SerializeField]
    private float _slowSpeed;
    [SerializeField]
    private float _jumpSpeed;

    [Header("Camera")]
    [SerializeField]
    private Camera _playerCamera;
    [SerializeField]
    private float _cameraSesitivity;
    [SerializeField]
    private float _lookXLimit = 45.0f;
    [SerializeField]
    private float _crouchHeightDelta;

    [Header("Other")]
    [SerializeField]
    private float _gravity;

    private CharacterController _characterController;
    private Vector3 _moveDirection = Vector3.zero;
    private float _rotationX = 0;
    private bool _crouching;
    private bool _crouchingInput;
    private Vector3 _forward;
    private Vector3 _right;
    private Vector2 _inputSpeed;
    private bool _jump;
    private float _defaultCenterHeight;

    public void Move(InputAction.CallbackContext context)
    {
        _inputSpeed = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_characterController.isGrounded)
            {
                _jump = true;
            }
        }
    }

    public void Look(InputAction.CallbackContext context)
    {
        Vector2 lookDirection = context.ReadValue<Vector2>();
        _rotationX += -lookDirection.y * _cameraSesitivity;
        _rotationX = Mathf.Clamp(_rotationX, -_lookXLimit, _lookXLimit);
        _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, lookDirection.x * _cameraSesitivity, 0);
        UpdateDirectionVectors();
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.started && !_crouching)
        {
            _crouching = _crouchingInput = true;
            StartChrouching();
        }
        if (context.canceled && _crouching)
        {
            _crouchingInput = false;
            if (CrouchStopCheck())
            {
                StopCrouching();
            }
        }
    }

    private void StopCrouching()
    {
        _crouching = false;
        Vector3 cameraPos = _playerCamera.transform.position;
        cameraPos.y += _crouchHeightDelta;
        _playerCamera.transform.position = cameraPos;
        _characterController.height += _crouchHeightDelta;
        Vector3 center = _characterController.center;
        center.y += _crouchHeightDelta / 2f;
        _characterController.center = center;
    }

    private void StartChrouching()
    {
        Vector3 cameraPos = _playerCamera.transform.position;
        cameraPos.y -= _crouchHeightDelta;
        _playerCamera.transform.position = cameraPos;
        _characterController.height -= _crouchHeightDelta;
        Vector3 center = _characterController.center;
        center.y -= _crouchHeightDelta / 2f;
        _characterController.center = center;
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _defaultCenterHeight = _characterController.center.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UpdateDirectionVectors();
    }

    private void Update()
    {
        if(_crouching && !_crouchingInput)
        {
            if (CrouchStopCheck())
            {
                StopCrouching();
            }
        }
        float movementDirectionY = _moveDirection.y;
        Vector3 currentSpeed;
        currentSpeed.x = (_crouching ? _slowSpeed : _normalSpeed) * _inputSpeed.y;
        currentSpeed.y = (_crouching ? _slowSpeed : _normalSpeed) * _inputSpeed.x;
        _moveDirection = (_forward * currentSpeed.x) + (_right * currentSpeed.y);
        if (_jump)
        {
            _moveDirection.y = _jumpSpeed;
            _jump = false;
        }
        else
        {
            _moveDirection.y = movementDirectionY;
        }
        if (!_characterController.isGrounded)
        {
            _moveDirection.y -= _gravity * Time.deltaTime;
        }
        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    private bool CrouchStopCheck()
    {
        return !Physics.Raycast(transform.position, Vector3.up, (_characterController.height + _crouchHeightDelta + _defaultCenterHeight) / 2f, LayerMask.GetMask("Environment"));
    }

    private void UpdateDirectionVectors()
    {
        _forward = transform.TransformDirection(Vector3.forward);
        _right = transform.TransformDirection(Vector3.right);
    }
}
