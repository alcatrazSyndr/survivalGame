using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SurvivalCharacter_CharacterController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Rigidbody _characterRigidbody;
    [SerializeField] private Transform _characterModel;
    [SerializeField] private InputReceiver _inputReceiver;

    [Header("Data")]
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 5f;
    [SerializeField] private float _turnSpeed = 5f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _jumpCooldown = 0.1f;
    [SerializeField] private LayerMask _groundMasks;
    public LayerMask GroundMasks { get { return _groundMasks; } }

    [Header("Runtime")]
    [SerializeField] private Vector3 _movementVector;
    [SerializeField] private float _currentMovementSpeed;
    [SerializeField] private float _currentSprintSpeed;
    [SerializeField] private bool _grounded = false;
    [SerializeField] private bool _jumpPrevention = false;
    public float TotalCurrentMovementSpeed { get { return (_currentMovementSpeed + _currentSprintSpeed) * _currentMovementSpeed; } }

    [Header("Events")]
    public UnityEvent OnStartedJump = new UnityEvent();

    private void Start()
    {
        _inputReceiver.OnCameraRelativeMovementInputChanged.AddListener(MovementInputChanged);
        _inputReceiver.OnSprintInputChanged.AddListener(SprintInputChanged);
        _inputReceiver.OnJumpInputChanged.AddListener(JumpInput);
    }

    private void Update()
    {
        if (_movementVector != Vector3.zero)
            _characterModel.rotation = Quaternion.Lerp(_characterModel.rotation, Quaternion.LookRotation(_movementVector), Time.deltaTime * _turnSpeed);
    }

    private void FixedUpdate()
    {
        _grounded = Physics.CheckSphere(_characterRigidbody.position, 0.1f, _groundMasks.value);

        if (_movementVector != Vector3.zero)
        {
            var charPos = _characterRigidbody.position;
            var moveSpeedVector = _movementVector * _currentMovementSpeed * _movementSpeed * 0.01f;
            var sprintSpeedVector = _movementVector * _currentSprintSpeed * _sprintSpeed * 0.01f;
            var moveVector = charPos + moveSpeedVector + sprintSpeedVector;

            _characterRigidbody.MovePosition(moveVector);
        }
    }

    private void MovementInputChanged(Vector2 movementInput)
    {
        var movementVector = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
        if (movementInput == Vector2.zero)
            _movementVector = Vector3.zero;
        else
            _movementVector = movementVector;

        _currentMovementSpeed = _movementVector.magnitude;
    }

    private void SprintInputChanged(bool toggle)
    {
        _currentSprintSpeed = toggle ? 1f : 0f;
    }

    private void JumpInput(bool down)
    {
        if (!down) return;
        if (_jumpPrevention) return;

        if (_grounded)
        {
            _jumpPrevention = true;
            StartCoroutine(JumpCooldownCRT());

            _grounded = false;
            _characterRigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            OnStartedJump?.Invoke();
        }
    }

    private IEnumerator JumpCooldownCRT()
    {
        yield return new WaitForSecondsRealtime(_jumpCooldown);
        _jumpPrevention = false;
        yield break;
    }
}
