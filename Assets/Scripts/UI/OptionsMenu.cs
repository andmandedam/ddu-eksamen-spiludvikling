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

    public void ChangeControlScheme(Int32 controlScheme)
    {
        //switch (controlScheme)
        //{
        //    case (int)ControlSchemes.KeyboardAndMouse:
        //        {

        //        }
        //}
    }

    public enum ControlSchemes
    {
        KeyboardAndMouse = 0,
        KeyboardOnly,
        Gamepad,
    }
}
