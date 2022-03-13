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
    [SerializeField]
    private float _kickForce;

    [Header("References")]
    [SerializeField]
    private PlayerAnimationController _animationController;

    public System.EventHandler Landed;
    public System.EventHandler<bool> WalkingStateChanged;
    public bool IsCrouching { get => _crouching; }

    [HideInInspector]
    public bool MovementEnabled = true;

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
    private bool _lastGroundedState = true;
    private bool _lastWalkingState = true;
    private bool _carrying = false;

    public void Move(Vector2 value)
    {
        _inputSpeed = value;
    }

    public void Jump()
    {
        if (_characterController.isGrounded)
        {
            _jump = true;
            _animationController.StartJump();
        }
    }

    public void Look(Vector2 direction)
    {
        _rotationX += -direction.y * _cameraSesitivity;
        _rotationX = Mathf.Clamp(_rotationX, -_lookXLimit, _lookXLimit);
        _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, direction.x * _cameraSesitivity, 0);
        UpdateDirectionVectors();
    }

    public void StopCrouching()
    {
        if (_crouching)
        {
            _crouchingInput = false;
            if (CrouchStopCheck())
            {
                StopCrouchingInternal();
            }
        }
    }

    public void StartCrouching()
    {
        if (!_crouching)
        {
            _crouching = _crouchingInput = true;
            StartChrouchingInternal();
        }
    }

    public void SetCarrying(bool state)
    {
        _carrying = state;
    }

    private void StopCrouchingInternal()
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

    private void StartChrouchingInternal()
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
        UpdateDirectionVectors();
    }

    private void Update()
    {
        if (_crouching && !_crouchingInput)
        {
            if (CrouchStopCheck())
            {
                StopCrouching();
            }
        }
        float movementDirectionY = _moveDirection.y;
        Vector3 currentSpeed;
        currentSpeed.x = ((_crouching || _carrying) ? _slowSpeed : _normalSpeed) * _inputSpeed.y;
        currentSpeed.y = ((_crouching || _carrying) ? _slowSpeed : _normalSpeed) * _inputSpeed.x;
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
        SendEventIfLanded();
        _lastGroundedState = _characterController.isGrounded;
        SendWalkingStateEvent();
    }

    private void SendWalkingStateEvent()
    {
        if (_lastWalkingState == true && (!_characterController.isGrounded || _inputSpeed == Vector2.zero))
        {
            WalkingStateChanged?.Invoke(this, false);
            _lastWalkingState = false;
            _animationController.StopWalking();
        }
        if (_lastWalkingState == false && _characterController.isGrounded && _inputSpeed != Vector2.zero)
        {
            WalkingStateChanged?.Invoke(this, true);
            _lastWalkingState = true;
            _animationController.StartWalking();
        }
    }

    private void SendEventIfLanded()
    {
        if (_lastGroundedState == false && _characterController.isGrounded == true)
        {
            Landed?.Invoke(this, null);
            _animationController.StopJump();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForce((hit.point - transform.position).normalized * _kickForce);
        }
    }

    private bool CrouchStopCheck()
    {
        Vector3[] angles = new Vector3[] {
            new Vector3(0, 0, 0),
        new Vector3(0, 0, 15),
        new Vector3(15, 0, 0),
        new Vector3(-15, 0, 0),
        new Vector3(0, 0, -15)
        };
        for (int i = 0; i < 5; i++)
        {
            if (Physics.Raycast(transform.position, Quaternion.Euler(angles[i]) * Vector3.up, ((_characterController.height + _crouchHeightDelta + _defaultCenterHeight) / 2f) / Mathf.Cos(i == 0 ? 0 : Mathf.Deg2Rad * 15), LayerMask.GetMask("Environment")))
            {
                return false;
            }
        }
        return true;
    }

    private void UpdateDirectionVectors()
    {
        _forward = transform.TransformDirection(Vector3.forward);
        _right = transform.TransformDirection(Vector3.right);
    }
}
