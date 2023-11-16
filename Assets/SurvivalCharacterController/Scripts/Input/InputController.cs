using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Transform _cameraTransform;

    [Header("Events")]
    public UnityEvent OnPlayerMenuInput = new UnityEvent();
    public UnityEvent OnInteractionInput = new UnityEvent();
    public UnityEvent<Vector2> OnMouseInputChanged = new UnityEvent<Vector2>();

    [Header("Runtime")]
    private Vector2 _mouseInput = new Vector2();
    private Vector2 _movementInput = new Vector2();
    private Vector2 _cameraRelativeMovementInput = new Vector2();
    private bool _sprintInput = false;
    private bool _jumpInput = false;
    private bool _leftSecondaryInput = false;
    private bool _rightSecondaryInput = false;
    private InputReceiver _inputReceiver = null;
    public InputReceiver CurrentInputReceiver { get { return _inputReceiver; } }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // mouse input
        var mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (mouseInput != _mouseInput)
        {
            if (_inputReceiver != null)
            {
                _inputReceiver.OnMouseInputChanged?.Invoke(mouseInput);
            }
            OnMouseInputChanged?.Invoke(mouseInput);
        }
        _mouseInput = mouseInput;

        // movement input
        var movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (movementInput != _movementInput && _inputReceiver != null)
            _inputReceiver.OnMovementInputChanged?.Invoke(movementInput);
        _movementInput = movementInput;

        // camera relative movement input
        var cameraRelativeMovementInput = CameraRelativeInput(movementInput);
        if (cameraRelativeMovementInput != _cameraRelativeMovementInput && _inputReceiver != null)
            _inputReceiver.OnCameraRelativeMovementInputChanged?.Invoke(cameraRelativeMovementInput);
        _cameraRelativeMovementInput = cameraRelativeMovementInput;

        // jump input
        var jumpInput = Input.GetKey(KeyCode.Space);
        if (jumpInput != _jumpInput && _inputReceiver != null)
            _inputReceiver.OnJumpInputChanged?.Invoke(jumpInput);
        _jumpInput = jumpInput;

        // sprint input
        var sprintInput = Input.GetKey(KeyCode.LeftShift);
        if (sprintInput != _sprintInput && _inputReceiver != null)
            _inputReceiver.OnSprintInputChanged?.Invoke(sprintInput);
        _sprintInput = sprintInput;

        // menu input
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_inputReceiver != null)
            {
                _inputReceiver.OnPlayerMenuInput?.Invoke();
            }
            OnPlayerMenuInput?.Invoke();
        }

        // interaction input
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_inputReceiver != null)
            {
                _inputReceiver.OnInteractionInput?.Invoke();
            }
            OnInteractionInput?.Invoke();
        }

        // left secondary input (Q)
        var leftSecondaryInput = Input.GetKey(KeyCode.Q);
        if (leftSecondaryInput != _leftSecondaryInput && _inputReceiver != null)
            _inputReceiver.OnLeftSecondaryInputChanged?.Invoke(leftSecondaryInput);
        _leftSecondaryInput = leftSecondaryInput;

        // right secondary input (E)
        var rightSecondaryInput = Input.GetKey(KeyCode.E);
        if (rightSecondaryInput != _rightSecondaryInput && _inputReceiver != null)
            _inputReceiver.OnRightSecondaryInputChanged?.Invoke(rightSecondaryInput);
        _rightSecondaryInput = rightSecondaryInput;
    }

    public void SetInputReceiver(InputReceiver inputReceiver)
    {
        if (_inputReceiver != null)
        {
            _inputReceiver.OnCameraRelativeMovementInputChanged?.Invoke(Vector2.zero);
            _inputReceiver.OnJumpInputChanged?.Invoke(false);
            _inputReceiver.OnMouseInputChanged?.Invoke(Vector2.zero);
            _inputReceiver.OnMovementInputChanged?.Invoke(Vector2.zero);
            _inputReceiver.OnSprintInputChanged?.Invoke(false);
            _inputReceiver.OnLeftSecondaryInputChanged?.Invoke(false);
            _inputReceiver.OnRightSecondaryInputChanged?.Invoke(false);
        }
        _inputReceiver = inputReceiver;
    }

    public Vector2 CameraRelativeInput(Vector2 input)
    {
        var forward = _cameraTransform.forward;
        var right = _cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        var desiredInputDirection = forward * input.y + right * input.x;
        return new Vector2(desiredInputDirection.x, desiredInputDirection.z);
    }
}
