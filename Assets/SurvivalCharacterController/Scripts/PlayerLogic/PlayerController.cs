using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int TotalInventorySlots()
    {
        var baseInventorySlotCount = PersistentDataControllerSingleton.Instance.GameData.DefaultCharacterInventorySlots;

        var totalInventorySlots = baseInventorySlotCount;
        return totalInventorySlots;
    }
}
