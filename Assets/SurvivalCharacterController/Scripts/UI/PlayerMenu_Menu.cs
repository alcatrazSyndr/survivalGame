using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu_Menu : MonoBehaviour
{
    [SerializeField] protected PlayerController _playerController;
    [SerializeField] protected SurvivalCharacter_PlayerMenuController _menuController;
    [SerializeField] protected Button _navigationMenuButton;
    [SerializeField] protected GameObject _menuView;

    protected virtual void Start()
    {
        if (_navigationMenuButton != null)
            _navigationMenuButton.onClick.AddListener(OpenMenu);
    }

    public virtual void OpenMenu()
    {
        _menuController.MenuOpened(this);
        _menuView.SetActive(true);
    }

    public virtual void CloseMenu()
    {
        _menuView.SetActive(false);
    }
}
