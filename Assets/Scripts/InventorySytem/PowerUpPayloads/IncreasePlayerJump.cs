using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasePlayerJump : PowerUpPayload
{
    public void ActivatePayload(Player.PlayerJump player)
    {
        player.jumpCount += 1;
    }
}
