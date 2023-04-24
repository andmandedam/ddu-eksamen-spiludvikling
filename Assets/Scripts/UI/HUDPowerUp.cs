using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDPowerUp : MonoBehaviour
{
    [SerializeField] private Image powerUpImage;

    public void SetPowerUpSprite(Sprite powerUpSprite)
    {
        powerUpImage.enabled = true;
        powerUpImage.sprite = powerUpSprite;
    }
}
