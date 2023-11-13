using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenu_Inventory : PlayerMenu_Menu
{
    [SerializeField] private Transform _itemSlotRoot;
    [SerializeField] private GameObject _itemSlotPrefab;

    protected override void Start()
    {
        base.Start();
    }

    public override void CloseMenu()
    {
        foreach (Transform child in _itemSlotRoot)
        {
            Destroy(child.gameObject);
        }

        base.CloseMenu();
    }

    public override void OpenMenu()
    {
        base.OpenMenu();

        for (int i = 0; i < _playerController.TotalInventorySlots(); i++)
        {
            var itemSlotGO = Instantiate(_itemSlotPrefab, _itemSlotRoot);
        }
    }
}
