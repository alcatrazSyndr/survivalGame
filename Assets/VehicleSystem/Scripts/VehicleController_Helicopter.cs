using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController_Helicopter : VehicleController
{
    [Header("Helicopter Speed Data")]
    [SerializeField] private float _forwardSpeed = 100f;
    [SerializeField] private float _strafeSpeed = 100f;
    [SerializeField] private float _thrustSpeed = 100f;

    [Header("Helicopter Animation Data")]
    [SerializeField] private float _pitchSpeed = 100f;
    [SerializeField] private float _yawSpeed = 100f;
    [SerializeField] private float _rollSpeed = 100f;
    [SerializeField] private float _maxRotorSpeed = 100f;
    [SerializeField] private float _rotorStartupSpeed = 100f;

    [Header("Helicopter Visual References")]
    [SerializeField] private Transform _rotorMain;
    [SerializeField] private Transform _rotorBack;

    [Header("Helicopter Runtime")]
    [SerializeField] private Vector3 _vehicleInput = Vector3.zero;
    [SerializeField] private float _vehicleYawInput = 0f;
    [SerializeField] private bool _canLift = false;
    [SerializeField] private float _pitch = 0f;
    [SerializeField] private float _yaw = 0f;
    [SerializeField] private float _roll = 0f;
    [SerializeField] private float _rotorSpeed = 0f;

    protected override void Start()
    {
        base.Start();

        _mainRigidbody.maxLinearVelocity = 20f;
        _mainRigidbody.maxAngularVelocity = 0f;

        _inputReceiver.OnMovementInputChanged.AddListener(MovementInputChanged);
        _inputReceiver.OnJumpInputChanged.AddListener(UpForceInputChanged);
        _inputReceiver.OnSprintInputChanged.AddListener(DownForceInputChanged);
        _inputReceiver.OnLeftSecondaryInputChanged.AddListener(YawLeftInputchanged);
        _inputReceiver.OnRightSecondaryInputChanged.AddListener(YawRightInputchanged);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!_canLift) return;
        if (_rotorSpeed <= 4000f) return;

        _mainRigidbody.AddForce(-Physics.gravity, ForceMode.Acceleration);

        var thrustVector = Vector3.up * _vehicleInput.y * _thrustSpeed;
        _mainRigidbody.AddForce(thrustVector, ForceMode.Impulse);

        var forwardVector = transform.forward.normalized;
        forwardVector.y = 0f;
        forwardVector *= _vehicleInput.z * _forwardSpeed;

        var strafeVector = transform.right.normalized;
        strafeVector.y = 0f;
        strafeVector *= _vehicleYawInput * _strafeSpeed;

        _mainRigidbody.AddForce(forwardVector + strafeVector, ForceMode.Impulse);

        _pitch = Mathf.Lerp(_pitch, Mathf.Clamp(_vehicleInput.z * 25f, -25f, 25f), Time.fixedDeltaTime * _pitchSpeed);
        _yaw = Mathf.Lerp(_yaw,  (_vehicleInput.x * 2f), Time.fixedDeltaTime * _yawSpeed);
        _roll = Mathf.Lerp(_roll, Mathf.Clamp(-(_vehicleYawInput * 25f), -25f, 25f), Time.fixedDeltaTime * _rollSpeed);

        _mainRigidbody.rotation = Quaternion.Euler(0f, _mainRigidbody.rotation.eulerAngles.y, 0f) * Quaternion.Euler(_pitch, _yaw, _roll);
    }

    protected override void Update()
    {
        base.Update();
        if (_canLift && _rotorSpeed < _maxRotorSpeed)
        {
            _rotorSpeed += _rotorStartupSpeed * Time.deltaTime;
        }
        else if (!_canLift && _rotorSpeed > 0f)
        {
            _rotorSpeed -= _rotorStartupSpeed * Time.deltaTime;
        }
        else if (!_canLift && _rotorSpeed < 0f)
        {
            _rotorSpeed = 0f;
        }

        if (_rotorSpeed != 0f)
        {
            var rotMain = _rotorMain.localRotation;
            rotMain *= Quaternion.Euler(0f, _rotorSpeed * Time.deltaTime, 0f);
            _rotorMain.localRotation = rotMain;

            var rotBack = _rotorBack.localRotation;
            rotBack *= Quaternion.Euler(_rotorSpeed * Time.deltaTime, 0f, 0f);
            _rotorBack.localRotation = rotBack;
        }
    }

    private void MovementInputChanged(Vector2 input)
    {
        var movementInput = input;
        var vehicleInput = _vehicleInput;
        vehicleInput.x = movementInput.x;
        vehicleInput.z = movementInput.y;
        _vehicleInput = vehicleInput;
    }

    private void UpForceInputChanged(bool upInput)
    {
        var upModifier = upInput ? 1f : -1f;
        var vehicleInput = _vehicleInput;
        vehicleInput.y += upModifier;
        _vehicleInput = vehicleInput;
    }

    private void DownForceInputChanged(bool downInput)
    {
        var downModifier = downInput ? -1f : 1f;
        var vehicleInput = _vehicleInput;
        vehicleInput.y += downModifier;
        _vehicleInput = vehicleInput;
    }

    private void YawLeftInputchanged(bool yawInput)
    {
        var leftModifier = yawInput ? -1f : 1f;
        var vehicleYawInput = _vehicleYawInput;
        vehicleYawInput += leftModifier;
        _vehicleYawInput = vehicleYawInput;
    }
    private void YawRightInputchanged(bool yawInput)
    {
        var rightModifier = yawInput ? 1f : -1f;
        var vehicleYawInput = _vehicleYawInput;
        vehicleYawInput += rightModifier;
        _vehicleYawInput = vehicleYawInput;
    }

    public override void DriverEntered(PlayerController driver)
    {
        base.DriverEntered(driver);

        _canLift = true;
    }

    public override void DriverExit()
    {
        base.DriverExit();

        _canLift = false;
    }
}
