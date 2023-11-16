using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Vehicle Physics Components")]
    [SerializeField] protected Rigidbody _mainRigidbody;
    [SerializeField] protected Collider _mainCollider;

    [Header("Vehicle Visuals")]
    [SerializeField] protected Transform _mainRigidbodyVisual;

    [Header("Vehicle Data")]
    [SerializeField] protected List<Transform> _vehicleSeats;
    [SerializeField] protected List<Transform> _exitPoints;
    [SerializeField] protected Vector3 _centerOfMassOffset;

    [Header("Component References")]
    [SerializeField] protected InputReceiver _inputReceiver;

    [Header("Runtime")]
    protected PlayerController _driver = null;

    protected virtual void Start()
    {
        _mainRigidbody.centerOfMass = _centerOfMassOffset;

        _inputReceiver.OnInteractionInput.AddListener(DriverExit);
    }

    protected virtual void FixedUpdate()
    {
    }

    protected virtual void Update()
    {
        var pos = _mainRigidbody.position;
        var rot = _mainRigidbody.rotation;
        _mainRigidbodyVisual.rotation = rot;
        _mainRigidbodyVisual.position = pos;
    }

    public virtual void DriverEntered(PlayerController driver)
    {
        _driver = driver;

        Physics.IgnoreCollision(driver.CharacterController.CharacterCollider, _mainCollider, true);
        driver.CharacterController.FixToRigidbody(_mainRigidbody, _vehicleSeats[0].position, _vehicleSeats[0].rotation);
    }

    public virtual void DriverExit()
    {
        if (_driver == null) return;

        _driver.CharacterController.FixToRigidbody(null, _exitPoints[0].position, Quaternion.identity);
        Physics.IgnoreCollision(_driver.CharacterController.CharacterCollider, _mainCollider, false);

        _driver.ResetInputReceiverToCharacter();
        _driver = null;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + new Vector3(_mainRigidbody.centerOfMass.x, -_mainRigidbody.centerOfMass.y, _mainRigidbody.centerOfMass.z), 0.3f);
    }
}
