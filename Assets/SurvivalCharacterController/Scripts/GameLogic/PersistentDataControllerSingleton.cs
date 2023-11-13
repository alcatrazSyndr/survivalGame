using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDataControllerSingleton : MonoBehaviour
{
    public static PersistentDataControllerSingleton Instance { get; private set; }

    [SerializeField] private GameDataSO _gameData;
    public GameDataSO GameData { get { return _gameData; } }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
        else
        {
            Instance = this;
        }
    }
}
