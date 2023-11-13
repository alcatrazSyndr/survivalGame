using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionReceiver : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] protected string _interactText = "Interact";
    [SerializeField] protected string _interactSuffixHotkey = " [ F ]";

    public virtual void Interact(PlayerController player)
    {
    }
}
