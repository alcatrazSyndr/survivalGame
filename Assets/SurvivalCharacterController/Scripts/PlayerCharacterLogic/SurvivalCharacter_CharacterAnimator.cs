using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalCharacter_CharacterAnimator : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private SurvivalCharacter_CharacterController _characterController;
    [SerializeField] private Animator _characterAnimator;

    [Header("Data")]
    [SerializeField] private float _walkAnimationLerpSpeed = 5f;
    [SerializeField] private float _landingHeight = 0.5f;
    public float LandingHeight { get { return _landingHeight; } }

    [Header("Runtime")]
    private float _currentMoveSpeed = 0f;
    private float _currentMoveSpeedTarget = 0f;

    private void Start()
    {
        _characterController.OnStartedJump.AddListener(StartedJump);
        _characterController.OnFixedToRigidbody.AddListener(AnimateCharacterFixedToRigidbody);
    }

    private void Update()
    {
        _currentMoveSpeedTarget = _characterController.TotalCurrentMovementSpeed;

        if (Mathf.Abs(_currentMoveSpeed - _currentMoveSpeedTarget) > 0.01f)
        {
            _currentMoveSpeed = Mathf.Lerp(_currentMoveSpeed, _currentMoveSpeedTarget, Time.deltaTime * _walkAnimationLerpSpeed);
            _characterAnimator.SetFloat("MovementSpeed", _currentMoveSpeed);
        }
        else if (_currentMoveSpeedTarget == 0f && _currentMoveSpeed != 0f)
        {
            _currentMoveSpeed = 0f;
            _characterAnimator.SetFloat("MovementSpeed", _currentMoveSpeed);
        }
    }

    private void StartedJump()
    {
        _characterAnimator.Play("Jump_Unarmed");
    }

    private void AnimateCharacterFixedToRigidbody(Rigidbody rigidbody)
    {
        if (rigidbody != null)
        {
            if (rigidbody.transform.GetComponent<VehicleController>())
            {
                _characterAnimator.Play("Driving_Jeep");
            }
        }
        else
        {
            _characterAnimator.Play("Move Tree");
        }
    }
}
