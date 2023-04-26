using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void AdjustVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
    }

    public enum ControlSchemes
    {
        KeyboardAndMouse = 0,
        KeyboardOnly,
        Gamepad,
    }
}
