using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUp {

    protected override void OnHit() {
        player.ActivateSpeed();
    }
}
