using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Vehicle Physics Components")]
    [SerializeField] private WheelCollider _leftFront;
    [SerializeField] private WheelCollider _rightFront;
    [SerializeField] private WheelCollider _leftBack;
    [SerializeField] private WheelCollider _rightBack;
    [SerializeField] private Rigidbody _mainRigidbody;
    [SerializeField] private Collider _mainCollider;

    [Header("Vehicle Visuals")]
    [SerializeField] private Transform _leftFrontVisual;
    [SerializeField] private Transform _rightFrontVisual;
    [SerializeField] private Transform _leftBackVisual;
    [SerializeField] private Transform _rightBackVisual;
    [SerializeField] private Transform _mainRigidbodyVisual;

    [Header("Vehicle Data")]
    [SerializeField] private float _torque;
    [SerializeField] private float _brakeForce;
    [SerializeField] private float _maxSteeringAngle;
    [SerializeField] private float _centerOfMassModifier = 1f;
    [SerializeField] private bool _fwd = false;
    [SerializeField] private bool _rwd = false;
    [SerializeField] private float _maxVelocity = 60f;
    [SerializeField] private List<Transform> _vehicleSeats;
    [SerializeField] private List<Transform> _exitPoints;

    [Header("Component References")]
    [SerializeField] private InputReceiver _inputReceiver;

    [Header("Runtime")]
    private Vector2 _vehicleInput = Vector2.zero;
    private bool _brakeInput = false;
    private PlayerController _driver = null;

    private void Start()
    {
        var com = _mainRigidbody.centerOfMass;
        com.y -= _centerOfMassModifier;
        _mainRigidbody.centerOfMass = com;

        _inputReceiver.OnMovementInputChanged.AddListener(VehicleInputChanged);
        _inputReceiver.OnJumpInputChanged.AddListener(BrakeInputChanged);
        _inputReceiver.OnInteractionInput.AddListener(DriverExit);
    }

    private void FixedUpdate()
    {
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

    private void Update()
    {
        UpdateWheelVisual(_leftBack, _leftBackVisual);
        UpdateWheelVisual(_leftFront, _leftFrontVisual);
        UpdateWheelVisual(_rightFront, _rightFrontVisual);
        UpdateWheelVisual(_rightBack, _rightBackVisual);
        var pos = _mainRigidbody.position;
        var rot = _mainRigidbody.rotation;
        _mainRigidbodyVisual.rotation = rot;
        _mainRigidbodyVisual.position = pos;
    }

    private void UpdateWheelVisual(WheelCollider wheel, Transform visual)
    {
        Vector3 pos;
        Quaternion rot;
        wheel.GetWorldPose(out pos, out rot);
        visual.position = pos;
        visual.rotation = rot;
    }

    private void VehicleInputChanged(Vector2 input)
    {
        _vehicleInput = input;
    }

    private void BrakeInputChanged(bool brake)
    {
        _brakeInput = brake;
    }

    public void DriverEntered(PlayerController driver)
    {
        _driver = driver;

        Physics.IgnoreCollision(driver.CharacterController.CharacterCollider, _mainCollider, true);
        driver.CharacterController.FixToRigidbody(_mainRigidbody, _vehicleSeats[0].position, _vehicleSeats[0].rotation);
    }

    private void DriverExit()
    {
        if (_driver == null) return;

        _driver.CharacterController.FixToRigidbody(null, _vehicleSeats[0].position, _vehicleSeats[0].rotation);
        Physics.IgnoreCollision(_driver.CharacterController.CharacterCollider, _mainCollider, false);

        _driver.ResetInputReceiverToCharacter();
        _driver = null;
    }
}
