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
    public UnityEvent OnPlayerMenuInput = new UnityEvent();
}
