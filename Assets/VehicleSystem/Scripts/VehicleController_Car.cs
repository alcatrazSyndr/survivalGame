using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController_Car : VehicleController
{
    [Header("Car Wheel Controllers")]
    [SerializeField] private WheelCollider _leftFront;
    [SerializeField] private WheelCollider _rightFront;
    [SerializeField] private WheelCollider _leftBack;
    [SerializeField] private WheelCollider _rightBack;

    [Header("Car Wheel Visuals")]
    [SerializeField] private Transform _leftFrontVisual;
    [SerializeField] private Transform _rightFrontVisual;
    [SerializeField] private Transform _leftBackVisual;
    [SerializeField] private Transform _rightBackVisual;

    [Header("Car Vehicle Data")]
    [SerializeField] private float _torque;
    [SerializeField] private float _brakeForce;
    [SerializeField] private float _maxSteeringAngle;
    [SerializeField] private bool _fwd = false;
    [SerializeField] private bool _rwd = false;
    [SerializeField] private float _maxVelocity = 60f;

    [Header("Car Runtime")]
    private Vector2 _vehicleInput = Vector2.zero;
    private bool _brakeInput = false;

    protected override void Start()
    {
        base.Start();

        _inputReceiver.OnMovementInputChanged.AddListener(VehicleInputChanged);
        _inputReceiver.OnJumpInputChanged.AddListener(BrakeInputChanged);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        var velocity = _mainRigidbody.velocity.magnitude;
        if (_fwd)
        {
            _leftFront.motorTorque = velocity < _maxVelocity ? _vehicleInput.y * _torque : 0f;
            _rightFront.motorTorque = velocity < _maxVelocity ? _vehicleInput.y * _torque : 0f;
        }
        if (_rwd)
        {
            _leftBack.motorTorque = velocity < _maxVelocity ? _vehicleInput.y * _torque : 0f;
            _rightBack.motorTorque = velocity < _maxVelocity ? _vehicleInput.y * _torque : 0f;
        }

        var brakeForce = _brakeInput ? _brakeForce : 0f;
        _leftBack.brakeTorque = brakeForce;
        _leftFront.brakeTorque = brakeForce;
        _rightBack.brakeTorque = brakeForce;
        _rightFront.brakeTorque = brakeForce;

        var steerAngle = _maxSteeringAngle * _vehicleInput.x;
        _leftFront.steerAngle = steerAngle;
        _rightFront.steerAngle = steerAngle;
        _leftBack.steerAngle = 0.1f * -steerAngle;
        _rightBack.steerAngle = 0.1f * -steerAngle;
    }

    protected override void Update()
    {
        base.Update();

        UpdateWheelVisual(_leftBack, _leftBackVisual);
        UpdateWheelVisual(_leftFront, _leftFrontVisual);
        UpdateWheelVisual(_rightFront, _rightFrontVisual);
        UpdateWheelVisual(_rightBack, _rightBackVisual);
    }

    private void VehicleInputChanged(Vector2 input)
    {
        _vehicleInput = input;
    }

    private void BrakeInputChanged(bool brake)
    {
        _brakeInput = brake;
    }

    private void UpdateWheelVisual(WheelCollider wheel, Transform visual)
    {
        Vector3 pos;
        Quaternion rot;
        wheel.GetWorldPose(out pos, out rot);
        visual.position = pos;
        visual.rotation = rot;
    }

    public override void DriverEntered(PlayerController driver)
    {
        base.DriverEntered(driver);

        _brakeInput = false;
    }

    public override void DriverExit()
    {
        base.DriverExit();

        _brakeInput = true;
    }
}
