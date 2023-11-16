using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputReceiver : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent<Vector2> OnMouseInputChanged = new UnityEvent<Vector2>();
    public UnityEvent<Vector2> OnMovementInputChanged = new UnityEvent<Vector2>();
    public UnityEvent<Vector2> OnCameraRelativeMovementInputChanged = new UnityEvent<Vector2>();
    public UnityEvent<bool> OnSprintInputChanged = new UnityEvent<bool>();
    public UnityEvent<bool> OnJumpInputChanged = new UnityEvent<bool>();
    public UnityEvent<bool> OnLeftSecondaryInputChanged = new UnityEvent<bool>();
    public UnityEvent<bool> OnRightSecondaryInputChanged = new UnityEvent<bool>();
    public UnityEvent OnPlayerMenuInput = new UnityEvent();
    public UnityEvent OnInteractionInput = new UnityEvent();

    [Header("Data")]
    [SerializeField] private Transform _cameraTargetTransform = null;
    public Transform CameraTargetTransform { get { return _cameraTargetTransform; } }
    [SerializeField] private float _cameraTransformPivotDistance = 4f;
    public Vector3 CameraTransformLocalPositionTarget { get { return new Vector3(0f, 0f, -_cameraTransformPivotDistance); } }
}
