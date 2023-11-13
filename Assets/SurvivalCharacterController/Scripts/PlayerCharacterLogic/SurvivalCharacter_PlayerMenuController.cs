using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalCharacter_PlayerMenuController : MonoBehaviour
{
    [SerializeField] private List<PlayerMenu_Menu> _playerMenus = new List<PlayerMenu_Menu>();
    [SerializeField] private GameObject _playerMenuCanvas;
    [SerializeField] private PlayerMenu_Menu _lastOpenedMenu = null;
    [SerializeField] private InputController _inputController;

    private void Start()
    {
        _inputController.OnPlayerMenuInput.AddListener(ToggleMenu);
    }

    private void ToggleMenu()
    {
        var toggle = _playerMenuCanvas.activeSelf;
        if (toggle)
        {
            Cursor.lockState = CursorLockMode.Locked;
            ClosePlayerMenu();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            OpenPlayerMenu();
        }
    }

    private void OpenPlayerMenu()
    {
        _playerMenuCanvas.SetActive(true);

        if (_lastOpenedMenu == null)
            _lastOpenedMenu = _playerMenus[0];

        _lastOpenedMenu.OpenMenu();
    }

    private void ClosePlayerMenu()
    {
        foreach (var playerMenu in _playerMenus)
        {
            playerMenu.CloseMenu();
        }

        _playerMenuCanvas.SetActive(false);
    }

    public void MenuOpened(PlayerMenu_Menu menu)
    {
        foreach (var playerMenu in _playerMenus)
        {
            if (playerMenu != menu)
                playerMenu.CloseMenu();
        }

        _lastOpenedMenu = menu;
    }
}
