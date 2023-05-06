using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    private Int32 currentResolutionOption = 0;
    private bool isFullscreen = true;
    private Vector2Int[] resolutions = new Vector2Int[]
    {
        new Vector2Int(1920, 1080),
        new Vector2Int(1280, 720),
        new Vector2Int(640, 360),
        new Vector2Int(1920, 1440),
        new Vector2Int(1280, 960),
        new Vector2Int(800, 600),
    };
    public void SetFullscreen(bool fullscreen)
    {
        isFullscreen = fullscreen;
        Screen.SetResolution(resolutions[currentResolutionOption].x, resolutions[currentResolutionOption].y, fullscreen);
    }

    public void AdjustVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
    }

    public void SetResolution(Int32 selectedOption)
    {
        currentResolutionOption = selectedOption;
        Screen.SetResolution(resolutions[selectedOption].x, resolutions[selectedOption].y, isFullscreen);
    }

}
