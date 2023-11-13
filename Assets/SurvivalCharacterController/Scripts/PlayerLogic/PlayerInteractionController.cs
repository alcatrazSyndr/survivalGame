using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private PlayerController _playerController;

    [Header("Data")]
    [SerializeField] private float _maxInteractionDistance = 5f;
    [SerializeField] private LayerMask _interactionLayerMasks;

    [Header("Runtime")]
    private InteractionReceiver _currentInteractionReceiver = null;

    private void Start()
    {
        _playerController.PlayerInputController.OnInteractionInput.AddListener(InteractionInput);
    }

    private void Update()
    {
        if (!_playerController.CanInteract) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _maxInteractionDistance, _interactionLayerMasks.value))
        {
            var interactionReceiver = hit.transform.GetComponent<InteractionReceiver>();

            if (_currentInteractionReceiver != interactionReceiver)
                SetNewInteractionReceiver(interactionReceiver);
        }
        else if (_currentInteractionReceiver != null)
        {
            SetNewInteractionReceiver(null);
        }
    }

    private void SetNewInteractionReceiver(InteractionReceiver interactionReceiver)
    {
        _currentInteractionReceiver = interactionReceiver;
    }

    private void InteractionInput()
    {
        if (_currentInteractionReceiver == null) return;

        _currentInteractionReceiver.Interact(_playerController);
        _currentInteractionReceiver = null;
    }
}
