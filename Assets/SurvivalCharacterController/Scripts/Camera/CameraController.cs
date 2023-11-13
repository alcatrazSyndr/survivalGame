using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Transform References")]
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Transform _cameraArmX;
    [SerializeField] private Transform _cameraArmY;
    [SerializeField] private Transform _cameraTransform;

    [Header("Components")]
    [SerializeField] private PlayerController _playerController;

    [Header("Runtime")]
    private Vector3 _cameraTransformLocalPositionTarget = new Vector3(0f, 0f, -4f);

    private void Start()
    {
        _playerController.OnInputReceiverChanged.AddListener(InputReceiverChanged);
        _playerController.PlayerInputController.OnMouseInputChanged.AddListener(CameraInputChanged);
    }

    private void Update()
    {
        transform.position = _cameraTarget.position;
        if (_cameraTransform.localPosition != _cameraTransformLocalPositionTarget)
            _cameraTransform.localPosition = _cameraTransformLocalPositionTarget;
    }

    private void CameraInputChanged(Vector2 cameraInput)
    {
        if (Cursor.lockState != CursorLockMode.None)
        {
            _cameraArmY.localRotation *= Quaternion.Euler(new Vector3(0f, cameraInput.x * 1.5f, 0f));
            _cameraArmX.localRotation *= Quaternion.Euler(new Vector3(-cameraInput.y * 1.5f, 0f, 0f));
        }
    }

    private void InputReceiverChanged(InputReceiver inputReceiver)
    {
        if (inputReceiver != null && inputReceiver.CameraTargetTransform != null)
        {
            _cameraTarget = inputReceiver.CameraTargetTransform;
            _cameraTransformLocalPositionTarget = inputReceiver.CameraTransformLocalPositionTarget;
        }
    }
}
