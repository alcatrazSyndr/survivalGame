using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionReceiver_Vehicle : InteractionReceiver
{
    [Header("Component References")]
    [SerializeField] private VehicleController _vehicleController;

    public override void Interact(PlayerController player)
    {
        base.Interact(player);

        var inputReceiver = transform.GetComponent<InputReceiver>();
        if (inputReceiver != null)
            player.ChangeInputReceiver(transform.GetComponent<InputReceiver>());

        _vehicleController.DriverEntered(player);
    }
}
