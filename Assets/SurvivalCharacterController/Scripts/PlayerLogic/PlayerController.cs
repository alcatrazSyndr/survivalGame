using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private SurvivalCharacter_PlayerMenuController _menuController;
    [SerializeField] private InputReceiver _characterInputReceiver;
    [SerializeField] private InputController _inputController;
    public InputController PlayerInputController { get { return _inputController; } }
    [SerializeField] private SurvivalCharacter_CharacterController _characterController;
    public SurvivalCharacter_CharacterController CharacterController { get { return _characterController; } }

    [Header("Events")]
    public UnityEvent<InputReceiver> OnInputReceiverChanged = new UnityEvent<InputReceiver>();

    public bool CanInteract 
    { 
        get 
        {
            var inMenu = !_menuController.InMenu;
            var controllingCharacter = _inputController.CurrentInputReceiver == _characterInputReceiver;

            var canInteract = inMenu && controllingCharacter;
            return canInteract;
        }
    }

    private void Start()
    {
        ChangeInputReceiver(_characterInputReceiver);
    }

    public int TotalInventorySlots()
    {
        var baseInventorySlotCount = PersistentDataControllerSingleton.Instance.GameData.DefaultCharacterInventorySlots;

        var totalInventorySlots = baseInventorySlotCount;
        return totalInventorySlots;
    }

    public void ChangeInputReceiver(InputReceiver inputReceiver)
    {
        _inputController.SetInputReceiver(inputReceiver);
        OnInputReceiverChanged?.Invoke(inputReceiver);
    }

    public void ResetInputReceiverToCharacter()
    {
        _inputController.SetInputReceiver(_characterInputReceiver);
        OnInputReceiverChanged?.Invoke(_characterInputReceiver);
    }
}
