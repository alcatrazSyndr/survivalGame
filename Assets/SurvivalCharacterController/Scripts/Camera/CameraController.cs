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
    [SerializeField] private InputController _inputController;

    private void Start()
    {
        _inputController.OnMouseInputChanged.AddListener(CameraInputChanged);
    }

    private void Update()
    {
        transform.position = _cameraTarget.position;
    }

    private void CameraInputChanged(Vector2 cameraInput)
    {
        if (Cursor.lockState != CursorLockMode.None)
        {
            _cameraArmY.localRotation *= Quaternion.Euler(new Vector3(0f, cameraInput.x * 1.5f, 0f));
            _cameraArmX.localRotation *= Quaternion.Euler(new Vector3(-cameraInput.y * 1.5f, 0f, 0f));
        }
    }
}
