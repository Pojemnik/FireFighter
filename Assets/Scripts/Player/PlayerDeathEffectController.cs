using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerDeathEffectController : MonoBehaviour
{
    [System.Serializable]
    private struct MovementStep
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 upDirection;
        public float duration;
    }

    [Header("Config")]
    [SerializeField]
    private MovementStep[] steps;
    [Header("References")]
    [SerializeField]
    private GameObject _target;

    private CharacterController _controller;
    private PlayerMovement _movement;

    private bool _isPlaying;
    private int _currentStep;
    private float _currentStepTime;
    private Vector3 _startPos;
    private Quaternion _startRotation;

    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        _controller = GetComponent<CharacterController>();
        //for (int i = 0; i < steps.Length; i++)
        //{
        //    MovementStep step = steps[i];
        //    step.position = _target.transform.TransformPoint(step.position);
        //    step.rotation = _target.transform.TransformDirection(step.rotation);
        //    Debug.DrawRay(step.position, step.rotation, Color.magenta, 100000);
        //    Debug.Log(step.position);
        //    //Gizmos.DrawSphere(step.position, 0.1f);
        //}
    }

    public void PlayEffect()
    {
        _controller.enabled = false;
        _movement.enabled = false;
        _isPlaying = true;
        _currentStep = 0;
        _currentStepTime = 0;
        for (int i = 0; i < steps.Length; i++)
        {
            steps[i].position = _target.transform.TransformPoint(steps[i].position);
            steps[i].rotation = _target.transform.TransformDirection(steps[i].rotation);
            steps[i].upDirection = _target.transform.TransformDirection(steps[i].upDirection);
        }
        _startPos = _target.transform.position;
        _startRotation = _target.transform.rotation;
    }

    private void Update()
    {
        if (_isPlaying)
        {
            _currentStepTime += Time.deltaTime;
            if (_currentStepTime >= steps[_currentStep].duration)
            {
                _currentStepTime -= steps[_currentStep].duration;
                _startPos = steps[_currentStep].position;
                _startRotation = Quaternion.LookRotation(steps[_currentStep].rotation);
                _currentStep++;
                if (_currentStep >= steps.Length)
                {
                    _isPlaying = false;
                    return;
                }
            }
            _target.transform.position = Vector3.Lerp(_startPos, steps[_currentStep].position, _currentStepTime / steps[_currentStep].duration);
            _target.transform.rotation = Quaternion.Slerp(_startRotation, Quaternion.LookRotation(steps[_currentStep].rotation, steps[_currentStep].upDirection), _currentStepTime / steps[_currentStep].duration);
        }
    }
}
