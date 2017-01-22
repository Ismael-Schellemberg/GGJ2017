using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvulnerablePowerUp :  PowerUp {

    protected override void OnHit() {
        player.ActivateInvulnerablity();
    }
}
