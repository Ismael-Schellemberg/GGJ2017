using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPowerUp :  PowerUp {

    protected override void OnHit() {
        player.ActivateMagnet();
    }
}
